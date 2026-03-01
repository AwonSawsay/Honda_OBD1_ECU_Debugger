//
//      Moates Ostrich C# handler v1.0
//      A partially implemented class tested with Moates Ostrich 2.0 ROM Emulator



// Attach event handler to track emulator events
//Emulator.PropertyChanged += this.OnPropertyChanged;

// Add the following function to the main program class to act upon notify events
/*void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
{
    // Serial Number changed
    if (e.PropertyName == "OstrichSerialString")
    {
        // Update the serial number tbox
        this.Invoke(new EventHandler(DisplayOstrichSerialNumber));
    }

    // Ostrich reported an error
    if (e.PropertyName == "OstrichError" && Emulator.OstrichError)
    {
        // Update the serial number tbox
        this.Invoke(new EventHandler(DisplayOstrichError));
        cboxAutoStep.Checked = false;
    }

    // Ostrich ROM Dump is complete
    if (e.PropertyName == "EmuDumpReady" && Emulator.EmuDumpReady)
    {
        // EMU dump complete.
    }
}
    */





using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.IO;


namespace ECU_Debugger
{
    public class Emulator : INotifyPropertyChanged
    {
        SerialPort Ostrich;
        int StartChunkAddress = 0;
        byte[] bulkOutput;
        int BulkWritePacket = 0;

        List<int> OstrichRXBuffer = new List<int>();
        int OstrichStatus;
        byte[] OstrichSerialNumber = new byte[9];
        private bool ostrichError;
        private bool emuDumpReady;
        public event PropertyChangedEventHandler PropertyChanged;
        private string ostrichSerialString;
        
        // Ostrich Reading
        byte[] emuDump; 
        byte[] emuDumpBuffer;
        int emuDumpBufferIndex = 0;
        int emuDumpMainIndex = 0;
        int emuDumpCHK = 0;
        int bulkReadStart = 0;
        int bulkReadTotalBlocks = 0;
        int bulkReadBlocksPerPacket = 0;
        int bulkReadFinish = 0;
        bool bulkReadWriteToFile = false;

        // INotifyPropertyChange Variable
        public bool OstrichError
        {
            get { return ostrichError; }
            set
            {
                ostrichError = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
            }
        }
        // INotifyPropertyChange Variable
        public bool EmuDumpReady
        {
            get { return emuDumpReady; }
            set
            {
                emuDumpReady = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
            }
        }
        // INotifyPropertyChange Variable
        public string OstrichSerialString
        {
            get { return ostrichSerialString; }
            set
            {
                ostrichSerialString = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
            }
        }

        // Flags other class that important variables have changed
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        // Opens emulator serial port
        public SerialPort OstrichOpenPort(string portName)
        {
            OstrichClosePort();
            Ostrich = new SerialPort();
            try
            {

                Ostrich.PortName = portName;
                Ostrich.BaudRate = 921600;
                Ostrich.DataBits = 8;
                Ostrich.StopBits = StopBits.One;
                Ostrich.Parity = Parity.None;
                Ostrich.RtsEnable = false;
                Ostrich.ReadTimeout = 500;
                Ostrich.Open();
                OstrichRequestSerialAndVID();
                Ostrich.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Ostrich_DataReceived);

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Ostrich;
        }

        // Closes open emulator serial port
        public void OstrichClosePort()
        {
            if (Ostrich != null && Ostrich.IsOpen)
            {
                Ostrich.DiscardInBuffer();
                Ostrich.DiscardOutBuffer();
                Ostrich.Close();
            }
        }
        
        // sends request for serial number and vendor ID to emulator
        public void OstrichRequestSerialAndVID()
        {
            OstrichSerialString = "";
            byte[] OstrichRequestSerial = new byte[3] { 78, 83, 161 };
            Ostrich.Write(OstrichRequestSerial, 0, 3);
            OstrichStatus = 1;
            ostrichError = false;
        }
           
        // Serial RX handler
        private void Ostrich_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            OstrichRXBuffer.Clear();
            int CurrentByte;
            int OstrichRXChecksum = 0;
            int ByteCount = 0;

            while (OstrichStatus != 0 && Ostrich.IsOpen)
            //while (Ostrich.BytesToRead > 0) 
            {

                try
                {
                    CurrentByte = Ostrich.ReadByte();
                    OstrichRXChecksum = OstrichRXChecksum + CurrentByte;
                    ByteCount++;
                    
                    if (CurrentByte == 63 && OstrichStatus != 3) // ? sent from ostrich, indicating an error (ignored during bulk read)
                    {
                        OstrichError = true;
                        OstrichStatus = 0;
                    }
                    switch (OstrichStatus)
                    {
                        //Packet status 1 = Looking For serial number
                        case 1:
                            if (ByteCount == 10)
                            {
                                OstrichRXChecksum = OstrichRXChecksum - CurrentByte;
                                ushort WordCheckSum = Convert.ToUInt16(OstrichRXChecksum);
                                byte ByteCheckSum = (byte)(WordCheckSum & 0xFF);
                                OstrichStatus = 0;
                                
                                // Serial number recd. update string
                                OstrichSerialString = OstrichRXDataFormat(OstrichRXBuffer);
                                OstrichRXBuffer.Clear();
                            }
                            else
                            {
                                OstrichRXBuffer.Add(CurrentByte);
                                OstrichSerialNumber[ByteCount - 1] = (byte)CurrentByte;
                            }
                        break;
                        
                            //Currently bulk sending, awaiting RX confirmation
                        case 2:
                            if (CurrentByte == 79) //Got good response from ostrich, Continue upload
                            {
                                OstrichContinueBulkWrite();
                            }
                        break;
                        
                            // Waiting for bulk read 
                        case 3:
                            // Full packet recd, and this is checksum?
                            if (emuDumpBufferIndex == bulkReadBlocksPerPacket * 256)
                            {
                                // Yes, compare checksums
                                emuDumpCHK &= 0xff;                                
                                
                                // Checksums Match?
                                if (CurrentByte == (byte)emuDumpCHK)
                                {
                                    // Packet complete, status is idle. Request next packet
                                    OstrichStatus = 0;
                                    OstrichContinueBulkRead();
                                    return;
                                }
                                else
                                {
                                    // Checksum fail, status is idle
                                    MessageBox.Show("Checksum mismatch while dumping emulator.");
                                    OstrichStatus = 0;
                                    return;
                                }
                            }
                            // Packet data received. update array, index, and checksum
                            emuDumpBuffer[emuDumpBufferIndex] = (byte)CurrentByte;
                            emuDumpCHK += CurrentByte;
                            emuDumpBufferIndex++;


                        break;

                        default:
                        break;
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                    ostrichError = true;
                    OstrichStatus = 0;

                }
            }

        }

        // Writes entire ROM quickly
        public void OstrichWriteBulk(byte[] SourceArray, bool quietBulkWrite = false, bool firstWrite = false)
        {
            // Is this the first write?
            if(firstWrite)OstrichStatus = 0;
            
            // Check Array Size is 32kb
            if (SourceArray.Length != 32768)
            {
                MessageBox.Show("Invalid BIN Size. Bin must be 32768 bytes.");
                OstrichStatus = 0;
                return;
            }
            
            // Check that port is open
            if (!Ostrich.IsOpen)
            {
                MessageBox.Show("Ostrich COM Port not opened");
                OstrichStatus = 0;
                return;
            }
            
            // First write setup
            if (OstrichStatus == 0)
            {
                bulkOutput = new byte[SourceArray.Length];
                Array.Copy(SourceArray, bulkOutput, SourceArray.Length);
                BulkWritePacket = 0;
                StartChunkAddress = 0;
                OstrichStatus = 0;
                ostrichError = false;
            }

            try
            {
                if (BulkWritePacket < 8)
                {
                    
                    // Create 4kb array with room for packet header and CS
                    byte[] OutputArray = new byte[4102];
                    
                    // Create Header - fast write, 16 * 256 blocks per write, starting at 0x8000
                    byte[] Header = new byte[5] { (byte)'Z', (byte)'W', 16, 0, (byte)(0x80 + (BulkWritePacket * 0x10)) };
                    
                    //Copy header and data to output array
                    Array.Copy(Header, 0, OutputArray, 0, Header.Length);
                    Array.Copy(SourceArray, StartChunkAddress, OutputArray, 5, 4096);

                    // checksum
                    int Checksum = 0;
                    foreach (byte element in OutputArray)
                    {
                        Checksum = Checksum + element;
                    }
                    byte[] ChecksumByte = BitConverter.GetBytes(Checksum);
                    
                    //add checksum to the end of the packet
                    OutputArray[OutputArray.Length - 1] = ChecksumByte[0];
                    
                    // Status = Looking for RX confirmation
                    OstrichStatus = 2;
                    
                    // Send packet
                    Ostrich.Write(OutputArray, 0, OutputArray.Length);
                    BulkWritePacket++;
                }
                
                // Sent 8 packets. Send complete
                else
                {
                    if (!quietBulkWrite) MessageBox.Show("BIN Uploaded to Ostrich without error.");
                    quietBulkWrite = false;
                    OstrichStatus = 0;
                }
               
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message + "A problem while trying to write to emulator");
            }
        }

        //Continues bulk write function after confirmation recd from emulator
        private void OstrichContinueBulkWrite()
        {
            StartChunkAddress = StartChunkAddress + 4096;
            OstrichWriteBulk(bulkOutput);
        }

        //Slow write entire rom to ostrich 
        public void OstrichSlowWriteEntireRom(byte[] SourceRomArray)
        {
            int WriteStartAddress = 0;
            byte ByteBlockSize = 255;
            OstrichError = false;
            while (WriteStartAddress < 32513 && !OstrichError)
            {
                OstrichWriteSpecific_256Max(SourceRomArray, WriteStartAddress, WriteStartAddress, ByteBlockSize, false);
                Ostrich.ReadTimeout = 250;
                int OstrichResponse = Ostrich.ReadByte();
                if (OstrichResponse != 79)
                {
                    MessageBox.Show("Bad response from Ostrich after write request");
                    OstrichError = true;
                }
                WriteStartAddress = WriteStartAddress + 256;
            }
            MessageBox.Show("Slow write completed.");

        }

        // Ostrich write function from array up to 256 bytes
        public void OstrichWriteSpecific_256Max(byte[] SourceArray, int EMUStartAddress, int SourceArrayStartAddress = 0, int BytesToWrite = 0, bool WaitForResponse = true)
        {

            if (Ostrich == null || !Ostrich.IsOpen)
            {
                MessageBox.Show("Open Ostrich COM port first.");
                OstrichError = true;
                return;
            }
            Ostrich.DiscardInBuffer();
            OstrichError = false;

            byte[] OutputArray = new byte[261];
            prepareOstrichWritePacket(SourceArray, ref OutputArray, EMUStartAddress, SourceArrayStartAddress, BytesToWrite);

            //Perform write
            Ostrich.Write(OutputArray, 0, OutputArray.Length);

            //Return if not waiting for response from EMU
            if (!WaitForResponse)
            {
                return;
            }

            BlockWhileWaitingForResponse();

        }

        // Poll serial port continuously until response or timeout
        private void BlockWhileWaitingForResponse()
        {
            //Wait for EMU response
            int ReadAttempts = 0;
            try
            {
                Ostrich.ReadTimeout = 500;
                int OstrichResponse = 0;
                //Read from port until a response is received or too many timeouts
                while (OstrichResponse == 0 || OstrichResponse == -1)
                {
                    OstrichResponse = Ostrich.ReadByte();
                }

                //Recevid a byte other than 'O'
                if (OstrichResponse != 79)
                {
                    MessageBox.Show("Bad response from Ostrich after write request");
                    OstrichError = true;
                }
            }
            catch (Exception error)
            {
                //Too many timeouts
                ReadAttempts++;
                if (ReadAttempts > 50)
                {
                    MessageBox.Show(error.Message + "\r\n" + "If this continues to happen, you may need to unplug and replug the Ostrich and re-open the port");
                    OstrichError = true;
                }

            }
        }

        // Send byte [] up to 256 bytes
        public void OstrichWrite(byte[] source, int EmuAddress, bool WaitForResponse)
        {
            if(source.Length > 256 || source.Length == 0)
            {
                MessageBox.Show("Write function limited to 0-256 bytes.", "Error");
                return;
            }
            if (!Ostrich.IsOpen)
            {
                MessageBox.Show("Ostrich port not opened.");
                return;
            }
            
            // Clear RX and reset ostrich status
            Ostrich.DiscardInBuffer();
            OstrichError = false;
            OstrichStatus = 0;
            
            // Prepare packet header
            EmuAddress += 0x8000;
            byte MSB = (byte)(EmuAddress >> 8);
            byte LSB = (byte)(EmuAddress & 0xFF);
            byte[] header = new byte[4] { (byte) 'W' , (byte) source.Length , MSB , LSB };
            
            // Combine data and header
            byte[] output = new byte[source.Length + header.Length + 1];
            Array.Copy(header,output,header.Length);
            Array.Copy(source,0,output,header.Length - 1,source.Length);
            
            // Add checksum
            output[output.Length - 1] = 0x00;
            int checksum = 0;
            foreach (byte item in output)
            {
                checksum += item;
            }
            output[output.Length - 1] = (byte)(checksum & 0xff);

            //Send packet
            Ostrich.Write(output, 0, output.Length);

            // End function if we're not waiting
            if (!WaitForResponse) return;

            // Check for response repeatedly until timeout
            BlockWhileWaitingForResponse();
        }
        // Helper function to convert data into emulator packet
        private void prepareOstrichWritePacket(byte[] SourceArray, ref byte[] OutputArray, int EMUStartAddress, int SourceArrayStartAddress = 0, int BytesToWrite = 0)
        {
            //If 0 byte count , then send entire source array (256 byte max)
            if (BytesToWrite == 0)
            {
                BytesToWrite = Convert.ToInt32(SourceArray.Length);
                if (BytesToWrite > 256)
                {
                    MessageBox.Show("Failed trying to write " + BytesToWrite + " bytes to 256 byte buffer.");
                    return;
                }
            }

            OutputArray = new byte[BytesToWrite + 5];
            EMUStartAddress = EMUStartAddress + 0x8000;
            byte MSB = (byte)(EMUStartAddress >> 8);
            byte LSB = (byte)(EMUStartAddress & 0xFF);

            int packetIndex = 0;
            bool LoopDone = false;
            while (!LoopDone)
            {

                switch (packetIndex)
                {
                    //W for write
                    case 0:
                        OutputArray[packetIndex] = 87;
                        break;
                    //Bytes to send
                    case 1:
                        OutputArray[packetIndex] = (byte)BytesToWrite;
                        break;
                    //Start address high byte
                    case 2:
                        OutputArray[packetIndex] = MSB;
                        break;
                    //Start address low byte
                    case 3:
                        OutputArray[packetIndex] = LSB;
                        break;
                    //Data bytes
                    case 4:
                        Array.Copy(SourceArray, SourceArrayStartAddress, OutputArray, packetIndex, BytesToWrite);
                        break;
                    //Checksum byte
                    case 5:
                        int Checksum = 0;
                        foreach (byte element in OutputArray)
                        {
                            Checksum = Checksum + element;
                        }

                        OutputArray[OutputArray.Length - 1] = (byte)Checksum;
                        LoopDone = true;
                        break;
                }
                packetIndex++;
            }
        }

        // Fast read of entire EMU. Reads in 16*256 byte packets. option to save to dated file
        public void OstrichBulkReadEntire(bool writeToFile = false , bool size64KB = false)
        {
            bulkReadWriteToFile = writeToFile;
            EmuDumpReady = false;
            OstrichStatus = 3;

            bulkReadStart = 0x8000;
            bulkReadTotalBlocks = 128;
            bulkReadBlocksPerPacket = 16;
            if (size64KB)
            {
                bulkReadStart = 0;
                bulkReadTotalBlocks = 256;
            }
            bulkReadFinish = bulkReadStart + (bulkReadTotalBlocks * 256);

            // Prepare array and index
            emuDumpBufferIndex = 0;
            emuDumpMainIndex = 0;
            emuDumpBuffer = new byte[bulkReadBlocksPerPacket * 256];
            emuDump = new byte[bulkReadTotalBlocks * 256];

            // send request for first 16 blocks
            OstrichBulkReadRequest(bulkReadStart, bulkReadBlocksPerPacket);
        }

        // Continue already started bulk read. called from serial handler after emu confirmation recd
        private void OstrichContinueBulkRead()
        {
            if (emuDumpMainIndex < bulkReadTotalBlocks * 256)
            {
                // Copy packet to main array
                Array.Copy(emuDumpBuffer, 0, emuDump, emuDumpMainIndex, emuDumpBuffer.Length);
            }
            
            // Adjust address and index for next packet
            int offset = (bulkReadBlocksPerPacket * 256);
            bulkReadStart += offset;
            emuDumpMainIndex += offset;
            
            // More data to get?
            if (bulkReadStart < bulkReadFinish)
            {
            
                // send request for next blocks
                OstrichBulkReadRequest(bulkReadStart, bulkReadBlocksPerPacket);
                return;
            }

            // Data transfer complete
            EmuDumpReady = true;
            string complete = "EMU Dump complete.\n";
            if (bulkReadWriteToFile) 
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\EMU_DUMP_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".bin";
                writeByteArrayToFile(emuDump, path);
                complete += "Dump has been saved as :\n" + path;
            }
            MessageBox.Show(complete);
        }
        public byte[] OstrichGetCompletedEmuDump()
        {
            return emuDump;
        }
        
        // Write array to file
        public void writeByteArrayToFile(byte[] input, string path)
            => File.WriteAllBytes(path, input);
        
        // Fast bulk read. reads 256kb blocks, 16 block stable max
        private void OstrichBulkReadRequest(int startAddress, int blocksPerPacket)
        {
            // Reset Checksum and buffer index
            emuDumpCHK = 0;
            emuDumpBufferIndex = 0;

            // Read request 
            byte MSB = (byte) (startAddress >> 8);
            byte MMSB = (byte) (startAddress & 0xFF);

            byte[] readRequest = new byte[6] { (byte) 'Z', (byte) 'R', (byte) blocksPerPacket, MMSB, MSB , 0x00};

            // Calculate checksum
            int checksum = 0;
            foreach (byte element in readRequest)
            {
                checksum += element;
            }
            checksum &= 0xff;
            
            // Add checksum to packet
            readRequest[5] = (byte)(checksum);

            // Send request
            Ostrich.Write(readRequest, 0, readRequest.Length);
            
            //Status = waiting for emu dump
            OstrichStatus = 3;
            
        }

        //Convert list of int to a space seperated string
        private string OstrichRXDataFormat(List<int> dataInput)
        {
            string strOut = "";

            foreach (int element in dataInput)
            {
                strOut += element.ToString("X2") + " ";
            }
            return strOut;
        }

        //Reset ostrich vendor ID
        public void OstrichResetVID(byte vendorID)
        {
            byte[] OstrichUpdateSerialAndVidCommand = new byte[11];
            OstrichUpdateSerialAndVidCommand[0] = 78;//4e
            OstrichUpdateSerialAndVidCommand[1] = vendorID;
            OstrichUpdateSerialAndVidCommand[10] = 0;
            Array.Copy(OstrichSerialNumber, 1, OstrichUpdateSerialAndVidCommand, 2, OstrichSerialNumber.Length - 1);
            int Checksum = 0;
            foreach (byte element in OstrichUpdateSerialAndVidCommand)
            {
                Checksum = Checksum + element;
            }
            byte[] ChecksumByte = BitConverter.GetBytes(Checksum);
            OstrichUpdateSerialAndVidCommand[10] = ChecksumByte[0];
            if (Ostrich.IsOpen)
            {
                Ostrich.Write(OstrichUpdateSerialAndVidCommand, 0, OstrichUpdateSerialAndVidCommand.Length);
                try
                {
                    Ostrich.ReadTimeout = 3000;
                    int OstrichResponse = Ostrich.ReadByte();
                    if (OstrichResponse == 79)
                    {
                        MessageBox.Show("Ostrich Vendor ID reset to " + vendorID);
                        byte[] OstrichRequestSerial = new byte[3] { 78, 83, 161 };
                        Ostrich.Write(OstrichRequestSerial, 0, 3);
                        OstrichStatus = 1;
                    }
                    else
                    {
                        MessageBox.Show("Bad response from Ostrich after write request");
                        OstrichError = true;
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message + "\r\n" + "If this continues to happen, you may need to unplug and replug the Ostrich and re-open the port");
                    OstrichError = true;
                }
            }
        }
    }


}
