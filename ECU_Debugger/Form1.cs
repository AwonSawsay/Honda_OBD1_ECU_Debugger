using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;



namespace ECU_Debugger
{
    public partial class Form1 : Form
    {
        //VCAL Re-Routing
        bool usingVCALreRoute = true;
        bool whiteListChangedFromOriginal = false;
        int VCALNumber = 0;
        int realVCALAddress = 0;
        int VCALVectorTableLoc = 0;
        int romOffsetWhiteList = 0;
        int VCALcallCount = 0;
        int romOffsetRealVcalSub = 0;
        const int BEGIN_VCAL_VECTOR_TABLE = 0x28;
        const byte VCAL_0_OPCODE = 0x10;
        List<int> VCALWhiteList = new List<int>();
        int[] VCALvectorTable = new int[8];



        //Strings for trace log
        string strTrace_R0 = "";
        string strTrace_R1 = "";
        string strTrace_R2 = "";
        string strTrace_R3 = "";
        string strTrace_R4 = "";
        string strTrace_R5 = "";
        string strTrace_R6 = "";
        string strTrace_R7 = "";
        string strTrace_DP = "";
        string strTrace_X1 = "";
        string strTrace_X2 = "";
        string strTrace_ACC = "";
        string strTrace_LRB = "";
        string strTrace_PSWL4 = "";
        string strTrace_PSWL5 = "";
        string strTrace_BCB = "";
        string strTrace_MIE = "";
        string strTrace_CF = "";
        string strTrace_ZF = "";
        string strTrace_HC = "";
        string strTrace_SF = "";
        string strTrace_DD = "";
        string strTrace_PSW = "";
        string strTrace_BP = "";
        string strTrace_SCB = "";
        string strTrace_StackPointer = "";
        string strTrace_Stack1 = "";
        string strTrace_Stack2 = "";
        string strTrace_Stack3 = "";
        string strTrace_Stack4 = "";
        string strTrace_Stack5 = "";
        string strTrace_DPconts = "";
        string strTrace_Xram1Addy = "";
        string strTrace_Xram2Addy = "";
        string strTrace_Xram1Conts = "";
        string strTrace_Xram2Conts = "";
        string strtrace_RamWatch = "";
        string strRamWatchAddress = "";

        //Trace Log
        StreamWriter objStreamWriter;
        int ContentsAddress = 0;
        int intDPConts_RAM = 0;
        bool boolOffsetPointer = false;
        string PointerContentsHexValue = "";
        string InstructionLine = "";
        string strTraceLog;
        string pathFile;
        string strPlain_R0 = "";
        string strPlain_R1 = "";
        string strPlain_R2 = "";
        string strPlain_R3 = "";
        string strPlain_R4 = "";
        string strPlain_R5 = "";
        string strPlain_R6 = "";
        string strPlain_R7 = "";
        string strPlain_DP = "";
        string strPlain_X1 = "";
        string strPlain_X2 = "";
        string strPlain_ACC = "";
        string strPlain_LRB = "";
        string strPlain_PSWL4 = "";
        string strPlain_PSWL5 = "";
        string strPlain_BCB = "";
        string strPlain_MIE = "";
        string strPlain_CF = "";
        string strPlain_ZF = "";
        string strPlain_HC = "";
        string strPlain_SF = "";
        string strPlain_DD = "";
        string strPlain_PSWH = "";
        string strPlain_PSWL = "";
        string strPlain_PSW = "";
        string strPlain_BP = "";
        string strPlain_SCB = "";
        string strPlain_StackPointer = "";
        string strPlain_Stack1 = "";
        string strPlain_Stack2 = "";
        string strPlain_Stack3 = "";
        string strPlain_Stack4 = "";
        string strPlain_Stack5 = "";
        string strPlain_DPconts = "";

        //Dasm ASM related

        OpenFileDialog OPENAsmDialog = new OpenFileDialog();
        string[] ASMFileArray;
        string[] RenamedASMFileArray;
        string[] ASMSourceCodeArray;
        string xmlRenamingFilePath = "";
        string strIgnoreDasmArgs = "";
        string strForceDasmArgs = "";
        string Dasm662Path;
        string DasmDefaultArea = " 5000 7fff";
        string DasmIndirectJumps = "";
        string CurrentASMPath;
        string ASMtoCompilePath;
        string compiledBINPath;
        bool ASMOpened = false;
        bool RenameXMLReady = false;
        bool readyToRename = false;
        bool showRenamed = false;
        int CurrentASMIndex;


        //Debugger ROM Code
        bool LRBConflict = false;
        bool SCBConflict = false;
        bool codeFileHasBeenOpened = false;
        int LRBDebuggerValue = 0;
        int PSWDebuggerValue = 0;
        int BaudRateAddress1;
        int BaudRateAddress2;
        int BaudRatePatchValue = 0;
        int ROMOffsetWhichLoop;
        int DebuggerLRBOffsetFromRom;
        int DebuggerPSWOffsetFromRom;
        int DebuggerIEOffsetFromRom;
        int ROMWhichLoopAddress;
        int intPSWValue = 0;

        //Bin and Ostrich
        OpenFileDialog OPENBINDialog = new OpenFileDialog();
        bool BINOpened = false;
        bool OstrichSentError = false;
        bool quietBulkWrite = false;
        bool usingASMFile = false;
        List<int> OstrichRXBuffer = new List<int>();
        List<int> chunkAddresses = new List<int>();
        List<int> addressLength = new List<int>();
        Byte[] currentBINArray;
        Byte[] newBinArray;
        Byte[] BinWithPatchesAndDebuggerCode;
        Byte[] OstrichBinArrayOut;
        byte[] OstrichSerialNumber = new byte[9];
        byte[] byteCodeStubCAll = new byte[] { 50, 0, 117, 3 };
        byte[] byteAltCodeStubCall = new byte[] { 4, 50, 0, 117, 3, 50, 0, 117, 255 };
        byte[] ROMSettingsAndDebuggerCodeArray;
        string[] strAryNewASM;
        byte MMSB = (byte)(00);
        byte MSB = (byte)(112);
        int intDebuggerCodeStubSourceOffset = 0;
        int intCodeStubAddress = 0;
        int intPatchBytesSourceOffset = 0;
        int intExtraRam1Offset = 0;
        int intExtraRam2Offset = 0;
        int intExtraRam3Offset = 0;
        int intExtraRam4Offset = 0;
        int intExtraRam5Offset = 0;
        int intExtraRam6Offset = 0;
        int intDPAddress = 0;
        int OstrichStatus;
        int InitialOstrichBP = 0;
        int StartChunkAddress = 0;
        int BulkChunkNumber = 0;
        int TimerTickCount;
        int LowerBoundary = 0;
        int UpperBoundary = 32768;
        int intPrevLowerBoundary = 0;
        int intPrevUpperBoundary = 0;
        int intPrevChunkSize = 0;
        int intChunkSize = 0;
        string ROMSettingsAndDebuggerCodePath;
        string strBINPath = "";



        //Serial
        List<int> WorkingBuffer = new List<int>();
        List<int> BufferBeingProcessed = new List<int>();
        List<int> dataBuffer = new List<int>();
        List<int> dataBuffer1 = new List<int>();
        int ReportedByteCount = 0;
        int intBufferLength;
        int intPacketsRecd = 0;
        int PreviousPacketNumber = 0;
        bool UseFirstBuffer = true;
        bool CheckSumPassed = false;
        bool InitialBPAlreadyRemoved;
        bool DDFlag = false;
        bool DDFlagCorrection = false;
        bool BinHasBeenUploadedSinceCreation = false;
        bool StopWriteDueToError = false;
        bool ROMWhichLoopEquals1 = false;

        //Debugger handling
        System.Collections.BitArray PSWBitArray;
        int intNextInstructionAddress;
        int intBranchAddress;
        int intCallAddress;
        int intStack1Value = 0;
        int intStack4Value = 0;
        int intCurrent_BP;
        int intPrevNextInstruction = 0;
        int intPrevBranchAddress = 0;
        int intPrevCallAddress = 0;
        string strOpCode = "";
        bool boolReDasm = false;
        bool boolNewBreakpoint = false;

        //Look ahead ram
        int intLA1XRAMAddress1 = 0;
        int intLA1XRAMAddress2 = 0;
        int intLA2XRAMAddress1 = 0;
        int intLA2XRAMAddress2 = 0;
        int intXramLA11Value = 0;
        int intXramLA12Value = 0;
        int intXramLA21Value = 0;
        int intXramLA22Value = 0;
        int intRAMWatchByte1 = 0;
        int intRAMWatchByte2 = 0;
        int intRamWatchAddress = 0;
        int intRAMWatchDisplayedAddy = 0;
        int intRamWatchValue = 0;
        int intIncomingRAMAddress1 = 0;
        int intIncomingRAMAddress2 = 0;
        int intIncomingRAMAddress3 = 0;
        int intIncomingRAMAddress4 = 0;
        int intModifiedX1 = 0;
        int intModifiedX2 = 0;
        bool boolLA1word = false;
        bool boolLA2word = false;
        bool boolIncomingWordLA1 = false;
        bool boolIncomingWordLA2 = false;
        bool dotFoundonNextInstruction = false;
        bool dotFoundonBranchInstruction = false;
        bool boolXRam1Found = false;
        bool boolXRam2Found = false;
        bool boolXRam3Found = false;
        bool boolXRam4Found = false;
        bool ExtraRam1incoming = false;
        bool ExtraRam2incoming = false;
        bool ExtraRam3incoming = false;
        bool ExtraRam4incoming = false;
        bool instructionBranched = false;
        bool useModifiedX1 = false;
        bool useModifiedX2 = false;

        string[] SFRLabels = {"NO_NAME" , "NO_NAME" , "NO_NAME" , "LRB"   , "LRBH"     , "NO_NAME" , "NO_NAME" , "NO_NAME" , "NO_NAME" , "NO_NAME",
                              "NO_NAME" , "NO_NAME" , "NO_NAME" , "NO_NAME", "NO_NAME" , "NO_NAME" , "SBYCON"  , "WDT"     , "PRPHF"   , "STPACP",
                              "NO_NAME" , "NO_NAME" , "NO_NAME" , "NO_NAME", "IRQ"     , "IRQH"    , "IE"      , "IEH"     , "EXION"   , "NO_NAME",
                              "NO_NAME" , "NO_NAME" , "P0"      , "P0IO"   , "P1"      , "P1IO"    , "P2"      , "P2IO"    , "P2SF"    , "NO_NAME",
                              "P3"      , "P3IO"    , "P3SF"    , "NO_NAME", "P4"      , "P4IO"    , "P4SF"    , "P5"      , "TM0"     , "TM0H",
                              "TMR0"    , "TMR0H"   , "TM1"     , "TM1H"   , "TMR1"    , "TMR1H"   , "TM2"     , "TM2H"    , "TMR2"    , "TMR2H",
                              "TM3"     , "TM3H"    , "TMR3"    , "TMR3H"  , "TCON0"   , "TCON1"   , "TCON2"   , "TCON3"   , "NO_NAME" , "NO_NAME",
                              "TRNSIT"  , "NO_NAME" , "STTM"    , "STTMR"  , "STTMC"   , "NO_NAME" , "SRTM"    , "SRTMR"   , "SRTMC"   , "NO_NAME",
                              "STCON"   , "STBUF"   , "NO_NAME" , "NO_NAME", "SRCON"   , "SRBUF"   , "SRSTAT"  , "NO_NAME" , "ADSCAN"  , "ADSEL",
                              "NO_NAME" , "NO_NAME" , "NO_NAME" , "NO_NAME", "NO_NAME" , "NO_NAME" , "ADCR0"   , "ADCR0H"  , "ADCR1"   , "ADCR1H",
                              "ADCR2"   , "ADCR2H"  , "ADCR3"   , "ADCR3H" , "ADCR4"   , "ADCR4H"  , "ADCR5"   , "ADCR5H"  , "ADCR6"   , "ADCR6H",
                              "ADCR7"   , "ADCR7H"  , "PWMC0"   , "PWMC0H" , "PWMR0"   , "PWMR0H"  , "PWMC1"   , "PWMC1H"  , "PWMR1"   , "PWMR1H",
                              "PWCON0"  , "NO_NAME" , "PWCON1"};


        public Form1()
        {
            InitializeComponent();
            setMiscToolTips();
        }

        //Set checkboxes and paths
        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort1.DtrEnable = false;
            serialPort1.RtsEnable = false;
            cboxAutoStep.Checked = false;
            cboxDebuggerIE.Checked = false;
            nudDebuggerIE.Enabled = false;
            cboxDebuggerLRB.Checked = false;
            nudDebuggerLRB.Enabled = false;
            cboxDebuggerPSW.Checked = false;
            nudDebuggerPSW.Enabled = false;
            cboxAutoUploadOnBINLoad.Checked = true;
            pathFile = Directory.GetCurrentDirectory();
            pathFile += @"\ECUTrace.txt";
            cboxSaveTrace.Checked = false;
            progressBar3.Visible = false;
            cboxQuietCompiler.Enabled = false;
            lblChangedBytes.Text = "";
            lblRamWatchBinary.Text = "";
            lblXram1Binary.Text = "";
            lblXram2Binary.Text = "";
            lblDPContsBinary.Text = "";
            lblSizeWithVCAL.Text = "";
            lblVCALbyteSize.Text = "";
            lblVCALnumber.Text = "";

        }

        //About - menu item
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This program will allow you to step through the instructions on " +
                "OBD 1 Honda ECUs while monitoring the main registers as they are changed " +
                "\r\n\r\n Created by Awon using code provided by Catur P.  " +
                "\r\n dasm662 by Andy Sloan and Doc is also used and distributed with ECU Debugger.",
                "About ECU Debugger", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Exit - menu item
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            groupBox12.Width = panel1.Width - 213;
            groupBox12.Height = panel1.Height - 40;

            tboxTraceWindow.Height = panel1.Height - 92;
        }

        //------------------ ASM Source Code handling -----------------------------

        private void btnLoadASMFile_Click(object sender, EventArgs e)
        {
            OPENAsmDialog.Filter = "ASM Files | *.asm";
            if (OPENAsmDialog.ShowDialog() == DialogResult.OK)
            {
                lblChangedBytes.Text = "";
                tboxOpenFileSimpleName.Text = "";
                ASMtoCompilePath = OPENAsmDialog.FileName;
                compiledBINPath = System.IO.Path.GetDirectoryName(ASMtoCompilePath) + System.IO.Path.GetFileNameWithoutExtension(ASMtoCompilePath) + "_DBG";
                readAllLinesInASM(ASMtoCompilePath, ref ASMSourceCodeArray);
                //Compile ASM
                if (compileASMtoBIN(ASMtoCompilePath, compiledBINPath))
                {
                    strBINPath = compiledBINPath + ".bin";
                    BINOpened = true;
                    usingASMFile = true;
                    CreateBINArraysDecompileBINandUpdateASMWindow();

                }
                //Decompile
                if (decompileBIN(strBINPath, compiledBINPath + ".asm", false, getDasmArgs()))
                {
                    CurrentASMPath = compiledBINPath + ".asm";
                    ASMOpened = true;
                    LoadASMFileIntoArraysRenameIfNeededAndUpdateASMWindow();
                    ASMOpened = true;
                    getVCALroutingInfoFromROM();
                    AddDebuggerCodeAndPatchesToRomAndUpload();
                }




            }
        }
        
        private bool decompileBIN(string binPath, string asmPath, bool quiet = false, string args = "")
        {
            try
            {
                bool noExitError = true;
                string strCompilerError = "";
                string strCompilerOutput = "";

                Dasm662Path = Directory.GetCurrentDirectory();
                Dasm662Path += @"\dasm662.exe";

                var proc = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = Dasm662Path,
                        Arguments = "\"" + binPath + "\"" + " " + "\"" + asmPath + "\"" + args,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                if (!quiet)
                {
                    //If there are any errors reported by dasm662, gather and send to messagebox
                    strCompilerError = "";
                    while (!proc.StandardError.EndOfStream)
                    {
                        strCompilerError += proc.StandardError.ReadLine() + "\r\n";
                    }
                    if (strCompilerError != "") MessageBox.Show(strCompilerError, "DASM662 Error");


                    //Gather dasm662 output then send to messagebox
                    strCompilerOutput = "";
                    while (!proc.StandardOutput.EndOfStream)
                    {
                        strCompilerOutput += proc.StandardOutput.ReadLine() + "\r\n";
                    }
                    if (strCompilerOutput != "") MessageBox.Show(strCompilerOutput, "DASM662 Output");

                    //Exit error
                    if (proc.ExitCode != 0)
                    {
                        MessageBox.Show("Error, Exit Code not equal to 0", "DASM662 Error");
                        noExitError = false;
                    }
                }
                proc.WaitForExit();
                return noExitError;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message + "\r\n\r\n" + "Is dasm662.exe located int the same directory as ECU Debugger? \r\n " +
                    "Also make sure the bin path does not contain illegal characters. \r\n Try moving the bin the root directory and using a simple name.");
                return false;
            }
        }

        //Read all the lines in an ASM file to a string []
        //Needed to do it this way to work around file access issues while another process is using the file
        private void readAllLinesInASM(string path, ref string[] destArray)
        {
            int ioRetries = 3;
            int retryDelay = 200;
            for (int i = 0; i < ioRetries; i++)
            {
                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string streamString = reader.ReadToEnd();
                            destArray = streamString.Split('\n');
                            for (int idx = 0; idx < destArray.Length; idx++)
                            {
                                destArray[idx] = destArray[idx].Replace("\r", "");
                            }

                        }
                    }
                }
                catch (IOException) when (i < ioRetries)
                {
                    System.Threading.Thread.Sleep(retryDelay);
                }
            }
        }
        
        //Compile ASM into BIN return true if program exits clean
        private bool compileASMtoBIN(string inputPath, string outputPath, bool quiet = false, string args = "")
        {

            bool booldeleteFile = false;
            string strCompilerError = "";
            string strCompilerOutput = "";
            string strCommand = args + "\"" + inputPath + "\" \"" + outputPath + "\"";
            bool noExitError = true;
            try
            {
                //extract resource asm662.exe
               // booldeleteFile = extractASM662();

                var proc = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "asm662.exe",
                        Arguments = strCommand,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                if (!quiet)
                {
                    //If there are any errors reported by asm662, gather and send to messagebox
                    strCompilerError = "";
                    while (!proc.StandardError.EndOfStream)
                    {
                        strCompilerError += proc.StandardError.ReadLine() + "\r\n";
                    }
                    if (strCompilerError != "") MessageBox.Show(strCompilerError, "ASM662 Error");



                    strCompilerOutput = "";
                    //Gather asm662 output then send to messagebox 

                    while (!proc.StandardOutput.EndOfStream)
                    {
                        strCompilerOutput += proc.StandardOutput.ReadLine() + "\r\n";
                    }
                    if (strCompilerOutput != "") MessageBox.Show(strCompilerOutput, "ASM662 Output");
                }
                proc.WaitForExit();

                //Exit error
                if (proc.ExitCode != 0)
                {
                    MessageBox.Show("Error, Exit Code not equal to 0", "ASM662 Error");
                    noExitError = false;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            if (booldeleteFile)
            {
                //System.IO.File.Delete(System.IO.Directory.GetCurrentDirectory() + @"\asm662.exe");
            }
            return noExitError;
        }

        //Read the asm files into working arrays, rename the asm if option is selected and then update the ASM window
        private void LoadASMFileIntoArraysRenameIfNeededAndUpdateASMWindow()
        {
            try
            {
                ASMFileArray = System.IO.File.ReadAllLines(CurrentASMPath);
                RenamedASMFileArray = System.IO.File.ReadAllLines(CurrentASMPath);
                rtboxASMFile.SelectionFont = new Font("DejaVu Sans Mono", 8, FontStyle.Bold);
                if (cboxRenameOnChange.Checked && readyToRename)
                {
                    renameASMArray();
                    rtboxASMFile.Lines = RenamedASMFileArray;
                    return;
                }

                rtboxASMFile.Lines = ASMFileArray;
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //Finds the index in ASM array for the given address. If address is invalid, it is reduced until it's valid
        private void getIndex_ValidateAddress(ref int index, ref int Address)
        {
            if (ASMOpened)
            {
                try
                {
                    index = IndexOfInstructionAddressInArray(ASMFileArray, Address);
                    int AddressCheck = 0;
                    //Address not found, reduce address until one is found
                    while (index == -1 && AddressCheck < 300)
                    {
                        Address--;
                        index = IndexOfInstructionAddressInArray(ASMFileArray, Address);
                        AddressCheck++;
                    }

                }
                catch
                {
                    MessageBox.Show("Error locating address in ASM");
                }
            }
        }

        private void HighlightLineInASMTextBox(int Index)
        {
            if (Index > 0 && ASMOpened)
            {
                rtboxASMFile.Focus();
                SelectLineInRichTextBox(rtboxASMFile, Index);
            }
        }
        //Search Array for the last instance of an address preceeded by "; "
        //Returns -1 if not found
        private int IndexOfInstructionAddressInArray(string[] myarray, int address)
        {
            int Index = 0;
            string strSearchString = "; " + address.ToString("X4");
            try
            {
                Index = Array.FindLastIndex(myarray, s => s.Contains(strSearchString));
            }
            catch(Exception err) 
            {
                MessageBox.Show(err.Message + "\r\n While parsing BP address from ASM file.");
            }
            return Index;
        }
        private void SelectLineInRichTextBox(RichTextBox myRichTextBox, Int32 lineIndex)
        {
            Int32 j = 0;
            lineIndex++;
            String text = myRichTextBox.Text;
            if (lineIndex > myRichTextBox.Lines.Length)
                lineIndex = myRichTextBox.Lines.Length;
            //Add the index of all \n characters until we reach the index of the address. This will give us the starting character to highlight
            for (Int32 i = 1; i < lineIndex; i++)
            {
               j = text.IndexOf('\n', j + 1);
              if (j == -1) break;
            }
            
            if (lineIndex > 1)
            {
                myRichTextBox.Select(j + 1, myRichTextBox.Lines[lineIndex - 1].Length);
            }
            else
            {
                myRichTextBox.Select(j, 60);
            }

        }

        //Get the instruction address that follows the one entered and it's index in the ASM array
        private int DetermineNextInstructionAddress(int intCurrentHexAddress, int index)
        {
            //int CurrentAddressIndex;
            string NextLineAddress;
            try
            {
                //CurrentAddressIndex = IndexOfInstructionAddressInArray(ASMFileArray, intCurrentHexAddress);
                NextLineAddress = ASMFileArray[index + 1].Substring(ASMFileArray[index + 1].IndexOf("; ") + 2, 4);
                if (NextLineAddress == "warn")
                {
                    NextLineAddress = ASMFileArray[index + 2].Substring(ASMFileArray[index + 2].IndexOf("; ") + 2, 4);
                }
            }
            catch
            {
                MessageBox.Show("Be sure that you are using a clean asm file produced by asm662.", "Can not parse next address from ASM");
                NextLineAddress = "0000";
            }
            return Convert.ToInt32(NextLineAddress, 16);
            
        }
        
        private bool ParseInstructionLineAndOpCodes(int intAddress, int index, ref int nextAddress, ref int branchAddress, ref int callAddress, ref string ROMPointerContents, ref string opCode )
        {
            
            int intNextInstAddy = 0;
            int intBranchAddy = 0;
            int intCallAddy = 0;
            bool DDCorrect = false;
            string pointerContents = "";
            string thisOpcode = "";
            string thisInstructionLine = "";
            string firstOperand = "";
            string secondOperand = "";
            boolReDasm = false;

            //Next address set to next sequential address unless the OP codes change it
            intNextInstAddy = DetermineNextInstructionAddress(intAddress, index);

            //Parse the opcode
            thisInstructionLine = ASMFileArray[index];
            //thisOpcode = ASMFileArray[index].Substring(16, 5).Trim();
            thisOpcode = thisInstructionLine.Substring(16, 5).Trim();
            firstOperand = thisInstructionLine.Substring(24, thisInstructionLine.IndexOf(";") - 24).Trim();
            if(thisInstructionLine.Contains(","))
            {
                firstOperand = thisInstructionLine.Substring(24).Split(',', ';')[0];
                secondOperand = thisInstructionLine.Split(',', ';')[1].Trim();
            }




            try
            {
                switch (thisOpcode)
                {
                    case "J":
                        //Regular Jump - Reverse endian of the last two opcode bytes, then convert to int
                        //string strIndirectJumpAddy = ASMFileArray[index].Substring(ASMFileArray[index].Length - 2, 2) + ASMFileArray[index].Substring(ASMFileArray[index].Length - 4, 2);
                        string strIndirectJumpAddy =thisInstructionLine.Substring(thisInstructionLine.Length - 2, 2) + thisInstructionLine.Substring(thisInstructionLine.Length - 4, 2);
                        intNextInstAddy = Convert.ToInt32(strIndirectJumpAddy, 16);

                        
                        if (thisInstructionLine.Contains("["))
                        {
                            int intPossiblePointer = GetPointerFromReg(ASMFileArray[index].Split('[', ']')[1]);
                            if(intPossiblePointer != 0)
                            {
                                boolReDasm = true;
                                intNextInstAddy = intPossiblePointer;
                            }


                        }

                        break;
                    case "SJ":
                        intNextInstAddy = determineOffsetJumpAddress(intAddress, index, 2);
                        break;
                    case "JEQ":
                    case "JNE":
                    case "JLT":
                    case "JLE":
                    case "JGT":
                    case "JGE":
                    case "JRNZ":
                        intBranchAddy = determineOffsetJumpAddress(intAddress, index, 2);
                        break;
                    case "JBS":
                    case "JBR":
                        intBranchAddy = determineOffsetJumpAddress(intAddress, index, 3);
                        break;
                    case "CAL":
                        //Reverse endian of the last to opcode bytes, then convert to int
                        string callAddy = ASMFileArray[index].Substring(ASMFileArray[index].Length - 2, 2) + ASMFileArray[index].Substring(ASMFileArray[index].Length - 4, 2);
                        intCallAddy = Convert.ToInt32(callAddy, 16);
                        break;
                    case "SCAL":
                        intCallAddy = determineOffsetJumpAddress(intAddress, index, 2);
                        break;

                    case "VCAL":
                        int intVCALNum = Convert.ToInt32(ASMFileArray[index].Substring(24, 1));
                        string CallAddress = String.Concat(ASMFileArray[21 + intVCALNum].Substring(ASMFileArray[21 + intVCALNum].LastIndexOf(";") + 9, 2), ASMFileArray[21 + intVCALNum].Substring(ASMFileArray[21 + intVCALNum].LastIndexOf(";") + 7, 2));
                        intCallAddy = Convert.ToInt32(CallAddress, 16);
                        //If using vcal reroute and the vcal being called is the one we are re-reouting- change the breakpoint to the actual vcal sub, not where the vector table says
                        if (usingVCALreRoute && intVCALNum == VCALNumber) intCallAddy = realVCALAddress;
                        break;
                    case "RT":
                        intNextInstAddy = intStack1Value;
                        break;
                    case "RTI":
                        intNextInstAddy = intStack4Value;
                        break;
                    case "CMPB":
                        //The dasm doesnt always reflect this opcode ocrrectly, this is a work around so the debugger doesn't try placing a breakpoint
                        //on the line following this code, when its actually the other byte from a word
                        if (DDFlag && opCode != "LB")
                        {
                            string CMPMCheck = ASMFileArray[index].Substring(64, 2);
                            if (CMPMCheck == "C6" || CMPMCheck == "48" || CMPMCheck == "49" || CMPMCheck == "4A" || CMPMCheck == "4B"
                             || CMPMCheck == "92" || CMPMCheck == "90" || CMPMCheck == "91" || CMPMCheck == "A1" || CMPMCheck == "A0"
                             || CMPMCheck == "A4" || CMPMCheck == "C7" || CMPMCheck == "B5" || CMPMCheck == "B2" || CMPMCheck == "B3"
                             || CMPMCheck == "B0" || CMPMCheck == "B1")
                            {
                                DDCorrect = true;
                            }
                        }
                        break;
                    case "LCB":
                    case "CMPCB":
                        pointerContents = GetHexValueFromPointerLocationInInstructionLine(ASMFileArray, index, false);
                        break;
                    case "LC":
                    case "CMPC":
                        pointerContents = GetHexValueFromPointerLocationInInstructionLine(ASMFileArray, index, true);
                        break;
                    //Used only for lookahead ram pointers
                    case "MOV":
                        if(firstOperand == "X1" && secondOperand.Contains("#") || firstOperand == "X1" && secondOperand.Contains("tbl_"))
                        {
                            string strNewX1 = thisInstructionLine.Substring(thisInstructionLine.Length - 2, 2) + thisInstructionLine.Substring(thisInstructionLine.Length - 4, 2);
                            intModifiedX1 = Convert.ToInt32(strNewX1, 16);
                            useModifiedX1 = true;
                        }
                        if(firstOperand == "X2" && secondOperand.Contains("#") || firstOperand == "X2" && secondOperand.Contains("tbl_"))
                        {
                            string strNewX2 = thisInstructionLine.Substring(thisInstructionLine.Length - 2, 2) + thisInstructionLine.Substring(thisInstructionLine.Length - 4, 2);
                            intModifiedX2 = Convert.ToInt32(strNewX2, 16);
                            useModifiedX2 = true;
                        }
                        if (firstOperand == "X1" && secondOperand == "A")
                        {

                            intModifiedX1 = GetPointerFromReg("ACC");
                            useModifiedX1 = true;
                        }
                        if (firstOperand == "X2" && secondOperand == "A")
                        {

                            intModifiedX1 = GetPointerFromReg("ACC");
                            useModifiedX2 = true;
                        }
                        break;
                    case "CLR":
                        if (firstOperand == "X1")
                        {
                            intModifiedX1 = 0;
                            useModifiedX1 = true;
                        }
                        if (firstOperand == "X2")
                        {
                            intModifiedX2 = 0;
                            useModifiedX2 = true;
                        }
                        break;
                    case "INC":
                        if (firstOperand == "X1")
                        {
                            intModifiedX1 = Convert.ToInt32(strPlain_X1, 16) + 1;
                            useModifiedX1 = true;
                        }
                        if (firstOperand == "X2")
                        {
                            intModifiedX2 = Convert.ToInt32(strPlain_X2, 16) + 1;
                            useModifiedX2 = true;
                        }
                        break;
                    case "DEC":
                        if (firstOperand == "X1")
                        {
                            intModifiedX1 = Convert.ToInt32(strPlain_X1, 16) - 1;
                            useModifiedX1 = true;
                        }
                        if (firstOperand == "X2")
                        {
                            intModifiedX2 = Convert.ToInt32(strPlain_X2, 16) - 1;
                            useModifiedX2 = true;
                        }
                        break;

                    default:
                        //No branching found on this instruction line, clear branch addresses and set next instruction to the next sequential instruction
                        intCallAddy = 0;
                        intBranchAddy = 0;
                        break;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message + "\r\n\r\n Couldn't parse the next possible address from the dasm.  This can be caused by " +
                    "a bad dasm file, or indirect jumps being used.");
            }
            nextAddress = intNextInstAddy;
            branchAddress = intBranchAddy;
            callAddress = intCallAddy;
            ROMPointerContents = pointerContents;
            opCode = thisOpcode;
            return DDCorrect;   

        }
        
        //Use the last byte in the opcode to determine the Jump offset and address
        private int determineOffsetJumpAddress(int instructionAddress, int ASMIndex, int OpcodeBytelength)
        {
            int offset = int.Parse(ASMFileArray[ASMIndex].Substring(ASMFileArray[ASMIndex].Length - 2, 2), System.Globalization.NumberStyles.HexNumber);
            int jumpAddress = instructionAddress + offset + OpcodeBytelength;
            if (offset > 128)
            {
                jumpAddress = jumpAddress - 256;
            }
            return jumpAddress;
        }
        
        // handles the following cases, [Pointer] , 0xxxxh[Pointer] , tbl_XXXX[Pointer] , 0xxxxh , tbl_XXXX
        // This will check for "h", "tbl_", and "[]" and get the int value of each and then add them to get the address.
        // Then the contents of the adress is obtained and returned as a hex string. 
        private string GetHexValueFromPointerLocationInInstructionLine(string[] ASMArray, int ArrayIndex, bool ReturnWord = false)
        {

            int hAddress = 0;
            int PointerRegisterAddress = 0;
            int TableAddress = 0;
            ContentsAddress = 0;
            int ContentsByte1 = 0;
            int ContentsByte2 = 0;
            string ContentsHexString = "";
            bool boolNegativeOffset = false;
            string SecondInstructionOperand = ASMFileArray[ArrayIndex].Split(',', ';')[1].Trim();

            
            if (SecondInstructionOperand.Contains("h"))
            {
                hAddress = Convert.ToInt32(SecondInstructionOperand.Substring(
                           SecondInstructionOperand.IndexOf("h") - 4, 4), 16);
                if (hAddress > 32768)
                {
                    hAddress = 65536 - hAddress;
                    boolNegativeOffset = true;

                }

            }
            boolOffsetPointer = false;
            if (SecondInstructionOperand.Contains("["))
            {

                boolOffsetPointer = true;
                string InstructionLinePointerRegister = SecondInstructionOperand.Split('[', ']')[1];
                PointerRegisterAddress = 0;
                PointerRegisterAddress = GetPointerFromReg(InstructionLinePointerRegister);
            }

            if (SecondInstructionOperand.Contains("tbl_"))
            {
                string strTableAddress = SecondInstructionOperand.Substring(SecondInstructionOperand.IndexOf("tbl_") + 4, 4);
                TableAddress = Convert.ToInt32(strTableAddress, 16);
            }
            if (boolNegativeOffset)
            {
                ContentsAddress = TableAddress + PointerRegisterAddress - hAddress;
            }
            else
            {
                ContentsAddress = hAddress + TableAddress + PointerRegisterAddress;
            }

                ContentsByte1 = BinWithPatchesAndDebuggerCode[ContentsAddress];
                ContentsByte2 = BinWithPatchesAndDebuggerCode[ContentsAddress + 1];
                if (ReturnWord)
                {
                    int WordValue = (ContentsByte2 << 8) + ContentsByte1;
                    ContentsHexString = WordValue.ToString("X4");
                }
                else
                {
                    ContentsHexString = ContentsByte1.ToString("X2");
                }
            return ContentsHexString;
        }

        private void btnClearText_Click(object sender, EventArgs e)
        {
            if (tboxTraceWindow.Text != "")
            {
                tboxTraceWindow.Text = "";
            }
        }
        private void rtboxASMFile_MouseDown(object sender, MouseEventArgs e)
        {
            if (tboxECUReturnAddress.Text != "")
            {
                HighlightLineInASMTextBox(CurrentASMIndex);
            }

        }

        private void rtboxASMFile_Click(object sender, EventArgs e)
        {
            highlightAfterValidating(0);
        }

        //---------------- ASM Renaming -------------------------------------------

        DataSet dataset1 = new DataSet();
        OpenFileDialog OPENAsmRenamingFile = new OpenFileDialog();
        
        private void btnOpenXMLFile_Click(object sender, EventArgs e)
        {
            OpenXMLFile();
            readyToRename = true; ;
        }

        private void OpenXMLFile()
        {
            OPENAsmRenamingFile.Filter = "XML Files | *.xml";
            if (OPENAsmRenamingFile.ShowDialog() == DialogResult.OK)
            {
                xmlRenamingFilePath = OPENAsmRenamingFile.FileName;
                ReadXMLFileIntoDataset(xmlRenamingFilePath);
            }
        }

        private void ReadXMLFileIntoDataset(string xmlPath)
        {
            try
            {
                dataset1.Clear();
                dataset1.ReadXml(xmlPath);
                RenameXMLReady = true;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void RenameUsingValuesFromXML(string[] OrigArray, string[] NewArray, bool RestoreToOriginalValue = false)
        {
            progressBar3.Visible = true;
            progressBar3.Minimum = 1;
            progressBar3.Value = 1;
            progressBar3.Step = 1;
            progressBar3.Maximum = OrigArray.Length;

            for (int index = 0; index < OrigArray.Length; index++)
            {
                foreach (DataTable table in dataset1.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        string searchString = Convert.ToString(dr[1]);
                        if (Convert.ToString(dr[0]) == "True" && searchString != "" && OrigArray[index].Contains(searchString))
                        {
                            //Use an occurance compare to see if we've already made a replacement that would cause a false match
                            if (System.Text.RegularExpressions.Regex.Matches(OrigArray[index], System.Text.RegularExpressions.Regex.Escape(searchString)).Count == System.Text.RegularExpressions.Regex.Matches(NewArray[index], System.Text.RegularExpressions.Regex.Escape(searchString)).Count)
                            {
                                NewArray[index] = CaseSenstiveReplace(NewArray[index], searchString, Convert.ToString(dr[2]));
                            }
                            else
                            {
                                //Split the original string and the replacement string then only replace an instance that occurs in the same location in both 
                                string[] origWords = OrigArray[index].Split(' ');
                                string[] newWords = NewArray[index].Split(' ');
                                int wordIndex = 0;
                                foreach (string word in origWords)
                                {
                                    if (word.Contains(searchString) && newWords[wordIndex].Contains(searchString))
                                    {
                                        newWords[wordIndex] = CaseSenstiveReplace(newWords[wordIndex], searchString, Convert.ToString(dr[2]));
                                    }
                                    wordIndex++;
                                }
                                string replacementString = "";
                                for (int i = 0; i < newWords.Count(); i++)
                                {
                                    replacementString += newWords[i];
                                    replacementString += " ";
                                }
                                NewArray[index] = replacementString.Remove(replacementString.Length - 1, 1);
                            }
                        }
                    }
                }
                progressBar3.PerformStep();
            }
            if(!cboxRenameOnChange.Checked)
            {
                MessageBox.Show("Rename Complete");
            }
            progressBar3.Visible = false;

        }

        public string CaseSenstiveReplace(string originalString, string oldValue, string newValue)
        {

            //Ignore search match if it is located after " DB "
            //if (cBoxIgnoreDB.Checked && originalString.Contains(" DB ") && originalString.IndexOf(" DB ") < originalString.IndexOf(oldValue))
            if (originalString.Contains(" DB ") && originalString.IndexOf(" DB ") < originalString.IndexOf(oldValue))
            {
                return originalString;
            }

            //Ignore search match if it is located after " DW "
            // if (cBoxIgnoreDW.Checked && originalString.Contains(" DW ") && originalString.IndexOf(" DW ") < originalString.IndexOf(oldValue))
            //{
            //   return originalString;
            //}

            //Ignore text located after finding a # unless for a pointer register or label_ or tbl_
            //if (cboxIgnorePound.Checked && originalString.Contains("#"))
            if (originalString.Contains("#"))
            {
                if (originalString.IndexOf(oldValue) < originalString.IndexOf("#") || newValue.Contains("label_") || newValue.Contains("tbl_"))
                {
                    return originalString.Replace(oldValue, newValue);
                }

                if (!originalString.Contains("X1,") && !originalString.Contains("X2,") && !originalString.Contains("DP,"))
                {
                    return originalString;
                }
            }

            return originalString.Replace(oldValue, newValue);
        }

        private void btnApplyRenamingMask_Click(object sender, EventArgs e)
        {
            renameASMArray();
        }

        private void renameASMArray()
        {
            if (!readyToRename)
            {
                return;
            }
            try
            {
                ReadXMLFileIntoDataset(xmlRenamingFilePath);
                RenameUsingValuesFromXML(ASMFileArray, RenamedASMFileArray);
                rtboxASMFile.Lines = RenamedASMFileArray;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (showRenamed)
            {
                rtboxASMFile.Lines = RenamedASMFileArray;
            }
            else
            {
                rtboxASMFile.Lines = ASMFileArray;
            }
            showRenamed = !showRenamed;
            highlightAfterValidating(0);
            
        }
        private void btnSaveRenamedASM_Click(object sender, EventArgs e)
        {
            string RenamedAsmName = System.IO.Path.GetDirectoryName(CurrentASMPath) + "RENAMED_" + System.IO.Path.GetFileName(CurrentASMPath);
            using (StreamWriter outputfile = new StreamWriter(RenamedAsmName))
            {
                foreach (string line in RenamedASMFileArray)
                    outputfile.WriteLine(line);
            }
            MessageBox.Show("File saved as " + System.IO.Path.GetFileName(RenamedAsmName));
        }

        //-------------------------------------------------------------------------


        //Used to convert hex values written in serial send box to their byte values
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private string GetPointerValuesFromPointerTextBox(TextBox Textbox, string PointerName)
        {
            string Output = "";
            if (Textbox.Text != "" && Convert.ToInt32(Textbox.Text, 16) > 0x47e && Convert.ToInt32(Textbox.Text, 16) < 0x8000 && BINOpened)
            {
                try
                {
                    int Pointer = Convert.ToInt32(Textbox.Text, 16);
                    string DP = BinWithPatchesAndDebuggerCode[Pointer].ToString("X2");
                    string DP1 = BinWithPatchesAndDebuggerCode[Math.Min(Pointer + 1, 32767)].ToString("X2");
                    string DP2 = BinWithPatchesAndDebuggerCode[Math.Min(Pointer + 2, 32767)].ToString("X2");
                    string DP3 = BinWithPatchesAndDebuggerCode[Math.Min(Pointer + 3, 32767)].ToString("X2");
                    Output = PointerName + " Contents:" +
                          "\r\n [" + PointerName + " + 0]: " + DP
                        + "\r\n [" + PointerName + " + 1]: " + DP1
                        + "\r\n [" + PointerName + " + 2]: " + DP2
                        + "\r\n [" + PointerName + " + 3]: " + DP3;
                }
                catch
                {

                }
            }
            else
            {
                Output = "Contents Unavailable";
            }
            return Output;
        }

        //------------------- Serial and ECU Datastream --------------------------
        
        //Serial handler for data received from the ECU - grab a packet, switch buffers and send it off for processing
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if(serialPort1.BytesToRead == 0) { return; }

            //Look for packet footer, then the 2 following bytes, switch buffers and call RXPacketProcessing
            int CurrentByte;
            int PacketStatus = 1;
            int BytesRead = 0;
            int ErrorCount = 0;
            //Thread.Sleep(2);

            //while (serialPort1.BytesToRead > 0)
            bool ChecksumReceived = false;
            while(!ChecksumReceived)
            {
                try
                {
                    serialPort1.ReadTimeout = 50;
                    CurrentByte = (serialPort1.ReadByte());
                    BytesRead++;
                    if (UseFirstBuffer) { dataBuffer.Add(CurrentByte); }
                    else { dataBuffer1.Add(CurrentByte); }

                    switch (PacketStatus)
                    {
                        //Packet status 1 =  Looking for "DE" (dec 222)
                        case 1:
                            if (CurrentByte == 222) { PacketStatus++; }
                            break;
                        //Packet status 2 = Found "DE" now Looking for "AD" (dec 173)
                        case 2:
                            if (CurrentByte == 222)
                            { break; }
                            if (CurrentByte == 173)
                            { PacketStatus++; }
                            else
                            { PacketStatus--; }
                            break;
                        case 3:
                            ReportedByteCount = CurrentByte;
                            PacketStatus++;
                            break;
                        //Packet status 4 = Found DE and AD. This is the checksum byte. Send packet for processing
                        case 4:
                            PacketStatus = 1;
                            ChecksumReceived = true;
                            serialPort1.DiscardInBuffer();
                            intPacketsRecd++;
                            UseFirstBuffer = !UseFirstBuffer;
                            this.Invoke(new EventHandler(RXPacketProcessing));
                            break;
                    }
                }
                catch (Exception error)
                {
                    serialPort1.DiscardInBuffer();
                    ErrorCount++;
                    if(ErrorCount > 1)
                    {

                        dataBuffer.Clear();
                        dataBuffer1.Clear();
                        MessageBox.Show(error.Message + "There was a problem reading from the ECU serial port. Check the COM port settings to be sure they match the ECU settings.");
                        break;
                    }

                }
                //Thread.Sleep(0);
            }
        }
        //Copy packet from databuffer or databuffer1 to BufferBeingProcessed and then validate checksum
        //If checksum is valid, process all packet data, update Tboxes and variables, update tracelog,
        //set next breakpoint, etc
        private void RXPacketProcessing(object sender, EventArgs e)
        {
            try
            {
                int intPacketStartIndex;
                BufferBeingProcessed.Clear();
                if (UseFirstBuffer)
                {
                    intBufferLength = dataBuffer1.Count;
                    intPacketStartIndex = intBufferLength - ReportedByteCount;
                    BufferBeingProcessed = dataBuffer1.GetRange(intPacketStartIndex, ReportedByteCount);
                    dataBuffer1.Clear();
                }
                else
                {
                    intBufferLength = dataBuffer.Count;
                    intPacketStartIndex = intBufferLength - ReportedByteCount;
                    BufferBeingProcessed = dataBuffer.GetRange(intPacketStartIndex, ReportedByteCount);
                    dataBuffer.Clear();
                }

                CheckSumPassed = CheckCheckSum(BufferBeingProcessed);

                if (CheckSumPassed )
                {
                    this.ProcessSerialPacketUpdateTraceAndTboxes();
                }
                else
                {
                    MessageBox.Show("Serial Packet from ECU failed Checksum");
                }
                }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                serialPort1.DiscardInBuffer();
            }
        }
        
        //Validate packet checksum and clear one of the temporary buffers
        private bool CheckCheckSum(List<int> datainput)
        {
            int PacketStatus = 1;
            int CheckSum = 0;
            bool SumIsGood = false;
            foreach (int RXbyte in datainput)
            {
                CheckSum = CheckSum + RXbyte;
                switch (PacketStatus)
                {
                    //Packet status 1 =  Looking for "DE" (dec 222)
                    case 1:
                        if (RXbyte == 222) { PacketStatus++; }
                        break;
                    //Packet status 2 = Found "DE" now Looking for "AD" (dec 173)
                    case 2:
                        if (RXbyte == 173)
                        {
                            PacketStatus++;
                        }
                        else if (RXbyte == 222)
                        { break; }
                        else
                        { PacketStatus--; }
                        break;
                    //Packet status 3 = Found DE and AD. Get Byte count
                    case 3:
                        ReportedByteCount = RXbyte;
                        PacketStatus++;
                        break;
                    case 4:
                        PacketStatus = 1;
                        CheckSum = CheckSum - RXbyte;
                        ushort WordCheckSum = Convert.ToUInt16(CheckSum);
                        byte ByteCheckSum = (byte)(WordCheckSum & 0xFF);
                        if (ByteCheckSum == RXbyte)
                        {
                            SumIsGood = true;
                        }
                        else
                        {
                            SumIsGood = false;
                        }
                        break;
                }
            }
            if (UseFirstBuffer) { dataBuffer1.Clear(); } else { dataBuffer.Clear(); }
            return SumIsGood;
        }
        //Prepare output for tracelog
        private string CreateTraceLogRegisterString()
        {
                string strRegisters = "-----------------------------------------------------------------------------\r\n\r\n";
                strRegisters += intPacketsRecd + ")        BP =" + strTrace_BP + "\r\n" +
                   "                    r1  r0           r3  r2           r5  r4           r7  r6" + "\r\n" +
                   "              " + "er0 ="            + strTrace_R1    + strTrace_R0         + "    "  + 
                                      "er1 ="            + strTrace_R3    + strTrace_R2         + "    "  + 
                                      "er2 ="            + strTrace_R5    + strTrace_R4         + "    "  +
                                      "er3 ="            + strTrace_R7    + strTrace_R6         + "\r\n"  +
                   "              " + "MIE ="            + strTrace_MIE   + "         "         + "SCB =" + strTrace_SCB   + "         " + "LRB =" + strTrace_LRB    + "      "    + "SF  =" + strTrace_SF            + "\r\n" +
                   "              " + "PL4 ="            + strTrace_PSWL4 + "         "         + "PL5 =" + strTrace_PSWL5 + "         " + "BCB =" + strTrace_BCB    + "         " + "HC  =" + strTrace_HC            + "\r\n" +
                   "              " + "SS1 ="            + strTrace_Stack1+ "      "            + "SS2 =" + strTrace_Stack2+ "      "    + "SS3 =" + strTrace_Stack3 + "      "    + "SS4 =" + strTrace_Stack4        + "\r\n" +
                   "              " + "DP  ="            + strTrace_DP    + "      "            + "X1  =" + strTrace_X1    + "      "    + "X2  =" + strTrace_X2     + "      "    + "SSP =" + strTrace_StackPointer  + "\r\n" +
                   "              " + "CF  ="            + strTrace_CF    + "         "         + "DD  =" + strTrace_DD    + "         " + "ZF  =" + strTrace_ZF     + "         " + "PSW =" + strTrace_PSW           + "\r\n" +
                   "              " + strTrace_Xram1Addy + " = "          + strTrace_Xram1Conts + "     " + strTrace_Xram2Addy + " =" + strTrace_Xram2Conts + "      " + strRamWatchAddress + " = " + strtrace_RamWatch + "     " + "DPC =" +  strTrace_DPconts +  "\r\n" +
                   "              " + "ACC ="            + strTrace_ACC ;
                   
            try
            {
                if (ASMOpened)
                {
                    if (RenamedASMFileArray != null)
                    {
                        InstructionLine = RenamedASMFileArray[CurrentASMIndex];
                    }
                    else
                    {
                        InstructionLine = ASMFileArray[CurrentASMIndex];
                    }
                    //Show calculated pointer location and ROM contents for indirect referencing pointers in tracelog
                    if (PointerContentsHexValue != "")
                    {
                        InstructionLine = InstructionLine.Insert(InstructionLine.IndexOf(";") - 8, "( " + ContentsAddress.ToString("X2") + " = " +PointerContentsHexValue + " )");
                        lblLA11.Text = ContentsAddress.ToString("X4") + "h";
                        tboxLA1_1.Text = PointerContentsHexValue;

                        //Rename pointer location for tracelog if a rename file XML has been read already
                        if (RenameXMLReady && boolOffsetPointer)
                        {
                            string strPointerToTblName =  "tbl_" + ContentsAddress.ToString("X2").ToLower();
                            string strOriginalTblName = strPointerToTblName;
                            foreach (DataTable table in dataset1.Tables)
                            {
                                foreach (DataRow dr in table.Rows)
                                {

                                    if (Convert.ToString(dr[0]) == "True")
                                    {
                                        strPointerToTblName = CaseSenstiveReplace(strPointerToTblName, Convert.ToString(dr[1]), Convert.ToString(dr[2]));
                                    }
                                }
                            }
                            if(strOriginalTblName != strPointerToTblName)
                            {
                                InstructionLine = InstructionLine.Insert(InstructionLine.IndexOf("= ") + 2 , strPointerToTblName + " = ");
                                InstructionLine = InstructionLine.Substring(0, InstructionLine.LastIndexOf(")") + 1);
                            }

                        }
                    }
                    strRegisters += "\r\n" + "\r\n" + InstructionLine + "\r\n";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            return strRegisters;
        }
        //Get strings from Packet data, create trace string, update trace window, save to log file
        private void ProcessSerialPacketUpdateTraceAndTboxes()
        {


            //Update all register strings and textboxes from ECU serial packet
            GetVariablesAndStringsFromDataPacket(BufferBeingProcessed);

            if (boolNewBreakpoint)
            {
                boolNewBreakpoint = false;
                WriteTheNextBreakpoint();
                displayExtraRamData();
            }

            //Create PSW bit arrray and parse data into strings for tboxes and trace log
            PSWBitArray = CreateBitArray(intPSWValue, 16);
            ParsePSWIntoStringsUpdateTboxes(PSWBitArray);

            //Build string for the trace window and log file
            strTraceLog = CreateTraceLogRegisterString();
            
            //Update Tracelog window
            tboxTraceWindow.AppendText(strTraceLog);
            
            //Save data to log file
            if(cboxSaveTrace.Checked) SaveDataToTxtFile(pathFile, strTraceLog, true);

            //Report LRB or SCB conflicts
            if (LRBConflict)
            {
                MessageBox.Show("The debugger is using the same LRB address as the code that is being debugged. Change the LRB to a different " +
                                "address or the register data will be invalid. ", "LRB Conflict");
                LRBConflict = false;
            }
            if (SCBConflict)
            {
                MessageBox.Show("The debugger is using the same SCB address as the code that is being debugged.  " +
                                "Change the SCB (Lowest 3 bits of PSW) or the pointer register data (DP, X1, X2) may be invalid.", "PSW Conflict");
                SCBConflict = false;
            }
        }

        //Get variables and text data from the serial packet, update tboxes
        private void GetVariablesAndStringsFromDataPacket(List<int> dataInput) //Update all register strings and text boxes 
        {
            int PacketIndex = 0;
            string OldDP = "";
            string OldX1 = "";
            string OldX2 = "";
            string OldPSW = "";
            string OldLRB = "";
            string OldACC = "";
            string OldBP = "";
            string OldR0;
            string OldR1;
            string OldR2;
            string OldR3;
            string OldR4;
            string OldR5;
            string OldR6;
            string OldR7;
            string OldStackPointer = "";
            string Old_Stack1 = "";
            string Old_Stack2 = "";
            string Old_Stack3 = "";
            string Old_Stack4 = "";
            string Old_Stack5 = "";
            string Old_DPconts = "";
            

            //Clear values for data that's not always available 
            strTrace_Stack1 = "      ";
            strTrace_Stack2 = "      ";
            strTrace_Stack3 = "      ";
            strTrace_Stack4 = "      ";
            strTrace_Stack5 = "      ";
            tboxStack1.Text = "";
            tboxStack2.Text = "";
            tboxStack3.Text = "";
            tboxStack4.Text = "";
            tboxStack5.Text = "";

            //Process byte from packet, stopping before the footer, byte count, and checksum
            foreach (int element in dataInput)
            {
                //Data = packet size - 4 for 2 termination bytes, byte count, and checksum
                if (PacketIndex == ReportedByteCount - 4) break;

                switch (PacketIndex)
                {
                    case 0:
                        OldR0 = strPlain_R0;
                        strPlain_R0 = element.ToString("X2");
                        tboxR0.Text = strPlain_R0;
                        strTrace_R0 = AddBracketsIfNotEqual(OldR0, strPlain_R0);
                        break;
                    case 1:
                        OldR1 = strPlain_R1;
                        strPlain_R1 = element.ToString("X2");
                        tboxR1.Text = strPlain_R1;
                        strTrace_R1 = AddBracketsIfNotEqual(OldR1, strPlain_R1);
                        break;
                    case 2:
                        OldR2 = strPlain_R2;
                        strPlain_R2 = element.ToString("X2");
                        tboxR2.Text = strPlain_R2;
                        strTrace_R2 = AddBracketsIfNotEqual(OldR2, strPlain_R2);
                        break;
                    case 3:
                        OldR3 = strPlain_R3;
                        strPlain_R3 = element.ToString("X2");
                        tboxR3.Text = strPlain_R3;
                        strTrace_R3 = AddBracketsIfNotEqual(OldR3, strPlain_R3);
                        break;
                    case 4:
                        OldR4 = strPlain_R4;
                        strPlain_R4 = element.ToString("X2");
                        tboxR4.Text = strPlain_R4;
                        strTrace_R4 = AddBracketsIfNotEqual(OldR4, strPlain_R4);
                        break;
                    case 5:
                        OldR5 = strPlain_R5;
                        strPlain_R5 = element.ToString("X2");
                        tboxR5.Text = strPlain_R5;
                        strTrace_R5 = AddBracketsIfNotEqual(OldR5, strPlain_R5);
                        break;
                    case 6:
                        OldR6 = strPlain_R6;
                        strPlain_R6 = element.ToString("X2");
                        tboxR6.Text = strPlain_R6;
                        strTrace_R6 = AddBracketsIfNotEqual(OldR6, strPlain_R6);
                        break;
                    case 7:
                        OldR7 = strPlain_R7;
                        strPlain_R7 = element.ToString("X2");
                        tboxR7.Text = strPlain_R7;
                        strTrace_R7 = AddBracketsIfNotEqual(OldR7, strPlain_R7);
                        break;
                    //[DP] RAM Contents high byte
                    case 8:
                        Old_DPconts = strPlain_DPconts;
                        intDPConts_RAM = element << 8;
                        strPlain_DPconts = element.ToString("X2");
                        break;
                    //[DP] RAM Contents low byte
                    case 9:
                        intDPConts_RAM += element;
                        strPlain_DPconts += element.ToString("X2");
                        strTrace_DPconts = AddBracketsIfNotEqual(Old_DPconts, strPlain_DPconts);
                        break;

                    //DP high Byte
                    case 10:
                        OldDP = strPlain_DP;
                        intDPAddress = element << 8;
                        strPlain_DP = element.ToString("X2");
                        break;
                    //DP Low byte
                    case 11:
                        intDPAddress += element;
                        strPlain_DP += element.ToString("X2");
                        tboxDP.Text = strPlain_DP;
                        strTrace_DP = AddBracketsIfNotEqual(OldDP, strPlain_DP);
                        
                        //Update DP RAM contents tbox
                        tboxDPconts.Text = strPlain_DPconts;

                        //Update DP ROM contents tbox
                        tboxDPConts_ROM.Text = "";
                        if(intDPAddress < 0x8000 && intDPAddress > 0 && BINOpened) tboxDPConts_ROM.Text = ((BinWithPatchesAndDebuggerCode[intDPAddress + 1] << 8) + BinWithPatchesAndDebuggerCode[intDPAddress]).ToString("X4");
                        
                        //Update DP Ram binary display
                        lblDPContsBinary.Text = "";

                        //Byte for binary label = DP low byte
                        byte DPByteForBinary = (byte)intDPConts_RAM;

                        //If DP address is not even the high byte is the actual addressed byte
                        if ((intDPAddress & 1) == 1)
                        {
                            DPByteForBinary = (byte)(intDPConts_RAM >> 8); 
                        }
                        lblDPContsBinary.Text = Convert.ToString((DPByteForBinary) >> 4, 2).PadLeft(4, '0') + " " + Convert.ToString((DPByteForBinary & 0xf), 2).PadLeft(4, '0');
                        break;
                    //X1 high Byte
                    case 12:
                        OldX1 = strPlain_X1;
                        strPlain_X1 = element.ToString("X2");
                        break;
                    //X1 low byte
                    case 13:
                        strPlain_X1 += element.ToString("X2");
                        tboxX1.Text = strPlain_X1;
                        strTrace_X1 = AddBracketsIfNotEqual(OldX1, strPlain_X1);
                        break;
                    //X2 high Byte
                    case 14:
                        OldX2 = strPlain_X2;
                        strPlain_X2 = element.ToString("X2");
                        break;
                    //X2 low byte
                    case 15:
                        strPlain_X2 += element.ToString("X2");
                        tboxX2.Text = strPlain_X2;
                        strTrace_X2 = AddBracketsIfNotEqual(OldX2, strPlain_X2);
                        break;
                    //PSWH
                    case 16:
                        OldPSW = strPlain_PSW;
                        intPSWValue = element << 8;
                        strPlain_PSWH = element.ToString("X2");
                        strPlain_PSW = strPlain_PSWH;
                        tboxPSWH.Text = strPlain_PSWH;

                        break;
                    //PSWL
                    case 17:
                        intPSWValue += element;
                        strPlain_PSWL = element.ToString("X2");
                        strPlain_PSW += strPlain_PSWL;
                        //Check to see if the debugger is using the same SCB as the debugee
                        if ((int.Parse(strPlain_PSW, System.Globalization.NumberStyles.HexNumber) & 7) == (PSWDebuggerValue & 7)) SCBConflict = true;
                        tboxPSWL.Text = strPlain_PSWL;
                        strTrace_PSW = AddBracketsIfNotEqual(OldPSW, strPlain_PSW);
                        break;
                    //LRB high Byte
                    case 18:
                        OldLRB = strPlain_LRB;
                        strPlain_LRB = element.ToString("X2");
                        break;
                    //LRB low byte
                    case 19:
                        strPlain_LRB += element.ToString("X2");
                        //Check to see if the debugger is using the same LRB as the debugee
                        if (int.Parse(strPlain_LRB, System.Globalization.NumberStyles.HexNumber) == LRBDebuggerValue) LRBConflict = true;
                        tboxLRB.Text = strPlain_LRB;
                        strTrace_LRB = AddBracketsIfNotEqual(OldLRB, strPlain_LRB);
                        break;
                    //Acc High Byte
                    case 20:
                        OldACC = strPlain_ACC;
                        strPlain_ACC = element.ToString("X2");
                        break;
                    //ACC low byte
                    case 21:
                        strPlain_ACC += element.ToString("X2");
                        tboxACC.Text = strPlain_ACC;
                        strTrace_ACC = AddBracketsIfNotEqual(OldACC, strPlain_ACC);
                        break;
                    //BP High Byte
                    case 22:
                        OldBP = strPlain_BP;
                        intCurrent_BP = element << 8;
                        strPlain_BP = element.ToString("X2");
                        break;
                    //BP low byte
                    case 23:
                        intCurrent_BP = intCurrent_BP + element;
                        strPlain_BP += element.ToString("X2");
                        tboxECUReturnAddress.Text = strPlain_BP;
                        strTrace_BP = "[" + strPlain_BP + "]";
                        //Breakpoint data has been updated, write next jump
                        if (OldBP != strPlain_BP)
                        {
                            //WriteTheNextBreakpoint();
                            boolNewBreakpoint = true;
                            strTrace_BP = " " + strPlain_BP + " ";
                        }
                        break;
                    //Extra RAM Lookup 1 byte 1
                    case 24:
                        intXramLA11Value = element;
                        break;
                    //Extra RAM Lookup 1 byte 2
                    case 25:
                        intXramLA12Value = element;
                        break;
                    //Extra RAM Lookup 2 byte 1
                    case 26:
                        intXramLA21Value = element;
                        break;
                    //Extra RAM Lookup 2 byte 2
                    case 27:
                        intXramLA22Value = element;
                        break;
                    //RAM watch byte 1
                    case 28:
                        intRAMWatchByte1 = element;
                        intRamWatchValue = intRAMWatchByte1;
                        break;
                    //Ram watch Byte 2
                    case 29:
                        intRAMWatchByte2 = element;
                        intRamWatchValue += (intRAMWatchByte2 << 8);
                        strtrace_RamWatch = intRamWatchValue.ToString("X4");
                        break;
                    //Stack pointer high
                    case 30:
                        OldStackPointer = strPlain_StackPointer;
                        strPlain_StackPointer = element.ToString("X2");
                        break;
                    //Stack pointer Low
                    case 31:
                        strPlain_StackPointer += element.ToString("X2");
                        strTrace_StackPointer = AddBracketsIfNotEqual(OldStackPointer, strPlain_StackPointer);
                        tboxStackPointer.Text = strPlain_StackPointer;
                        break;
                    //Top of stack High byte (PC for normal RT or PSW for ISR RTI)
                    case 32:
                        Old_Stack1 = strPlain_Stack1;
                        intStack1Value = element << 8;
                        strPlain_Stack1 = element.ToString("X2");
                        break;
                    //Top of stack low byte
                    case 33:
                        intStack1Value = intStack1Value + element;
                        strPlain_Stack1 += element.ToString("X2");
                        tboxStack1.Text = strPlain_Stack1;
                        strTrace_Stack1 = AddBracketsIfNotEqual(Old_Stack1, strPlain_Stack1);
                        break;
                    //Stack 2 High byte (LRB for ISR RTI)
                    case 34:
                        Old_Stack2 = strPlain_Stack2;
                        strPlain_Stack2 = element.ToString("X2");
                        break;
                    //Stack 2 low byte 
                    case 35:
                        strPlain_Stack2 += element.ToString("X2");
                        tboxStack2.Text = strPlain_Stack2;
                        strTrace_Stack2 = AddBracketsIfNotEqual(Old_Stack2, strPlain_Stack2);
                        break;
                    //Stack 3 high Byte (ACC for ISR RTI)
                    case 36:
                        Old_Stack3 = strPlain_Stack3;
                        strPlain_Stack3 = element.ToString("X2");
                        break;
                    //Stack 3 low byte
                    case 37:
                        strPlain_Stack3 += element.ToString("X2");
                        tboxStack3.Text = strPlain_Stack3;
                        strTrace_Stack3 = AddBracketsIfNotEqual(Old_Stack3, strPlain_Stack3);
                        break;
                    //Stack 4 High byte (PC for RTI if in ISR)
                    case 38:
                        Old_Stack4 = strPlain_Stack4;
                        intStack4Value = element << 8;
                        strPlain_Stack4 = element.ToString("X2");
                        break;
                    //stack4 lo
                    case 39:
                        intStack4Value = intStack4Value + element;
                        strPlain_Stack4 += element.ToString("X2");
                        tboxStack4.Text = strPlain_Stack4;
                        strTrace_Stack4 = AddBracketsIfNotEqual(Old_Stack4, strPlain_Stack4);
                        break;
                    //Stack 5 hi byte (Top of stack pre ISR)
                    case 40:
                        Old_Stack5 = strPlain_Stack5;
                        strPlain_Stack5 = element.ToString("X2");
                        break;
                    //Stack 5
                    case 41:
                        strPlain_Stack5 += element.ToString("X2");
                        tboxStack5.Text = strPlain_Stack5;
                        strTrace_Stack5 = AddBracketsIfNotEqual(Old_Stack5, strPlain_Stack5);
                        break;
                    default:
                        break;
                }
                PacketIndex++;
            }


            return;
        }

        //Convert byte or word into bit array
        private System.Collections.BitArray CreateBitArray(int value, int BitCount)
        {
            System.Collections.BitArray BitArray;
            string BitString = Convert.ToString(value, 2).PadLeft(BitCount, '0');

            BitArray = new System.Collections.BitArray(BitString.Select(s => s == '1').ToArray());
            return BitArray;
        }

        //Parse PSW Bit Array
        private void ParsePSWIntoStringsUpdateTboxes(System.Collections.BitArray bitArray)
        {
            int ReversedOrderBitNumber = 0;
            string OldCF ;
            string OldZF ;
            string OldHC ;
            string OldDD ;
            string OldSF ;
            string OldMIE;
            string OldBCB = "" ;
            string OldPSWL4;
            string OldPSWL5;
            string OldSCB ="";

            foreach (bool element in bitArray)
            {
                
                switch (ReversedOrderBitNumber)
                {
                    //PSWH Bit 7 CF
                    case 0:
                        OldCF = strPlain_CF;
                        strPlain_CF = element ? "1" : "0";
                        tboxCF.Text = strPlain_CF;
                        strTrace_CF = " " + strPlain_CF + " ";
                        if (OldCF != strPlain_CF)
                        {
                            strTrace_CF = "[" + strPlain_CF + "]";
                        }
                        break;
                   
                    //PSWH Bit 6 ZF
                    case 1:
                        OldZF = strPlain_ZF;
                        strPlain_ZF = element ? "1" : "0";
                        tboxZF.Text = strPlain_ZF;
                        strTrace_ZF = " " + strPlain_ZF + " ";
                        if (OldZF != strPlain_ZF)
                        {
                            strTrace_ZF = "[" + strPlain_ZF + "]";
                        }
                        break;
                    //PSWH Bit 5 HC
                    case 2:
                        OldHC = strPlain_HC;
                        strPlain_HC = element ? "1" : "0";
                        tboxHC.Text = strPlain_HC;
                        strTrace_HC = " " + strPlain_HC + " ";
                        if (OldHC != strPlain_HC)
                        {
                            strTrace_HC = "[" + strPlain_HC + "]";
                        }
                        break;
                    //PSWH Bit 4 DD
                    case 3:
                        DDFlag = element;
                        OldDD = strPlain_DD;
                        strPlain_DD = element ? "1" : "0";
                        tboxDD.Text = strPlain_DD;
                        strTrace_DD = AddBracketsIfNotEqual(OldDD, strPlain_DD);
                        int DD = Convert.ToInt16(element);
                        break;
                    //PSWH Bit 3
                    case 4:
                        break;
                    //PSWH Bit 2 
                    case 5:
                        break;
                    //PSWH Bit 1 SF
                    case 6:
                        OldSF = strPlain_SF;
                        strPlain_SF = element ? "1" : "0";
                        tboxSF.Text = strPlain_SF;
                        strTrace_SF = " " + strPlain_SF + " ";
                        if (OldSF != strPlain_SF)
                        {
                            strTrace_SF = "[" + strPlain_SF + "]";
                        }
                        break;
                    //PSWH Bit 0 MIE
                    case 7:
                        OldMIE = strPlain_MIE;
                        strPlain_MIE = element ? "1" : "0";
                        tboxMIE.Text = strPlain_MIE;
                        strTrace_MIE = " " + strPlain_MIE + " ";
                        if (OldMIE != strPlain_MIE)
                        {
                            strTrace_MIE = "[" + strPlain_MIE + "]";
                        }
                        break;
                    //PSWL Bit 7
                    case 8:
                    //PSWL Bit 6
                    case 9:
                        break;
                    //PSWL Bit 5 BCB
                    case 10:
                        OldPSWL5 = strPlain_PSWL5;
                        OldBCB = strPlain_BCB;
                        strPlain_PSWL5 = element ? "1" : "0";
                        strPlain_BCB = strPlain_PSWL5;
                        tboxPSWL5.Text = strPlain_PSWL5;
                        strTrace_PSWL5 = " " + strPlain_PSWL5 + " ";
                        if (OldPSWL5 != strPlain_PSWL5)
                        {
                            strTrace_PSWL5 = "[" + strPlain_PSWL5 + "]";
                        }
                        break;

                    //PSWL Bit 4 BCB
                    case 11:
                        OldPSWL4 = strPlain_PSWL4;
                        strPlain_PSWL4 = element ? "1" : "0";
                        strPlain_BCB += strPlain_PSWL4;
                        strPlain_BCB = Convert.ToString(Convert.ToInt16(strPlain_BCB, 2));
                        tboxPSWL4.Text = strPlain_PSWL4;
                        tboxBCB.Text = strPlain_BCB;
                        strTrace_PSWL4 = " " + strPlain_PSWL4 + " ";
                        strTrace_BCB = " " + strPlain_BCB + " ";
                        if (OldBCB != strPlain_BCB)
                        {
                            strTrace_BCB = "[" + strPlain_BCB + "]";
                        }
                        if (OldPSWL4 != strPlain_PSWL4)
                        {
                            strTrace_PSWL4 = "[" + strPlain_PSWL4 + "]";
                        }
                        break;
                    //PSWL Bit 3
                    case 12:
                        break;
                    //PSWL Bit 2
                    case 13:
                        OldSCB = strPlain_SCB;
                        strPlain_SCB = element ? "1" : "0";
                        break;
                    //PSWL Bit 1
                    case 14:
                        strPlain_SCB += element ? "1" : "0";
                        break;
                    //PSWL Bit 0
                    case 15:
                        strPlain_SCB += element ? "1" : "0";
                        strPlain_SCB = Convert.ToString(Convert.ToInt32(strPlain_SCB, 2));
                        tboxSCB.Text = strPlain_SCB;
                        strTrace_SCB = " " + strPlain_SCB + " ";
                        if( OldSCB != strPlain_SCB)
                        {
                            strTrace_SCB = "[" + strPlain_SCB + "]";
                        }
                        break;
                }

                ReversedOrderBitNumber++;
            }
            return;
            
        }
        
        //Compare the original and Current string. Add [] if differnt, spaces if the same
        private string AddBracketsIfNotEqual(string Orig, string Current)
        {
            if(Orig != Current)
            {
                Current = "[" + Current + "]";
                return Current;
            }
            Current = " " + Current + " ";
            return Current;
        }

        //Save Trace log file
        private void SaveDataToTxtFile(string Filepath, string stringToWrite, bool append)
        {
            try
            {
                //objStreamWriter = new StreamWriter(pathFile, state_AppendText);
                objStreamWriter = new StreamWriter(Filepath, append);
                objStreamWriter.WriteLine(stringToWrite);
                objStreamWriter.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        //Get available ports for Serial 1 - ECU serial
        private void cBoxCOMPORT_DropDown(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxCOMPORT.Items.Clear();
            cBoxCOMPORT.Items.AddRange(ports);
        }
       
        //Open Serial port 1 - ECU serial port
        private void btnOpenCOMPort_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cBoxCOMPORT.Text;
                serialPort1.BaudRate = Convert.ToInt32(CBoxBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopBits.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParityBits.Text);
                
                serialPort1.Open();
                progressBar1.Value = 100;
                cBoxCOMPORT.Enabled = false;
                CBoxBaudRate.Enabled = false;
                cBoxDataBits.Enabled = false;
                cBoxStopBits.Enabled = false;
                cBoxParityBits.Enabled = false;
            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Close Serial port 1 - ECU serial port
        private void btnCloseCOMPort_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            cBoxCOMPORT.Enabled = true;
            CBoxBaudRate.Enabled = true;
            cBoxDataBits.Enabled = true;
            cBoxStopBits.Enabled = true;
            cBoxParityBits.Enabled = true;
            if (serialPort1.IsOpen)
            {
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
               
            }
        }

        //------------------- BIN and EMU Handling --------------------------------
        
        //Patch bin using .code file and upload to ostrich (also update PSW LRB IE nuds)
        private void AddDebuggerCodeAndPatchesToRomAndUpload()
        {
            //No Bin file has been 
            if (!BINOpened)
            {
                MessageBox.Show("Please open a BIN file first.");
                return;
            }
            
            //Problem getting ROM settings, return
            if (!getDebuggerSettingsForROM())
            {
                return;
            }
            try
            {
                CreateRomCallArraysBasedOnRomAddress(Convert.ToInt32(nudROMCodeAddress.Value));
                AddDebuggerCodeToRom();
                SetECUBaudRateInROM(BinWithPatchesAndDebuggerCode, BaudRateAddress1, BaudRateAddress2, BaudRatePatchValue);
                AddConfigPatchesToRom();
                reRouteVCALinRomVectorTable(BinWithPatchesAndDebuggerCode, VCALVectorTableLoc, intCodeStubAddress, false);
                UpdateLRB_PSW_IE_Nuds();
                if (cboxSlowUpload.Checked)
                {
                    OstrichSlowWriteEntireRom(BinWithPatchesAndDebuggerCode);
                }
                else
                {
                    OstrichWriteBulk(BinWithPatchesAndDebuggerCode);
                }
                BinHasBeenUploadedSinceCreation = true;
                ROMWhichLoopEquals1 = false;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }
        
        //Open BIN file and create 3 working arrays of it.  Decompile the BIN and use the dasm to update the ASM window. Rename the asm content if needed
        private void CreateBINArraysDecompileBINandUpdateASMWindow()
        {
            tboxOpenFileSimpleName.Text = System.IO.Path.GetFileName(strBINPath);
            currentBINArray = System.IO.File.ReadAllBytes(strBINPath);
            OstrichBinArrayOut = System.IO.File.ReadAllBytes(strBINPath);
            BinWithPatchesAndDebuggerCode = System.IO.File.ReadAllBytes(strBINPath);
            rtboxASMFile.Text = "";
            string ASMPath = System.IO.Path.GetDirectoryName(strBINPath) + System.IO.Path.GetFileNameWithoutExtension(strBINPath);
            if (!usingASMFile) ASMPath += "_DBG.asm";
            else ASMPath += ".asm";
            if (decompileBIN(strBINPath, ASMPath, false, getDasmArgs()))
            {
                ASMOpened = true;
                CurrentASMPath = ASMPath;
                LoadASMFileIntoArraysRenameIfNeededAndUpdateASMWindow();

            }


        }

        //Add baud patches to bin to be uploaded
        private void SetECUBaudRateInROM(byte[] DestinationArray, int RomAddress1, int RomAddress2, int BaudByte)
        {

            DestinationArray[RomAddress1] = (byte)BaudByte;
            DestinationArray[RomAddress2] = (byte)BaudByte;
        }
        //This routine will evaluate the distance between the jump address and the next instruction address and then write the calls to the dumper routine
        // if there is not enough room inbetween, the jump offset will be changed to make room and the return address modifier will be changed to reflect the intended return address
        //Much of this code is pointless with the addition of VCAL routing since the call only occupies 1 byte
        private void OrganizeAndWriteJumpsToEMU()
        {

            try
            {
                //This first part will set boundaries for a 256 byte write to the ostrich. 
                // We're going to write all the different changes to an array and then execute 1 write for all the changes in the 256 byte block
                //If something is outside of the boundaries, we will write it seperately
                
                //Start with a clean copy of the BIN file with debugger settings 
                Array.Copy(BinWithPatchesAndDebuggerCode, 0, OstrichBinArrayOut, 0, BinWithPatchesAndDebuggerCode.Length);
                
                //Clear the lists used for determining the final chunk size
                chunkAddresses.Clear();
                addressLength.Clear();
                
                //Setting boundaries
                setChunkBoundariesAndChunkSize(intNextInstructionAddress);

                //Restore previous breakpoints if they are outside of the new chunk
                if (!InitialBPAlreadyRemoved  && !WithinRange(InitialOstrichBP, byteCodeStubCAll.Length, LowerBoundary, UpperBoundary))
                {
                    OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, InitialOstrichBP,InitialOstrichBP, (byte)byteCodeStubCAll.Length, true);
                    InitialBPAlreadyRemoved = true;
                }
                else if(!InitialBPAlreadyRemoved)
                {
                    chunkAddresses.Add(InitialOstrichBP);
                    addressLength.Add(byteCodeStubCAll.Length);
                    InitialBPAlreadyRemoved = true;
                }

                if (intPrevBranchAddress > 0 && !WithinRange(intPrevBranchAddress, byteCodeStubCAll.Length, LowerBoundary, UpperBoundary))
                {
                    OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, intPrevBranchAddress, intPrevBranchAddress, (byte)byteCodeStubCAll.Length, true);
                }
                else
                {
                    chunkAddresses.Add(intPrevBranchAddress);
                    addressLength.Add(byteCodeStubCAll.Length);
                }

                if (intPrevCallAddress > 0 && !WithinRange(intPrevCallAddress, byteCodeStubCAll.Length, LowerBoundary, UpperBoundary))
                {
                    OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, intPrevCallAddress, intPrevCallAddress, (byte)byteCodeStubCAll.Length, true);
                }
                else
                {
                    chunkAddresses.Add(intPrevCallAddress);
                    addressLength.Add(byteCodeStubCAll.Length);
                }


                if (intPrevNextInstruction > 0 && !WithinRange(intPrevNextInstruction - 2, byteAltCodeStubCall.Length + 2, LowerBoundary, UpperBoundary))
                {
                    OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, intPrevNextInstruction - 2, intPrevNextInstruction - 2, (byte)(byteAltCodeStubCall.Length + 2), true);
                }
                else
                {
                    chunkAddresses.Add(intPrevNextInstruction);
                    addressLength.Add(byteCodeStubCAll.Length + 2);
                }


                //This looks for DD related dasm errors and adjusts for them. EG- Listed as CMPB only looks at the next byte, but the ECU is using CMP and looking at the next 2
                if (DDFlagCorrection)
                {
                    DialogResult dialogResult = MessageBox.Show("The debugger has detected a CMPB instruction while the DD = 1.\r\n"
                        + "Does the code look like it should be treated as CMP instead?" + "\r\n\r\n"
                        + ASMFileArray[CurrentASMIndex - 1].Substring(16, ASMFileArray[CurrentASMIndex].Length - 16) + "\r\n"
                        + ASMFileArray[CurrentASMIndex].Substring(16, ASMFileArray[CurrentASMIndex].Length - 16) + "\r\n"
                        + ASMFileArray[CurrentASMIndex + 1].Substring(16, ASMFileArray[CurrentASMIndex + 1].Length - 16) + "\r\n"
                        + ASMFileArray[CurrentASMIndex + 2].Substring(16, ASMFileArray[CurrentASMIndex + 2].Length - 16),
                        "DD Instruction Conflict", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        intNextInstructionAddress++;
                    }
                    DDFlagCorrection = false;
                }

                //Branch address handling - Within the chunk size
                if (intBranchAddress > 0 && WithinRange(intBranchAddress, byteCodeStubCAll.Length, LowerBoundary, UpperBoundary))
                {

                    //Branch address handling when the branch is too close to the next instruction

                    //This is the alternate debugger call array 
                    //      Jump offset       Call1         Call2
                    //Value    04         32 XX XX 03   32 XX XX 06
                    //Byte      0          1  2  3  4    5  6  7  8
                    //Byte 0 (04) reroutes the Jump instruction to 4 bytes after the next byte, which would be byte 5 in our array
                    //Byte 5 is the call to the debugger code stub
                    //If the next sequential instruction is executed, the first call at byte 1 will be executed
                    //If the branch instruction is executed then the second call at byte 5 will be executed
                    //Byte 4 and byte 6 are the return address offsets used by the debugger code stub, to send the code back from the trap loops to the correct address (after the jumps are cleared and original code restored)

                    //If there is a branch possible, and it's less than 4 bytes ahead of the next instruction, re-route the jump offset to the alt code call, and send the appropriate offset return byte
                    //And we're not using VCAL re-routing
                    if (intBranchAddress > intNextInstructionAddress && (intBranchAddress - intNextInstructionAddress) < 4 && !usingVCALreRoute)
                    {
                        int N = 7 - BinWithPatchesAndDebuggerCode[(intNextInstructionAddress - 1)];
                        setAltStubCallAndCopyToOutArray((byte)N);
                    }

                    //If the branch instruction is behind the next instruction by less than 7 bytes
                    //And we're not using VCAL re-routing
                    else if (intNextInstructionAddress > intBranchAddress && (intNextInstructionAddress - intBranchAddress) < 7 && !usingVCALreRoute)
                    {
                        int N = 263 - BinWithPatchesAndDebuggerCode[(intNextInstructionAddress - 1)];
                        setAltStubCallAndCopyToOutArray((byte)N);
                    }
                    //branch address is out of the way, but within the chunk. Use normal debugger calls at branch address and next sequential address
                    //may or may not be using VCAL re-routing
                    else
                    {
                        Array.Copy(byteCodeStubCAll, 0, OstrichBinArrayOut, intBranchAddress, byteCodeStubCAll.Length);
                        Array.Copy(byteCodeStubCAll, 0, OstrichBinArrayOut, intNextInstructionAddress, byteCodeStubCAll.Length);
                        chunkAddresses.Add(intBranchAddress);
                        addressLength.Add(byteCodeStubCAll.Length);
                        chunkAddresses.Add(intNextInstructionAddress);
                        addressLength.Add(byteCodeStubCAll.Length);
                    }
                }

                //No branch or It is outside the chunk boundaries
                else
                {
                    Array.Copy(byteCodeStubCAll, 0, OstrichBinArrayOut, intNextInstructionAddress, byteCodeStubCAll.Length);
                    chunkAddresses.Add(intNextInstructionAddress);
                    addressLength.Add(byteCodeStubCAll.Length);

                    if (intBranchAddress > 0 && !WithinRange(intBranchAddress, byteCodeStubCAll.Length, LowerBoundary, UpperBoundary))
                    {
                        OstrichWriteSpecific_256Max(byteCodeStubCAll, intBranchAddress);
                    }
                }

                //About to step into a CAL
                if (cboxStepInto.Checked && intCallAddress > 0)
                {
                    //Call address is outside of the "chunk" boundaries. Write the debugger call to it's address directly
                    if (!WithinRange(intCallAddress, byteCodeStubCAll.Length, LowerBoundary, UpperBoundary))
                    {
                        OstrichWriteSpecific_256Max(byteCodeStubCAll, intCallAddress);
                    }
                    //Call address is within our "chunk" add it to the chunk array
                    else
                    {
                        Array.Copy(byteCodeStubCAll, 0, OstrichBinArrayOut, intCallAddress, byteCodeStubCAll.Length);
                        chunkAddresses.Add(intCallAddress);
                        addressLength.Add(byteCodeStubCAll.Length);
                    }
                }
                //Trim the chunk size down to the distance between the 2 farthest addresses
                MinimizeChunkSize(ref LowerBoundary, ref UpperBoundary, ref intChunkSize);
                if (intChunkSize > 255) throw new IndexOutOfRangeException ("BP chunk size too large.");
                //Write the chunk 
                OstrichWriteSpecific_256Max(OstrichBinArrayOut, LowerBoundary, LowerBoundary, (byte)intChunkSize, true);

                //White list was changed last time by, change it back
                bool updateRomWhitelist = false;
                if (whiteListChangedFromOriginal)
                {
                    whiteListChangedFromOriginal = false;
                    CreateVCALwhiteList(ASMFileArray, VCALvectorTable, VCALNumber, VCALWhiteList);
                    updateRomWhitelist = true ;
                }

                //Check to see if any of the BP addresses are on the whitelist. If they are, remove them from the ecu
                if (usingVCALreRoute) 
                {
                    int whiteListCount = VCALWhiteList.Count;
                    if(CheckListForItem(intNextInstructionAddress, VCALWhiteList)) VCALWhiteList.RemoveAll(item => item == intNextInstructionAddress);
                    if(CheckListForItem(intBranchAddress, VCALWhiteList)) VCALWhiteList.RemoveAll(item => item == intBranchAddress);
                    if(CheckListForItem(intCallAddress, VCALWhiteList)) VCALWhiteList.RemoveAll(item => item == intCallAddress);
                    
                    if(VCALWhiteList.Count != whiteListCount)
                    {
                        whiteListChangedFromOriginal = true; 
                        updateRomWhitelist = true;
                    }

                }
                
                //Update the ECU whitelist if there has been changes to it
                if (updateRomWhitelist) 
                {
                    UpdateWhiteListOnRom(VCALWhiteList);
                    updateRomWhitelist = false;
                }

                intPrevNextInstruction = intNextInstructionAddress;
                intPrevBranchAddress = intBranchAddress;
                intPrevCallAddress = intCallAddress;
                intPrevLowerBoundary = LowerBoundary;
                intPrevUpperBoundary = UpperBoundary;
                intPrevChunkSize = intChunkSize;

                //Re-start autostep timer
                if (cboxAutoStep.Checked)
                {
                    timer1.Enabled = false;
                    timer1.Enabled = true;
                    //Task.Delay(0).ContinueWith(_ => { BreakLoopUsingRomAddress(); });
                    BreakLoopUsingRomAddress();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        
        private void MinimizeChunkSize(ref int lowerBounds, ref int upperBounds, ref int chunkLength )
        {
            int highestAddressIndex;
            int lowestAddressIndex;

            //delete all 0 entries in chunkAddress
            //delete corresponding indexes in addressLength
            for (int i = 0; i < chunkAddresses.Count; i++)
            {
                if(chunkAddresses[i] == 0)
                {
                    chunkAddresses.RemoveAt(i);
                    addressLength.RemoveAt(i);
                    i--;
                }
            }
            //Check chunkAddress list for lowest and highest index
            highestAddressIndex = chunkAddresses.IndexOf(chunkAddresses.Max());
            lowestAddressIndex = chunkAddresses.IndexOf(chunkAddresses.Min());

            //set lowerbounds to lowest index, upperBounds to highest index + length
            lowerBounds = chunkAddresses[lowestAddressIndex];
            upperBounds = chunkAddresses[highestAddressIndex] + addressLength[highestAddressIndex];

            if (lowerBounds + addressLength[lowestAddressIndex] > upperBounds) upperBounds = lowerBounds + addressLength[lowestAddressIndex];
            chunkLength = upperBounds - lowerBounds;
            

        }

        //determine if a value plus its length is within a range
        private bool WithinRange(int value, int length, int LowerLimit, int UpperLimit)
        {
            bool result = false;
            if ((value) > LowerLimit && (value + length) < UpperLimit)
            {
                result = true;
            }
            return result;
        }

        private void setChunkBoundariesAndChunkSize(int address)
        {
            LowerBoundary = 0;
            if (address >= 129)
            {
                LowerBoundary = address - 128;
            }
            UpperBoundary = 32768;
            if (address <= 32640)
            {
                UpperBoundary = address + 127;
            }
            intChunkSize = UpperBoundary - LowerBoundary;

        }

        //This will write to the ROM to break the waiting loops and allow the ECU to execute the next line of code
        private void BreakLoopUsingRomAddress()
        {

            ROMWhichLoopAddress = intCodeStubAddress + (ROMOffsetWhichLoop - intDebuggerCodeStubSourceOffset);

            if (ROMWhichLoopEquals1)
            {
                byte[] LoopBreak = new byte[1] { 0 };
                OstrichWriteSpecific_256Max(LoopBreak, ROMWhichLoopAddress, 0, 1, true);
                ROMWhichLoopEquals1 = false;
            }
            else
            {
                byte[] LoopBreak = new byte[1] { 1 };
                OstrichWriteSpecific_256Max(LoopBreak, ROMWhichLoopAddress, 0, 1, true);
                ROMWhichLoopEquals1 = true;
            }
        }
        private void OstrichRestoreArrayAndRestoreDistantROMJumps()
        {
            //This will attempt to restore the emulator to it's original Loaded bin code, with as few writes as possible.

            //Restore the output array 
            Array.Copy(BinWithPatchesAndDebuggerCode, 0, OstrichBinArrayOut, 0, BinWithPatchesAndDebuggerCode.Length);

            //Restore the original breakpoint address if it hasn't been done already and its outside the boundaries
            if (!InitialBPAlreadyRemoved && !WithinRange(InitialOstrichBP, byteCodeStubCAll.Length, intPrevLowerBoundary, intPrevUpperBoundary))
            {
                OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, InitialOstrichBP, InitialOstrichBP, (byte)byteCodeStubCAll.Length);
                InitialBPAlreadyRemoved = true;
            }


            //There was a branch and it was outside of the previous chunk, restore it
            if (intPrevBranchAddress > 0 && !WithinRange(intPrevBranchAddress, byteCodeStubCAll.Length, intPrevLowerBoundary, intPrevUpperBoundary))
            {
                OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, intPrevBranchAddress, intPrevBranchAddress, (byte)byteCodeStubCAll.Length, true);
            }

            //There was a call and it was outside of the chunk, restore it
            if (intPrevCallAddress > 0 && !WithinRange(intPrevCallAddress, byteCodeStubCAll.Length, intPrevLowerBoundary, intPrevUpperBoundary))
            {
                OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, intPrevCallAddress, intPrevCallAddress, (byte)byteCodeStubCAll.Length, true);
            }

            if (intPrevNextInstruction > 0 && !WithinRange(intPrevNextInstruction - 2, byteAltCodeStubCall.Length + 2, intPrevLowerBoundary, intPrevUpperBoundary))
            {
                OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, intPrevNextInstruction - 2, intPrevNextInstruction - 2, (byte)(byteAltCodeStubCall.Length + 2), true);
            }

            if (intPrevChunkSize != 0)
            {
                //Restore the chunk
                OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, intPrevLowerBoundary, intPrevLowerBoundary, (byte)intPrevChunkSize, true);
            }


        }

        private void OstrichSetBreakpoint(int breakpointAddress, bool restoreFirst = true)
        {
            if (BINOpened)
            {
                if(restoreFirst) OstrichRestoreArrayAndRestoreDistantROMJumps();
                OstrichWriteSpecific_256Max(byteCodeStubCAll, breakpointAddress);
                InitialOstrichBP = breakpointAddress;

                InitialBPAlreadyRemoved = false;

            }
            else
            {
                MessageBox.Show("Open a Bin file first");
            }
        }
        private void setAltStubCallAndCopyToOutArray(byte ReturnAddressOffset)
        {
            byteAltCodeStubCall[byteAltCodeStubCall.Length - 1] = ReturnAddressOffset;
            Array.Copy(byteAltCodeStubCall, 0, OstrichBinArrayOut, (intNextInstructionAddress - 1), byteAltCodeStubCall.Length);
            chunkAddresses.Add(intNextInstructionAddress - 1);
            addressLength.Add(byteAltCodeStubCall.Length);
        }


        //-------------------- Ostrich --------------------------------------------

        //Show available COM ports for ostrich
        private void cboxOstrichComPortNumber_DropDown(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cboxOstrichComPortNumber.Items.Clear();
            cboxOstrichComPortNumber.Items.AddRange(ports);
        }

        //Open ostrich serial port
        private void btnOpenOstrichCom_Click(object sender, EventArgs e)
        {
            Array.Clear(OstrichSerialNumber, 0, OstrichSerialNumber.Length);
            tboxOstrichSerialNumber.Clear();
            try
            {
                serialPort2.PortName = cboxOstrichComPortNumber.Text;
                serialPort2.BaudRate = 921600;
                serialPort2.DataBits = 8;
                serialPort2.StopBits = StopBits.One;
                serialPort2.Parity = Parity.None;
                serialPort2.RtsEnable = false;

                serialPort2.Open();
                progressBar2.Value = 100;
                byte[] OstrichRequestSerial = new byte[3] { 78, 83, 161 };
                serialPort2.Write(OstrichRequestSerial, 0, 3);
                OstrichStatus = 1;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Close ostrich port
        private void btnCloseOstrichCom_Click(object sender, EventArgs e)
        {
            tboxOstrichSerialNumber.Clear();
            if (serialPort2.IsOpen)
            {
                serialPort2.Close();
                progressBar2.Value = 0;
            }
        }

        //Display ostrich serial number and clear ostrich buffer
        private void DisplayOstrichSerialNumber(object sender, EventArgs e)
        {
            tboxOstrichSerialNumber.Text = OstrichRXDataFormat(OstrichRXBuffer);
            OstrichRXBuffer.Clear();
        }
        
        //Show that there was an error in the ostrich serial number box, show message box, set ostrich error
        private void DisplayOstrichError(object sender, EventArgs e)
        {
            tboxOstrichSerialNumber.Text = "Ostrich Error";
            MessageBox.Show("Ostrich Sent an Error");
            OstrichSentError = false;
        }

        //Handle data sent from connected ostrich - eg serial fetch and bulk write operations
        private void serialport2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            OstrichRXBuffer.Clear();
            int CurrentByte;
            int OstrichRXChecksum = 0;
            int ByteCount = 0;
            
            while (OstrichStatus != 0)
            //while (serialPort2.BytesToRead > 0)
            {

                try
                {
                    CurrentByte = (serialPort2.ReadByte());
                    OstrichRXChecksum = OstrichRXChecksum + CurrentByte;
                    ByteCount++;
                    if(CurrentByte == 63) // ? sent from ostrich, indicating an error
                    {
                        OstrichSentError = true;
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
                                this.Invoke(new EventHandler(DisplayOstrichSerialNumber));
                            }
                            else
                            {
                                OstrichRXBuffer.Add(CurrentByte);
                                OstrichSerialNumber[ByteCount - 1] = (byte)CurrentByte;
                            }
                            break;
                        //Currently bulk sending BinWithPatchesAndDebuggerCode
                        case 2:
                           if(CurrentByte == 79) //Got good response from ostrich, Continue upload
                            {
                                this.Invoke(new EventHandler(ContinueBulkWrite));
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            if (OstrichSentError)
            {
                this.Invoke(new EventHandler(DisplayOstrichError));
            }
        }
                        
        //Upload button pressed, prepare and upload BIN with patches to emulator
        private void btnLoadBinToOstrich_Click(object sender, EventArgs e)
        {
            if (usingASMFile)
            {
                compileASMtoBIN(ASMtoCompilePath, Path.GetDirectoryName(strBINPath) + Path.GetFileNameWithoutExtension(strBINPath) );
            }
            CreateBINArraysDecompileBINandUpdateASMWindow();
            LoadASMFileIntoArraysRenameIfNeededAndUpdateASMWindow();
            AddDebuggerCodeAndPatchesToRomAndUpload();
            highlightAfterValidating(Convert.ToInt32(nudBPAddress.Value));
        }

        //Write full size 32k bin to ostrich
        private void OstrichWriteBulk(byte[] SourceArray)
        {
            try
            {
                if (SourceArray.Length == 32768)
                {
                    byte Ten = (byte)(16);
                    MSB = (byte)(MSB + Ten);


                    if (BulkChunkNumber < 8)
                    {
                        BulkChunkNumber++;
                        byte Z = (byte)(90);
                        byte W = (byte)(87);
                        byte n = (byte)(16);


                        byte[] OutputArray = new byte[4102];
                        byte[] Header = new byte[5] { Z, W, n, MMSB, MSB };
                        Array.Copy(Header, 0, OutputArray, 0, Header.Length);
                        Array.Copy(SourceArray, StartChunkAddress, OutputArray, 5, 4096);

                        int Checksum = 0;
                        foreach (byte element in OutputArray)
                        {
                            Checksum = Checksum + element;
                        }

                        byte[] ChecksumByte = BitConverter.GetBytes(Checksum);
                        OutputArray[OutputArray.Length - 1] = ChecksumByte[0];


                        if (serialPort2.IsOpen)
                        {
                            OstrichStatus = 2;
                            serialPort2.Write(OutputArray, 0, OutputArray.Length);
                        }
                        else
                        {
                            MessageBox.Show("Ostrich COM Port not opened");
                            BulkChunkNumber = 0;
                            StartChunkAddress = 0;
                            OstrichStatus = 0;
                            MSB = (byte)(112);
                        }
                    }
                    else
                    {
                        if (!quietBulkWrite) MessageBox.Show("BIN Uploaded to Ostrich without error.");
                        quietBulkWrite = false;
                        BulkChunkNumber = 0;
                        StartChunkAddress = 0;
                        OstrichStatus = 0;
                        MSB = (byte)(112);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid BIN Size. Bin must be 32768 bytes.");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message + "A proper size BIN must be opened first");
            }
        }

        private void ContinueBulkWrite(object sender, EventArgs e)
        {
            StartChunkAddress = StartChunkAddress + 4096;
            OstrichWriteBulk(BinWithPatchesAndDebuggerCode);
        }
        
        //Slow write to ostrich if bulk write isn't working (never happens)
        private void OstrichSlowWriteEntireRom(byte[] SourceRomArray)
        {
            int WriteStartAddress = 0;
            byte ByteBlockSize = 255;
            StopWriteDueToError = false;
            while (WriteStartAddress < 32513 && !StopWriteDueToError)
            {
                OstrichWriteSpecific_256Max(SourceRomArray, WriteStartAddress, WriteStartAddress, ByteBlockSize, false);
                serialPort2.ReadTimeout = 250;
                int OstrichResponse = serialPort2.ReadByte();
                if (OstrichResponse != 79)
                {
                    MessageBox.Show("Bad response from Ostrich after write request");
                    cboxAutoStep.Checked = false;
                    StopWriteDueToError = true;
                }
                WriteStartAddress = WriteStartAddress + 256;
            }
            MessageBox.Show("Slow write completed.");

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
        private void ResetOstrichVID()
        {
            byte VendorIDNumber = Convert.ToByte(nudVIDNumber.Value);
            byte[] OstrichUpdateSerialAndVidCommand = new byte[11];
            OstrichUpdateSerialAndVidCommand[0] = 78;//4e
            OstrichUpdateSerialAndVidCommand[1] = VendorIDNumber;
            OstrichUpdateSerialAndVidCommand[10] = 0;
            Array.Copy(OstrichSerialNumber, 1, OstrichUpdateSerialAndVidCommand, 2, OstrichSerialNumber.Length - 1);
            int Checksum = 0;
            foreach (byte element in OstrichUpdateSerialAndVidCommand)
            {
                Checksum = Checksum + element;
            }
            byte[] ChecksumByte = BitConverter.GetBytes(Checksum);
            OstrichUpdateSerialAndVidCommand[10] = ChecksumByte[0];
            if (serialPort2.IsOpen)
            {
                serialPort2.Write(OstrichUpdateSerialAndVidCommand, 0, OstrichUpdateSerialAndVidCommand.Length);
                    try
                    {
                        serialPort2.ReadTimeout = 3000;
                        int OstrichResponse = serialPort2.ReadByte();
                        if (OstrichResponse == 79)
                        {
                        MessageBox.Show("Ostrich Vendor ID reset to " + VendorIDNumber);
                        byte[] OstrichRequestSerial = new byte[3] { 78, 83, 161 };
                        serialPort2.Write(OstrichRequestSerial, 0, 3);
                        OstrichStatus = 1;
                    }
                    else
                    {
                        MessageBox.Show("Bad response from Ostrich after write request");
                        cboxAutoStep.Checked = false;
                        StopWriteDueToError = true;
                    }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message + "\r\n" + "If this continues to happen, you may need to unplug and replug the Ostrich and re-open the port");
                        cboxAutoStep.Checked = false;
                        StopWriteDueToError = true;
                    }
             }
        }

        private void OstrichWriteSpecific_256Max(byte[] SourceArray, int EMUStartAddress, int SourceArrayStartAddress = 0, int BytesToWrite = 0, bool WaitForResponse = true)
        {

            if (!serialPort2.IsOpen)
            {
                MessageBox.Show("Open Ostrich COM port first.");
                StopWriteDueToError = true;
                return;
            }
            OstrichSentError = false;
            
            byte[] OutputArray = new byte[261];
            prepareOstrichWritePacket(SourceArray, ref OutputArray, EMUStartAddress, SourceArrayStartAddress, BytesToWrite);

            //Perform write
            serialPort2.Write(OutputArray, 0, OutputArray.Length);

            //Return if not waiting for response from EMU
            if (!WaitForResponse)
            {
                return;
            }
            
            //Wait for EMU response
            int ReadAttempts = 0;
            try
            {
                serialPort2.ReadTimeout = 250;
                int OstrichResponse = 0;
                //Read from port until a response is received or too many timeouts
                while (OstrichResponse == 0 || OstrichResponse == -1)
                {
                    OstrichResponse = serialPort2.ReadByte();
                }

                //Recevid a byte other than 'O'
                if (OstrichResponse != 79)
                {
                    MessageBox.Show("Bad response from Ostrich after write request");
                    cboxAutoStep.Checked = false;
                    StopWriteDueToError = true;
                }
            }
            catch (Exception error)
            {
                //Too many timeouts
                ReadAttempts++;
                if (ReadAttempts > 50)
                {
                    MessageBox.Show(error.Message + "\r\n" + "If this continues to happen, you may need to unplug and replug the Ostrich and re-open the port");
                    StopWriteDueToError = true;
                }

            }

        }

        private void prepareOstrichWritePacket(byte[] SourceArray, ref byte[] OutputArray, int EMUStartAddress, int SourceArrayStartAddress = 0, int BytesToWrite = 0)
        {
            //If 0 byte count , then send entire source array (256 byte max)
            if (BytesToWrite == 0)
            {
                BytesToWrite = Convert.ToInt32(SourceArray.Length);
                if(BytesToWrite > 256)
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
                
        
        
        //---------------- code files and ROM Debugger Code -----------------------
        
        //Prepare the debugger caller code based on where it will be stored in the ROM
        private void CreateRomCallArraysBasedOnRomAddress(int ROMAddress)
        {
            if (usingVCALreRoute)
            {
                byteCodeStubCAll = new byte[1];
                byteCodeStubCAll[0] = (byte)(VCAL_0_OPCODE + VCALNumber);
                return;
            }
            byteCodeStubCAll = new byte[] { 50, 0, 117, 3 };
            int HiByte = ROMAddress >> 8;
            int LoByte = ROMAddress & 255;
            byteCodeStubCAll[1] = (byte)LoByte;
            byteCodeStubCAll[2] = (byte)HiByte;
            byteAltCodeStubCall[2] = (byte)LoByte;
            byteAltCodeStubCall[3] = (byte)HiByte;
            byteAltCodeStubCall[6] = (byte)LoByte;
            byteAltCodeStubCall[7] = (byte)HiByte;
        }
               
        //Get settings from .code file and VCAL settings from rom chosen by drop down menu and store in array
        //Return true if succesful
        private bool getDebuggerSettingsForROM()
        {
            codeFileHasBeenOpened = false; 
            byte[] tempCodeFileArray;
            ROMSettingsAndDebuggerCodePath = Directory.GetCurrentDirectory();
            switch (cmboxRomType.Text)
            {
                case "P12-P13-P14":
                    ROMSettingsAndDebuggerCodePath += @"\P13DebuggerCode.code";
                    tempCodeFileArray = Properties.Resources.P13DebuggerCode;
                    if(usingVCALreRoute) tempCodeFileArray = Properties.Resources.P13DebuggerCode_VCAL;
                    break;
                case "P13 Based HTS":
                    ROMSettingsAndDebuggerCodePath += @"\P13HTSDebuggerCode.code";
                    tempCodeFileArray = Properties.Resources.P13HTSDebuggerCode;
                    if(usingVCALreRoute) tempCodeFileArray = Properties.Resources.P13HTSDebuggerCode_VCAL;
                    break;
                case "ectune / HTS":
                    ROMSettingsAndDebuggerCodePath += @"\HTSDebuggerCode.code";
                    tempCodeFileArray = Properties.Resources.HTSDebuggerCode;
                    if (usingVCALreRoute) tempCodeFileArray = Properties.Resources.HTSDebuggerCode_VCAL;
                    break;
                case "P72":
                    ROMSettingsAndDebuggerCodePath += @"\P72DebuggerCode.code";
                    tempCodeFileArray = Properties.Resources.P72DebuggerCode;
                    if (usingVCALreRoute) tempCodeFileArray = Properties.Resources.P72DebuggerCode_VCAL;
                    break;
                case "P30":
                    ROMSettingsAndDebuggerCodePath += @"\P30DebuggerCode.code";
                    tempCodeFileArray = Properties.Resources.P30DebuggerCode;
                    if (usingVCALreRoute) tempCodeFileArray = Properties.Resources.P30DebuggerCode_VCAL;
                    break;
                case "Custom ROM":
                    ROMSettingsAndDebuggerCodePath += @"\CustomDebuggerCode.code";
                    tempCodeFileArray = Properties.Resources.CustomDebuggerCode;
                    if (usingVCALreRoute) tempCodeFileArray = Properties.Resources.P13HTSDebuggerCode_VCAL;
                    break;
                default:
                    MessageBox.Show("Check that you have selected a valid ECU Type");
                    return codeFileHasBeenOpened;

            }
            if(usingVCALreRoute)
            {
                ROMSettingsAndDebuggerCodePath = ROMSettingsAndDebuggerCodePath.Insert(ROMSettingsAndDebuggerCodePath.IndexOf('.') , "_VCAL");
            }
           
            try
            {
                
                if (checkForCodeFile(ROMSettingsAndDebuggerCodePath))
                {
                    tempCodeFileArray = System.IO.File.ReadAllBytes(ROMSettingsAndDebuggerCodePath);
                }
                if (ASMOpened) getVCALroutingInfoFromROM();
                codeFileHasBeenOpened = true;
                GetInfoFromCodeFile(tempCodeFileArray);
                if (usingVCALreRoute) 
                {
                    addVCALcodeToDebuggerCode();
                    CreateRomCallArraysBasedOnRomAddress(Convert.ToInt32(nudROMCodeAddress.Value));
                    
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message + "\r\n There is a problem with " + ROMSettingsAndDebuggerCodePath);
                codeFileHasBeenOpened = false;
            }
            return codeFileHasBeenOpened;

        }


        private bool checkForCodeFile(string path)
        {
            //Check directory for code file, if found return true
            //Unless checking for CustomDebuggerCode.code, then extract and show popup
            bool codeFileExists = false;
            if(File.Exists(path))
            {
                codeFileExists = true;
            }
            if(!codeFileExists && (Path.GetFileName(path) == "CustomDebuggerCode.code" || Path.GetFileName(path) == "CustomDebuggerCode_VCAL.code"))
            {
                extractCustomCodeFileShowPopup();
            }
            return codeFileExists; // True to file from folder
        }
        private void extractCustomCodeFileShowPopup()
        {
            try
            {
               
                //Extract CustomDebuggerCode.code
                byte[] tempCodeFile = Properties.Resources.CustomDebuggerCode;
                string newCodeFilePath = System.IO.Directory.GetCurrentDirectory() + @"\CustomDebuggerCode.code";
                using (System.IO.FileStream newCodeFile = new System.IO.FileStream(newCodeFilePath, System.IO.FileMode.CreateNew))
                {
                    newCodeFile.Write(tempCodeFile, 0, tempCodeFile.Length);
                }


                //extract CustomDebuggerCode_VCAL.code
                tempCodeFile = Properties.Resources.CustomDebuggerCode_VCAL;
                newCodeFilePath = System.IO.Directory.GetCurrentDirectory() + @"\CustomDebuggerCode_VCAL.code";
                using (System.IO.FileStream newCodeFile = new System.IO.FileStream(newCodeFilePath, System.IO.FileMode.CreateNew))
                {
                    newCodeFile.Write(tempCodeFile, 0, tempCodeFile.Length);
                }




                //Extract Debugger_ROM_Code.asm
                string tempASM = Properties.Resources.Debugger_ROM_Code;
                string newASMFilePath = System.IO.Directory.GetCurrentDirectory() + @"\Debugger_ROM_Code.asm";
                SaveDataToTxtFile(newASMFilePath, tempASM, false);

                //Extract Debugger_ROM_Code_VCAL.asm
                tempASM = Properties.Resources.Debugger_ROM_Code_VCAL;
                newASMFilePath = System.IO.Directory.GetCurrentDirectory() + @"\Debugger_ROM_Code_VCAL.asm";
                SaveDataToTxtFile(newASMFilePath, tempASM, false);

                MessageBox.Show("The files: \r\n\r\n" +
                                "CustomDebuggerCode.code,\r\n" +
                                "CustomDebuggerCode_VCAL.code,\r\n" +
                                "Debugger_ROM_Code.asm \r\n" +
                                "Debugger_ROM_Code_VCAL.asm \r\n\r\n" +
                                "have been extracted to the current directory.\r\n" +
                                "Modify the .code file to use the debugger with any unsupported ECU running OKI 66k asm from a 32KB ROM.\r\n" +
                                "See Debugger_ROM_Code.asm for details.", "Custom ECU Code");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Extraction Error");
            }
        }
        //Read Code file and assign variables data  
        private void GetInfoFromCodeFile(byte[] codeFileArray)
        {
            int intVectorTblOffset = 0;
            int whiteListSize = 0;
            int cf_PatchBytesLo = 0;
            int cf_PatchBytesHi = 1;
            int cf_CodeStubStartLo = 2;
            int cf_CodeStubStartHi = 3;
            int cf_DebuggerAddresslo = 4;
            int cf_DebuggerAddressHi = 5;
            int cf_Baud1Lo = 6;
            int cf_Baud1Hi = 7;
            int cf_Baud2Lo = 8;
            int cf_Baud2Hi = 9;
            int cf_BaudValue = 10;
            int cf_whichLoopLo = 11;
            int cf_whichLoopHi = 12;
            int cf_LRBoffsetLo = 13;
            int cf_LRBoffsetHi = 14;
            int cf_PSWOffsetLo = 15;
            int cf_PSWOffsetHi = 16;
            int cf_IEoffsetLo = 17;
            int cf_IEoffsetHi = 18;
            int cf_XtraRam1Lo = 19;
            int cf_XtraRam1Hi = 20;
            int cf_XtraRam2Lo = 21;
            int cf_XtraRam2Hi = 22;
            int cf_XtraRam3Lo = 23;
            int cf_XtraRam3Hi = 24;
            int cf_XtraRam4Lo = 25;
            int cf_XtraRam4Hi = 26;
            int cf_XtraRam5Lo = 27;
            int cf_XtraRam5Hi = 28;
            int cf_XtraRam6Lo = 29;
            int cf_XtraRam6Hi = 30;
            int cf_OrigVcalLocLo = 31;
            int cf_OrigVcalLocHi = 32;
            int cf_whiteListLocLo = 33;
            int cf_whiteListLocHi = 34;
            
            //If the .code file is a full 32kb , cut all the 0xff off the tail of it
            //Check for vector table and offset the .code pointers to read after it
            //Useful to use a fresh asm code file without having to trim it manually
            if (codeFileArray.Length == 32768)
            {
                int endAddress = 0;
                for (int i = 32768 -1 ; i > 0 ; i--)
                {
                    if(codeFileArray[i] != 0xff)
                    {
                        endAddress = i;
                        break;
                    }
                }
                int intFFcount = 0;
                for(int i = 0; i < 0x39; i++)
                {
                    if(codeFileArray[i] == 0xff)
                    {
                        intFFcount++;
                    }
                }
                if(intFFcount == 0x36)
                {
                    intVectorTblOffset = 0x38;
                }
                //If using vcal reroute make the array big enough to include whiteList
                if (usingVCALreRoute)
                {
                    whiteListSize = (VCALWhiteList.Count - 1) * 2;
                    ROMSettingsAndDebuggerCodeArray = new byte[(endAddress + 1) + whiteListSize];
                }
                else 
                { 
                    ROMSettingsAndDebuggerCodeArray = new byte[endAddress + 1];
                }

                Array.Copy(codeFileArray, ROMSettingsAndDebuggerCodeArray, ROMSettingsAndDebuggerCodeArray.Length);

            }

            //Default location to place debugger code stub in ROM
            intCodeStubAddress = (ROMSettingsAndDebuggerCodeArray[cf_DebuggerAddressHi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_DebuggerAddresslo + intVectorTblOffset];
            nudROMCodeAddress.Value = intCodeStubAddress;
            //Code file location where the debugger code begins
            intDebuggerCodeStubSourceOffset = (ROMSettingsAndDebuggerCodeArray[cf_CodeStubStartHi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_CodeStubStartLo + intVectorTblOffset];
            //Code file location for ECU specific patch bytes
            intPatchBytesSourceOffset = (ROMSettingsAndDebuggerCodeArray[cf_PatchBytesHi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_PatchBytesLo + intVectorTblOffset];
            //ROM addresses to patch for ECU serial baud rate and the value to patch
            BaudRateAddress1 = (ROMSettingsAndDebuggerCodeArray[cf_Baud1Hi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_Baud1Lo + intVectorTblOffset];
            BaudRateAddress2 = (ROMSettingsAndDebuggerCodeArray[cf_Baud2Hi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_Baud2Lo + intVectorTblOffset];
            BaudRatePatchValue = ROMSettingsAndDebuggerCodeArray[cf_BaudValue + intVectorTblOffset];
            //Code file location for the "monitored" byte for breaking the waiting loop
            ROMOffsetWhichLoop = (ROMSettingsAndDebuggerCodeArray[cf_whichLoopHi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_whichLoopLo + intVectorTblOffset];
            //Code file location for LRB, PSW, IE settings
            DebuggerLRBOffsetFromRom = (ROMSettingsAndDebuggerCodeArray[cf_LRBoffsetHi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_LRBoffsetLo + intVectorTblOffset];
            DebuggerPSWOffsetFromRom = (ROMSettingsAndDebuggerCodeArray[cf_PSWOffsetHi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_PSWOffsetLo + intVectorTblOffset];
            DebuggerIEOffsetFromRom = (ROMSettingsAndDebuggerCodeArray[cf_IEoffsetHi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_IEoffsetLo + intVectorTblOffset];
            //Code file locations for extra ram addresses
            intExtraRam1Offset = (ROMSettingsAndDebuggerCodeArray[cf_XtraRam1Hi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_XtraRam1Lo + intVectorTblOffset];
            intExtraRam2Offset = (ROMSettingsAndDebuggerCodeArray[cf_XtraRam2Hi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_XtraRam2Lo + intVectorTblOffset];
            intExtraRam3Offset = (ROMSettingsAndDebuggerCodeArray[cf_XtraRam3Hi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_XtraRam3Lo + intVectorTblOffset];
            intExtraRam4Offset = (ROMSettingsAndDebuggerCodeArray[cf_XtraRam4Hi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_XtraRam4Lo + intVectorTblOffset];
            intExtraRam5Offset = (ROMSettingsAndDebuggerCodeArray[cf_XtraRam5Hi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_XtraRam5Lo + intVectorTblOffset];
            intExtraRam6Offset = (ROMSettingsAndDebuggerCodeArray[cf_XtraRam6Hi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_XtraRam6Lo + intVectorTblOffset];
            lblDebuggerCodeSize.Text = ((ROMSettingsAndDebuggerCodeArray.Length - intDebuggerCodeStubSourceOffset) - whiteListSize) + " Bytes";
            //VCAL re routing
            romOffsetRealVcalSub = (ROMSettingsAndDebuggerCodeArray[cf_OrigVcalLocHi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_OrigVcalLocLo + intVectorTblOffset];
            romOffsetWhiteList = (ROMSettingsAndDebuggerCodeArray[cf_whiteListLocHi + intVectorTblOffset] << 8) + ROMSettingsAndDebuggerCodeArray[cf_whiteListLocLo + intVectorTblOffset];

        }
        
        //Add byte patches to the ROM being debugged based on the patches in the .code file
        private void AddConfigPatchesToRom()
        {
            int patchOffset =intPatchBytesSourceOffset;
            while (patchOffset < intDebuggerCodeStubSourceOffset)
            {
                byte PatchAddressHiByte = ROMSettingsAndDebuggerCodeArray[patchOffset + 1];
                byte PatchAddressLoByte = ROMSettingsAndDebuggerCodeArray[patchOffset];
                int PatchByte = patchOffset + 2;
                int PatchAddress = (PatchAddressHiByte << 8) + PatchAddressLoByte;
                Array.Copy(ROMSettingsAndDebuggerCodeArray, PatchByte, BinWithPatchesAndDebuggerCode, PatchAddress,1);
                patchOffset = patchOffset + 3;
            }
        }
        
        //Add LRB PSW and IE values from the nud boxes if applicable       
        private void AddPSW_LRB_IE_nudValuesToROMCode()
        {
            if (cboxDebuggerLRB.Checked)
            {
                int LRBValue = Convert.ToInt32(nudDebuggerLRB.Value);
                byte HiByte = (byte)(LRBValue >> 8);
                byte LoByte = (byte)(LRBValue & 255);
                ROMSettingsAndDebuggerCodeArray[DebuggerLRBOffsetFromRom] = LoByte;
                ROMSettingsAndDebuggerCodeArray[DebuggerLRBOffsetFromRom + 1] = HiByte;
                LRBDebuggerValue = LRBValue;
            }
            if (cboxDebuggerPSW.Checked)
            {
                int PSWValue = Convert.ToInt32(nudDebuggerPSW.Value);
                // byte HiByte = (byte)(PSWValue >> 8);
                // byte LoByte = (byte)(PSWValue & 255);
                // ROMSettingsAndDebuggerCodeArray[DebuggerPSWOffsetFromRom] = LoByte;
                //ROMSettingsAndDebuggerCodeArray[DebuggerPSWOffsetFromRom + 1] = HiByte;
                ROMSettingsAndDebuggerCodeArray[DebuggerPSWOffsetFromRom] = (byte)(PSWValue & 0xff);
                PSWDebuggerValue = PSWValue;
            }
            if (cboxDebuggerIE.Checked)
            {
                int IEValue = Convert.ToInt32(nudDebuggerIE.Value);
                byte HiByte = (byte)(IEValue >> 8);
                byte LoByte = (byte)(IEValue & 255);
                ROMSettingsAndDebuggerCodeArray[DebuggerIEOffsetFromRom] = LoByte;
                ROMSettingsAndDebuggerCodeArray[DebuggerIEOffsetFromRom + 1] = HiByte;
            }
        }
        
        //Update the value in the LRB PSW IE nud boxes to reflect the current settings being used
        private void UpdateLRB_PSW_IE_Nuds()
        {
            LRBDebuggerValue = (ROMSettingsAndDebuggerCodeArray[DebuggerLRBOffsetFromRom + 1] << 8) + (ROMSettingsAndDebuggerCodeArray[DebuggerLRBOffsetFromRom]);
            nudDebuggerLRB.Value = LRBDebuggerValue;

            //PSWDebuggerValue = (ROMSettingsAndDebuggerCodeArray[DebuggerPSWOffsetFromRom + 1] << 8) + (ROMSettingsAndDebuggerCodeArray[DebuggerPSWOffsetFromRom]);
            PSWDebuggerValue = ROMSettingsAndDebuggerCodeArray[DebuggerPSWOffsetFromRom];
            nudDebuggerPSW.Value = PSWDebuggerValue;

            int IEDebuggervalue = (ROMSettingsAndDebuggerCodeArray[DebuggerIEOffsetFromRom + 1] << 8) + (ROMSettingsAndDebuggerCodeArray[DebuggerIEOffsetFromRom]);
            nudDebuggerIE.Value = IEDebuggervalue;

        }
   
        //Add debugger code stub to BIN file, apply nud box LRB PSW and IE patches if applicable
        private void AddDebuggerCodeToRom()
        {
            if (ROMSettingsAndDebuggerCodeArray != null)
            {
                try
                {
                    AddPSW_LRB_IE_nudValuesToROMCode();
                    Array.Copy(ROMSettingsAndDebuggerCodeArray, intDebuggerCodeStubSourceOffset,
                    BinWithPatchesAndDebuggerCode, Convert.ToInt32(nudROMCodeAddress.Value),
                    ROMSettingsAndDebuggerCodeArray.Length - intDebuggerCodeStubSourceOffset);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message + "\r\n \r\n Be sure the BIN selected is a 32kb Honda ECU BIN, and that the .code file was created properly.");
                }
            }
        }
        //-------------------------------------------------------------------------


        //Set breakpoint pressed, parse instruction line, highlight ASm window, write breakpoint to emulator, reset packet counter
        private void btnSetBreakpoint_Click(object sender, EventArgs e)
        {
            //OstrichRestoreArrayAndRestoreDistantROMJumps();
            intCurrent_BP = Convert.ToInt32(nudBPAddress.Value);
            getIndex_ValidateAddress(ref CurrentASMIndex, ref intCurrent_BP);
            HighlightLineInASMTextBox(CurrentASMIndex);
            OstrichSetBreakpoint(intCurrent_BP);
            intPacketsRecd = 0;
        }
        //Remove breakpoint pressed - restore array and jumps, uncheck autostep and lock BP
        private void btnRemoveBreakPoint_Click(object sender, EventArgs e)
        {
            OstrichRestoreArrayAndRestoreDistantROMJumps();
            cboxAutoStep.Checked = false;
            cboxLockBP.Checked = false;
            stepDebugger();
        }

        //Step button pressed - break waiting loop, and start autostep timer if necessary
        private void btnStepFwd_Click(object sender, EventArgs e)
        {
            stepDebugger();
        }

        private void stepDebugger()
        {

            strPlain_BP = "";
            intCurrent_BP = 0;

            if (cboxLockBP.Checked)
            {
                if (cboxAutoStep.Checked)
                {
                    timer1.Enabled = true;
                }
                OstrichRestoreArrayAndRestoreDistantROMJumps();
                BreakLoopUsingRomAddress();
                Thread.Sleep(50);
                OstrichSetBreakpoint(Convert.ToInt32(nudBPAddress.Value), false);
             

            }
            else
            {
                BreakLoopUsingRomAddress();
            }
        }
       
        //Program exit. Close Com ports
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort2.IsOpen)
            {
                serialPort2.Close();
                progressBar2.Value = 0;
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
            }
        }
        
        //Open bin file and upload to emulator if auto load is clicked 
        private void btnLoadBinFile_Click(object sender, EventArgs e)
        {
            OPENBINDialog.Filter = "BIN Files | *.bin";
            if (OPENBINDialog.ShowDialog() == DialogResult.OK)
            {
                lblChangedBytes.Text = "";
                strBINPath = OPENBINDialog.FileName;
                usingASMFile = false;
                CreateBINArraysDecompileBINandUpdateASMWindow();
                BINOpened = true;
                if (cboxAutoUploadOnBINLoad.Checked)
                {
                    AddDebuggerCodeAndPatchesToRomAndUpload();
                }
                    
            }
        }

        //This is what writes the new breakpoints to the ECU, triggered by a new BP address being sent from the ECU
        private void WriteTheNextBreakpoint()
        {
            //Copy the lookahead variables, becuase we are about to change them to the next instruction
            copyLookAheadVariables();
            
            //Find the BP in the ASM file, parse the opcode and next potential instruction or branch address, re-dasm if an indirect jump is used and highlight the line in ASM
            CurrentASMIndex = IndexOfInstructionAddressInArray(ASMFileArray, intCurrent_BP);

            ParseInstructionLineAndOpCodes(intCurrent_BP, CurrentASMIndex, ref intNextInstructionAddress, ref intBranchAddress, ref intCallAddress, ref PointerContentsHexValue, ref strOpCode);
            ReDasmBinFUpdateIndirectJumps(boolReDasm);
            HighlightLineInASMTextBox(CurrentASMIndex);

            lookAheadRamHandling();


            //Write the appropriate breakpoints to the emulator
            //OstrichRestoreArrayAndRestoreDistantROMJumps(false);
            if(!cboxLockBP.Checked)
            {
                OrganizeAndWriteJumpsToEMU();
            }

        }

        void ReDasmBinFUpdateIndirectJumps(bool confirm)
        {
            //Re dasm the bin file to include the indirect jump location that was probably overlooked on initial dasm
            if (confirm)
            {
                DasmIndirectJumps += " " + intNextInstructionAddress.ToString("X4");
                ReDasmBiN();

            }
        }

        void ReDasmBiN ()
        {
            decompileBIN(strBINPath,CurrentASMPath,false);
            highlightAfterValidating(Convert.ToInt32(nudBPAddress.Value));
            getVCALroutingInfoFromROM();
        }
        //highlight an address in the asm window 
        //If address is invalid the next lowest address will be highlighted
        //If 0 the current BP will be highlighted, if current bp = 0 the BP nud value will be used
        private void highlightAfterValidating(int address)
        {
            if(address ==0)
            {
                if (intCurrent_BP != 0) address = intCurrent_BP;
                else address = Convert.ToInt32(nudBPAddress.Value);
            }
            rtboxASMFile.Focus();
            int idx = 0;
            getIndex_ValidateAddress(ref idx, ref address);
            SelectLineInRichTextBox(rtboxASMFile, IndexOfInstructionAddressInArray(ASMFileArray, address));
        }
        private string getDasmArgs()
        {
            strIgnoreDasmArgs = "";
            strForceDasmArgs = "";
            if(tboxDasmArgIgnore.Text != "")
            {
                strIgnoreDasmArgs = " " + tboxDasmArgIgnore.Text;
                strIgnoreDasmArgs = strIgnoreDasmArgs.Replace(" ", " D");
            }
            if(tboxDasmArgForce.Text != "")
            {
                strForceDasmArgs = " " + tboxDasmArgForce.Text;
            }


            string dasmArgs = DasmDefaultArea;
            dasmArgs += strIgnoreDasmArgs;
            dasmArgs += strForceDasmArgs;

            dasmArgs += DasmIndirectJumps;
            return dasmArgs;
        }
       
        //---------------- Extra RAM and pointers ---------------------------------
        //Parse any possible ram addresses from a given string. Return true if a dot is found
        bool lookAheadForRamValues(int NextInstruction, int nextIndex, ref int Ram1, ref int Ram2, ref string opCode, ref bool wordValue)
        {
            wordValue = false;
            Ram1 = 0;
            Ram2 = 0;
            
            string instructionLine = ASMFileArray[nextIndex].Substring(16,ASMFileArray[nextIndex].IndexOf(";") - 16);
            
            //Replace all SFR labels to hex value
            instructionLine = replaceSFRLabelsWithHexAddress(instructionLine);
            
            //If [USP] is found remove from "-" to "[USP]" and replace with a ")"
            if(instructionLine.Contains("[USP]"))
            {
                instructionLine = instructionLine.Remove(instructionLine.IndexOf("-") + 1, (instructionLine.IndexOf("[USP]") + 4) - instructionLine.IndexOf("-"));
                instructionLine = instructionLine.Replace("-", ")");
            }

            int firstHindex = instructionLine.IndexOf("h");
            int lastHindex = instructionLine.LastIndexOf("h");
            int intFirstbracketIndex = instructionLine.IndexOf("[");
            int intLastbracketIndex = instructionLine.LastIndexOf("[");
            int intPoundIndex = instructionLine.IndexOf("#");
            bool dotIndex = instructionLine.Contains(".");
            
            
            int intFirstBracketPointer = 0;
            int intLastBracketPointer = 0;
            string strFirstHAddress;
            int intFirstHAddress = 0;
            string strLastHAddress;
            int intLastHAddress = 0;

            //If pointer offsets are used ( [X1] [X2] [ACC] [DP] ) get the pointer value
            if(intFirstbracketIndex > 0)
            {
                string strFirstBracketReg = instructionLine.Substring(intFirstbracketIndex, 5) ;
                strFirstBracketReg = strFirstBracketReg.Split('[', ']')[1];
                intFirstBracketPointer = GetPointerFromReg(strFirstBracketReg);
                if (useModifiedX1 && strFirstBracketReg == "X1")
                {
                    intFirstBracketPointer = intModifiedX1;
                }
                if (useModifiedX2 && strFirstBracketReg == "X2")
                {
                    intFirstBracketPointer = intModifiedX2;
                }
            }

            //If a second pointer offset is used ( [X1] [X2] [ACC] [DP] ) get the pointer value
            if (intLastbracketIndex > intFirstbracketIndex)
            {
                string strLastBracketReg = instructionLine.Substring(intLastbracketIndex, 5);
                strLastBracketReg = strLastBracketReg.Split('[', ']')[1];
                intLastBracketPointer = GetPointerFromReg(strLastBracketReg);
                if (useModifiedX1 && strLastBracketReg == "X1")
                {
                    intFirstBracketPointer = intModifiedX1;
                }
                if (useModifiedX2 && strLastBracketReg == "X2")
                {
                    intFirstBracketPointer = intModifiedX2;
                }
            }

            //Found an 0xh adress add in an offset pointer if needed and parse the value, ignore if it's preceeded by a #
            if (firstHindex > 0 && intPoundIndex == -1 || firstHindex > 0 && intPoundIndex > firstHindex )
            {
                strFirstHAddress = instructionLine.Substring(firstHindex - 4, 4);
                bool success = int.TryParse(strFirstHAddress, System.Globalization.NumberStyles.HexNumber, null, out intFirstHAddress);

                if (!success)
                {
                    string tester = strFirstHAddress.Substring(2, 2);
                    int.TryParse(strFirstHAddress.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out intFirstHAddress);

                }
                //Add pointer offset
                intFirstHAddress += intFirstBracketPointer;
                Ram1 = intFirstHAddress;
            }

            //Found another 0xh adress add in an offset pointer if needed and parse the value
            if (lastHindex > firstHindex && (lastHindex - intPoundIndex) > 6)
            {
                strLastHAddress = instructionLine.Substring(lastHindex - 4, 4);
                bool success = int.TryParse(strLastHAddress, System.Globalization.NumberStyles.HexNumber, null, out intLastHAddress);

                if (!success)
                {
                    int.TryParse(strLastHAddress.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out intLastHAddress);
                }
                //Add pointer offset
                intLastHAddress += intLastBracketPointer;
                Ram2 = intLastHAddress;
            }
            //Handle word RAM requests
            opCode = instructionLine.Substring(0, 5).Trim();
            if (opCode == "L" || opCode == "ST" || opCode == "MOV" || opCode == "AND" || opCode == "XOR" || opCode == "CMP" || opCode == "XCHG" || opCode == "ADD" || opCode == "CMP")
            {
                wordValue = true;
            }
            
            //Avoid ROM lookups
            if(opCode =="LCB" || opCode == "LC" || opCode == "CMPC" || opCode == "CMPCB" )
            {
                Ram1 = 0;
                Ram2 = 0;
            }
            return dotIndex;

        }
        
        //Get register value for pointer from register indicated by string (DP ACC X1 X2)
        private int GetPointerFromReg(string register)
        {
            int PointerRegisterAddress = 0;
            switch (register)
            {
                case "X1":
                    if (strPlain_X1 != "")
                    {
                        PointerRegisterAddress = Convert.ToInt32(strPlain_X1, 16);
                    }
                    break;
                case "X2":
                    if (strPlain_X2 != "")
                    {
                        PointerRegisterAddress = Convert.ToInt32(strPlain_X2, 16);
                    }
                    break;
                case "DP":
                    if (strPlain_DP != "")
                    {
                        PointerRegisterAddress = Convert.ToInt32(strPlain_DP, 16);
                    }
                    break;
                case "ACC":
                    if (strPlain_ACC != "")
                    {
                        PointerRegisterAddress = Convert.ToInt32(strPlain_ACC, 16);
                    }
                    break;
                default:
                    PointerRegisterAddress = 0;
                    break;
            }
            return PointerRegisterAddress;
        }

        //Replace SFR labels with hex address
        private string replaceSFRLabelsWithHexAddress(string input)
        {
            //The search is repeated 4 times with the characters most likely to follow the address appended. This keeps P0 from replacing part of P0IO
            int SFRAddress = 0;
            string strSFRAddress = "";
            
            foreach(string Name in SFRLabels)
            {
                if(input.Contains(Name))
                {
                    strSFRAddress = SFRAddress.ToString("X2").PadLeft(3, '0').PadRight(4, 'h').ToLower();
                    input = input.Replace(" " + Name + " ", " " + strSFRAddress + " ");
                    input = input.Replace(" " + Name + ".", " " + strSFRAddress + ".");
                    input = input.Replace(" " + Name + ",", " " + strSFRAddress + ",");
                    input = input.Replace("(" + Name + ")", "(" + strSFRAddress + ")");
                }
                SFRAddress++;
            }
            return input;
        }
        void displayExtraRamData()
        {
            string strLA11value = intXramLA11Value.ToString("X2");
            string strLA12value = intXramLA12Value.ToString("X2");
            string strLA21value = intXramLA21Value.ToString("X2");
            string strLA22value = intXramLA22Value.ToString("X2");


            string strword1value;
            string strword2value;

            int intword1Value;
            int intword2Value;
            int intRamWatchAddress = 0;

            lblLA11.Text = "";
            lblLA12.Text = "";
            tboxLA1_1.Text = "";
            tboxLA1_2.Text = "";
            strTrace_Xram1Addy = "Xram1";
            strTrace_Xram2Addy = "Xram2";
            strTrace_Xram1Conts = "    ";
            strTrace_Xram2Conts = "    ";
            lblXram1Binary.Text = "";
            lblXram2Binary.Text = "";



            if (!instructionBranched)
            {
                if (ExtraRam1incoming)
                {

                    lblLA11.Text = intIncomingRAMAddress1.ToString("X4") + "h";
                    intRamWatchAddress = intIncomingRAMAddress1;
                    strTrace_Xram1Addy = lblLA11.Text;
                    if (boolIncomingWordLA1)
                    {
                        intword1Value = (intXramLA12Value << 8) + intXramLA11Value;
                        strword1value = intword1Value.ToString("X4");
                        tboxLA1_1.Text = strword1value;
                        strTrace_Xram1Conts = strword1value.PadLeft(4, '0');
                    }
                    else
                    {
                        tboxLA1_1.Text = strLA11value;
                        strTrace_Xram1Conts = strLA11value.PadLeft(4, '0');
                    }
                }
                if (ExtraRam2incoming && !boolLA1word)
                {
                    lblLA12.Text = intIncomingRAMAddress2.ToString("X4") + "h";
                    tboxLA1_2.Text = strLA12value;
                    strTrace_Xram2Conts = strLA12value.PadLeft(4, '0');
                }
            }



            if (instructionBranched)
            {
                if (ExtraRam3incoming)
                {

                    lblLA11.Text = intIncomingRAMAddress3.ToString("X4") + "h";
                    intRamWatchAddress = intIncomingRAMAddress3;
                    strTrace_Xram1Addy = lblLA11.Text;

                    if (boolIncomingWordLA2)
                    {
                        intword2Value = (intXramLA22Value << 8) + intXramLA21Value;
                        strword2value = intword2Value.ToString("X4");
                        tboxLA1_1.Text = strword2value;
                        strTrace_Xram1Conts = strword2value.PadLeft(4, '0');
                    }
                    else
                    {
                        tboxLA1_1.Text = strLA21value;
                        strTrace_Xram1Conts = strLA21value.PadLeft(4, '0');
                    }
                }
                if (ExtraRam4incoming && !boolLA2word)
                {
                    lblLA11.Text = intIncomingRAMAddress4.ToString("X4") + "h";
                    tboxLA1_2.Text = strLA22value;
                    strTrace_Xram2Conts = strLA22value.PadLeft(4, '0');
                }
            }
            if (tboxLA1_1.Text != "")
            {
                lblXram1Binary.Text = Convert.ToString((Convert.ToInt32(tboxLA1_1.Text, 16) & 0xff) >> 4, 2).PadLeft(4, '0') + " " + Convert.ToString((Convert.ToInt32(tboxLA1_1.Text, 16) & 0xf), 2).PadLeft(4, '0');
            }
            if (tboxLA1_2.Text != "")
            {
                lblXram2Binary.Text = Convert.ToString((Convert.ToInt32(tboxLA1_2.Text, 16) & 0xff) >> 4, 2).PadLeft(4, '0') + " " + Convert.ToString((Convert.ToInt32(tboxLA1_2.Text, 16) & 0xf), 2).PadLeft(4, '0');
            }

            tboxRamWatch.Text = ((intRAMWatchByte2 << 8) + intRAMWatchByte1).ToString("X4");
            lblRamWatchBinary.Text = Convert.ToString((intRAMWatchByte1 & 0xff) >> 4, 2).PadLeft(4, '0') + " " + Convert.ToString((intRAMWatchByte1 & 0xf), 2).PadLeft(4, '0');

            if (!cboxLockRamWatch.Checked && intRamWatchAddress != 0)
            {
                nudRAMWatch.Value = intRamWatchAddress;
            }

        }
        void copyLookAheadVariables()
        {
            strRamWatchAddress = intRamWatchAddress.ToString("X4") + "h";
            lblRamWatch.Text = strRamWatchAddress;
            intRAMWatchDisplayedAddy = intRamWatchAddress;
            //Determine if we are going to show the next instruction RAM or the branch RAM
            instructionBranched = true;
            if (intCurrent_BP == intNextInstructionAddress)
            {
                instructionBranched = false;
            }

            //workaround x1 or x2 being changed on the line before a RAM lookup
            //Since we are always working one instruction behind,(ahead?) The request for the [x1] pointer address will be invalid and too late to change it
            //Only "real fix" would be to parse the lines manually to determine X1's value before the ECu changes it
            string currentInst = ASMFileArray[CurrentASMIndex];

            intIncomingRAMAddress1 = intLA1XRAMAddress1;
            intIncomingRAMAddress2 = intLA1XRAMAddress2;
            intIncomingRAMAddress3 = intLA2XRAMAddress1;
            intIncomingRAMAddress4 = intLA2XRAMAddress2;
            ExtraRam1incoming = boolXRam1Found;
            ExtraRam2incoming = boolXRam2Found;
            ExtraRam3incoming = boolXRam3Found;
            ExtraRam4incoming = boolXRam4Found;
            boolIncomingWordLA1 = boolLA1word;
            boolIncomingWordLA2 = boolLA2word;

        }

        //Check upcoming instructions for RAM addresses.  If found, write them to the ROM debugger code stub so we can retreive their data on the next BP
        void lookAheadRamHandling()
        {
            //Parse the next possible instruction to find any hex addresses for RAm lookup. Set DDflagcorrection for CMP CMPB DD errors
            int LA1index = IndexOfInstructionAddressInArray(ASMFileArray, intNextInstructionAddress);
            dotFoundonNextInstruction = lookAheadForRamValues(intNextInstructionAddress, LA1index, ref intLA1XRAMAddress1, ref intLA1XRAMAddress2, ref strOpCode, ref boolLA1word);

            //Parse possible branch instruction to find any hex addresses for RAm lookup

            int LA2Index;
            int LookUp2 = intBranchAddress;
            if (intBranchAddress != 0)
            {
                LookUp2 = intBranchAddress;
                LA2Index = IndexOfInstructionAddressInArray(ASMFileArray, intBranchAddress);
                dotFoundonBranchInstruction = lookAheadForRamValues(LookUp2, LA2Index, ref intLA2XRAMAddress1, ref intLA2XRAMAddress2, ref strOpCode, ref boolLA2word);
            }

            if (cboxStepInto.Checked && intCallAddress != 0)
            {
                LookUp2 = intCallAddress;
                LA2Index = IndexOfInstructionAddressInArray(ASMFileArray, intCallAddress);
                dotFoundonBranchInstruction = lookAheadForRamValues(LookUp2, LA2Index, ref intLA2XRAMAddress1, ref intLA2XRAMAddress2, ref strOpCode, ref boolLA2word);
            }
            useModifiedX1 = false;
            useModifiedX2 = false;

            writeExtraRamAddressesToEmulator();
        }

        void writeExtraRamAddressesToEmulator()
        {
            boolXRam1Found = false;
            boolXRam2Found = false;
            boolXRam3Found = false;
            boolXRam4Found = false;

            //Get RAM watch value from nud box and make "even" to respect ram boundary
            //Add RAM watch address and next address to get word back using 1 byte responses from ECU
            intRamWatchAddress = Convert.ToInt32(nudRAMWatch.Value) & 0xfffe;
            ROMSettingsAndDebuggerCodeArray[intExtraRam5Offset] = (byte)(intRamWatchAddress & 0xff);
            ROMSettingsAndDebuggerCodeArray[intExtraRam5Offset + 1] = (byte)(intRamWatchAddress >> 8);
            
            ROMSettingsAndDebuggerCodeArray[intExtraRam6Offset] = (byte)((intRamWatchAddress + 1) & 0xff);
            ROMSettingsAndDebuggerCodeArray[intExtraRam6Offset + 1] = (byte)((intRamWatchAddress + 1) >> 8);


            if (intLA1XRAMAddress1 != 0)
            {
                ROMSettingsAndDebuggerCodeArray[intExtraRam1Offset] = (byte)(intLA1XRAMAddress1 & 0xff);
                ROMSettingsAndDebuggerCodeArray[intExtraRam1Offset + 1] = (byte)(intLA1XRAMAddress1 >> 8);
                boolXRam1Found = true;
            }
            if (intLA1XRAMAddress2 != 0)
            {
                ROMSettingsAndDebuggerCodeArray[intExtraRam2Offset] = (byte)(intLA1XRAMAddress2 & 0xff);
                ROMSettingsAndDebuggerCodeArray[intExtraRam2Offset + 1] = (byte)(intLA1XRAMAddress2 >> 8);
                boolXRam2Found = true;
            }
            //If returning word value set address 2 to the high byte
            if (boolLA1word)
            {
                ROMSettingsAndDebuggerCodeArray[intExtraRam2Offset] = (byte)((intLA1XRAMAddress1 + 1) & 0xff);
                ROMSettingsAndDebuggerCodeArray[intExtraRam2Offset + 1] = (byte)((intLA1XRAMAddress1 + 1) >> 8);
            }

            if (intLA2XRAMAddress1 != 0)
            {
                ROMSettingsAndDebuggerCodeArray[intExtraRam3Offset] = (byte)(intLA2XRAMAddress1 & 0xff);
                ROMSettingsAndDebuggerCodeArray[intExtraRam3Offset + 1] = (byte)(intLA2XRAMAddress1 >> 8);
                boolXRam3Found = true;
            }
            if (intLA2XRAMAddress2 != 0)
            {
                ROMSettingsAndDebuggerCodeArray[intExtraRam4Offset] = (byte)(intLA2XRAMAddress2 & 0xff);
                ROMSettingsAndDebuggerCodeArray[intExtraRam4Offset + 1] = (byte)(intLA2XRAMAddress2 >> 8);
                boolXRam4Found = true;
            }
            //If returning word value set address 2 to the high byte
            if (boolLA2word)
            {
                ROMSettingsAndDebuggerCodeArray[intExtraRam4Offset] = (byte)((intLA2XRAMAddress1 + 1) & 0xff);
                ROMSettingsAndDebuggerCodeArray[intExtraRam4Offset + 1] = (byte)((intLA2XRAMAddress1 + 1) >> 8);
            }
            int romstartaddy = intExtraRam1Offset + intCodeStubAddress;
            int sourcestartaddy = intExtraRam1Offset;
            byte copylength = (byte)((intExtraRam6Offset + 2) - intExtraRam1Offset);
            OstrichWriteSpecific_256Max(ROMSettingsAndDebuggerCodeArray, intCodeStubAddress + (intExtraRam1Offset - intDebuggerCodeStubSourceOffset), intExtraRam1Offset, (byte)(intExtraRam6Offset + 2 - intExtraRam1Offset), true);
        }
        //-------------------------------------------------------------------------
        
        
        //Auto-step timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if(PreviousPacketNumber > 0 && PreviousPacketNumber == intPacketsRecd)
            {
                TimerTickCount++;
            }
            else
            {
                TimerTickCount = 0;
            }
            
            if(TimerTickCount >5)
            {
                timer1.Enabled = false;
                cboxAutoStep.Checked = false;
                MessageBox.Show("AutoStep Timeout Error");
             }
            else
            {
                BreakLoopUsingRomAddress();
                if (timer1.Enabled)
                {
                    timer1.Enabled = false;
                    timer1.Enabled = true;
                }
                PreviousPacketNumber = intPacketsRecd;
            }
        }

        private void cmboxRomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDebuggerCodeSize.Text = "0 Bytes";
            getDebuggerSettingsForROM();
        }


        //------------------ Check box changes ------------------------------------
        //Enable manual load asm file button if auto dasm is disabled

        //Enable nud if checked
        private void cboxDebuggerLRB_CheckedChanged(object sender, EventArgs e)
        {
            if (!cboxDebuggerLRB.Checked)
            {
                nudDebuggerLRB.Enabled = false;
            }
            if (cboxDebuggerLRB.Checked)
            {
                nudDebuggerLRB.Enabled = true;
            }
        }
        //Enable nud if checked
        private void cboxDebuggerPSW_CheckedChanged(object sender, EventArgs e)
        {
            if (!cboxDebuggerPSW.Checked)
            {
                nudDebuggerPSW.Enabled = false;
            }
            if (cboxDebuggerPSW.Checked)
            {
                nudDebuggerPSW.Enabled = true;
            }
        }
        //Enable nud if checked
        private void cboxDebuggerIE_CheckedChanged(object sender, EventArgs e)
        {
            if (!cboxDebuggerIE.Checked)
            {
                nudDebuggerIE.Enabled = false;
            }
            if (cboxDebuggerIE.Checked)
            {
                nudDebuggerIE.Enabled = true;
            }
        }
        private void cboxMonitorBin_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxMonitorBin.Checked)
            {
                binMonitorTimer.Enabled = true;
                cboxQuietCompiler.Enabled = true;
            }

            else
            {
                binMonitorTimer.Enabled = false;
                cboxQuietCompiler.Checked = false;
                cboxQuietCompiler.Enabled = false;
            }

        }
        private void cboxAutoStep_CheckedChanged(object sender, EventArgs e)
        {

            if (!cboxAutoStep.Checked)
            {
                timer1.Enabled = false;
            }
        }
        //auto step changed. adjust the BPs accordingly
        private void cboxStepInto_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxStepInto.Checked && intCallAddress > 0)
            {
                //Write debugger call to the start of teh sub call
                OstrichWriteSpecific_256Max(byteCodeStubCAll, intCallAddress);
            }
            else if (!cboxStepInto.Checked && intCallAddress > 0)
            {
                //Write orig code to sub call location and write debugger call to the next instruction
                OstrichWriteSpecific_256Max(BinWithPatchesAndDebuggerCode, intCallAddress, intCallAddress, (byte)byteCodeStubCAll.Length);
                OstrichWriteSpecific_256Max(byteCodeStubCAll, intNextInstructionAddress);

            }
        }
        //-------------------------------------------------------------------------
        //Send byte from serial text box
        private void btnSendByte_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    byte[] HexBytesToSend;
                    HexBytesToSend = StringToByteArray(tboxHexToSend.Text);
                    serialPort1.Write(HexBytesToSend, 0, HexBytesToSend.Length);
                }
                catch(Exception error)
                {
                    MessageBox.Show(error.Message + "\r\n\r\n" +
                        "Be sure to only use 2 digit hex values and no white space.");
                }

                
            }
        }
        //Clear Serial send textbox when clicked
        private void tboxHexToSend_MouseDown(object sender, MouseEventArgs e)
        {
            tboxHexToSend.Text = "";
        }

        //Load ROM specific settings apply manual overrides and upload to Ostrich
        private void btnReloadDebuggerCode_Click(object sender, EventArgs e)
        {
            uploadDebuggerCodeToEMU();
        }

        private void uploadDebuggerCodeToEMU()
        {
            //Problem getting settings from code file or rom
            if (!getDebuggerSettingsForROM())
            {
                return;
            }
            AddPSW_LRB_IE_nudValuesToROMCode();
            intCodeStubAddress = Convert.ToInt32(nudROMCodeAddress.Value);

            int bytesToSend = ROMSettingsAndDebuggerCodeArray.Length - intDebuggerCodeStubSourceOffset;
            
            //If debugger code stub is larger than 255 bytes, make and send multiple packets
            if (bytesToSend > 255)
            {
                int packetSize = 255;
                int packetsToSend = (packetSize / 255) + 1;
                int bytesWritten = 0;
                while (bytesToSend > 0)
                {
                    OstrichWriteSpecific_256Max(ROMSettingsAndDebuggerCodeArray, intCodeStubAddress + bytesWritten, intDebuggerCodeStubSourceOffset + bytesWritten, (byte)(packetSize), true);
                    bytesWritten += packetSize;
                    bytesToSend -= packetSize;
                    packetSize = (byte)(bytesToSend);

                }
            }
            else OstrichWriteSpecific_256Max(ROMSettingsAndDebuggerCodeArray, intCodeStubAddress, intDebuggerCodeStubSourceOffset, (byte)(ROMSettingsAndDebuggerCodeArray.Length - intDebuggerCodeStubSourceOffset), true);
            if(usingVCALreRoute) reRouteVCALinRomVectorTable(BinWithPatchesAndDebuggerCode, VCALVectorTableLoc, intCodeStubAddress, BinHasBeenUploadedSinceCreation);
            ROMWhichLoopEquals1 = false;
        }

        private void btnResetOstrich_Click(object sender, EventArgs e)
        {
            ResetOstrichVID();
        }
        //Enter pressed while in BP nud box. Set Breakpoint
        private void nudBPAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSetBreakpoint.PerformClick();
            }
        }

        private void tsOpenRenamingForm_Click(object sender, EventArgs e)
        {
                Form2 frm = new Form2();
                frm.Show();
        }
        //---------------- Tool tips ----------------------------------------------
        //Get and set tool tip boxes containing pointer location contents
        private void tboxX1_TextChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(tboxX1, GetPointerValuesFromPointerTextBox(tboxX1, "X1"));
        }
        //Get and set tool tip boxes containing pointer location contents
        private void tboxX2_TextChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(tboxX2, GetPointerValuesFromPointerTextBox(tboxX2, "X2"));
        }
        //Get and set tool tip boxes containing pointer location contents
        private void tboxDP_TextChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(tboxDP, GetPointerValuesFromPointerTextBox(tboxDP, "DP"));
        }
        private void tboxRamWatch_TextChanged(object sender, EventArgs e)
        {
            string Addy1 = (intRAMWatchDisplayedAddy).ToString("X2").PadLeft(4, '0') + "h = ";
            string Addy2 = (intRAMWatchDisplayedAddy + 1).ToString("X2").PadLeft(4, '0') + "h = ";
            string value1 = (intRAMWatchByte1).ToString("X2");
            string value2 = (intRAMWatchByte2).ToString("X2");
            toolTip1.SetToolTip(tboxRamWatch, Addy1 + value1 + "\r\n" + Addy2 + value2);
        }

        //Show individual DP Contents Bytes for ram
        private void tboxDPconts_TextChanged(object sender, EventArgs e)
        {
            if(tboxDPconts.Text != "")
            {
                string Addy1 = (intDPAddress & 0xfffe).ToString("X2").PadLeft(4, '0') + "h = ";
                string Addy2 = ((intDPAddress & 0xfffe) + 1).ToString("X2").PadLeft(4, '0') + "h = ";
                int contvalue1 = Convert.ToInt32(strPlain_DPconts, 16);
                string value1 = (contvalue1 & 0xff).ToString("X2");
                string value2 = (contvalue1 >> 8).ToString("X2");
                toolTip1.SetToolTip(tboxDPconts, Addy1 + value1 + "\r\n" + Addy2 + value2);
            }

        }

        //Show individual DP Contents Bytes for ROM
        private void tboxDPConts_ROM_TextChanged(object sender, EventArgs e)
        {
            if(tboxDPConts_ROM.Text != "")
            {
                string Addy1 = (intDPAddress & 0xfffe).ToString("X2").PadLeft(4, '0') + "h = ";
                string Addy2 = ((intDPAddress & 0xfffe) + 1).ToString("X2").PadLeft(4, '0') + "h = ";
                int contvalue1 = Convert.ToInt32(tboxDPConts_ROM.Text, 16);
                string value1 = (contvalue1 & 0xff).ToString("X2");
                string value2 = (contvalue1 >> 8).ToString("X2");
                toolTip1.SetToolTip(tboxDPConts_ROM, Addy1 + value1 + "\r\n" + Addy2 + value2);
                
            }
            
        }

        private void setMiscToolTips()
        {
            toolTip1.SetToolTip(cboxMonitorBin,"Check this box to monitor the loaded BIN or ASM file for change.  \r\n" +
                                                    "When change is detected, the file will be compiled and loaded to the emulator automatically.");
            toolTip1.SetToolTip(cboxQuietCompiler, "Use this option to suppress compiler output when monitoring BIN or ASM files.");
            toolTip1.SetToolTip(tboxDasmArgIgnore, "Enter 4 digit hex addresses, seperated by a space that want the decompiler to ignore.\r\n" +
                                                  "Use this when false tbl_ and label_ references are appearing in the ASM.");
            toolTip1.SetToolTip(tboxDasmArgForce, "Enter 4 digit hex addresses, seperated by a space that want the decompiler acknowledge.\r\n" +
                                                   "Use this you want to decompile DB or DW areas of code caused by indirect or serial references.");
            toolTip1.SetToolTip(nudROMCodeAddress, "Select an area of the ROM where you would like the debugger code to be stored.\r\n" +
                                                   "Any unused code area or High cam fuel tables are a good place to work from.\r\n" +
                                                   "Be careful not to overwrite functioning code.");
            toolTip1.SetToolTip(btnLoadBinFile, "Select a BIN file that you would like to upload and debug");
            toolTip1.SetToolTip(btnLoadASMFile, "Select an ASM file that you would like to compile and upload for debugging");
            toolTip1.SetToolTip(cboxAutoUploadOnBINLoad, "Use this option to automatically upload the BIN to the emulator.");
            toolTip1.SetToolTip(cboxSlowUpload ,"Use this option if you are having trouble with the emulator's bulk write function.");
            toolTip1.SetToolTip(btnRefreshDasm , "Decompile the loaded BIN file using the Ignore and Force arguments.");
            toolTip1.SetToolTip(btnOpenXMLFile , "Open an XML renaming file to rename references in the ASM.\r\n" +
                                                 "Create one by using \"ASM Renamer\" or included renamer in the File menu.");
            toolTip1.SetToolTip(btnApplyRenamingMask , "Rename the ASM references using the loaded XML.");
            toolTip1.SetToolTip(btnSwap , "Switch between the original and renamed ASM file.");
            toolTip1.SetToolTip(btnSaveRenamedASM , "Save the renamed ASM file.");
            toolTip1.SetToolTip(cBoxCOMPORT , "Select the COM port settings for the device connected to the ECU's serial interface.\r\n" +
                                              "Testing has shown that a baud of 39063 works best when the ECU is set to \r\n" +
                                              "38400 Baud which is the default setting on predefined ECUs.");
            toolTip1.SetToolTip(btnResetOstrich , "Use this to reset the ostrich Vendor ID.  \r\n" +
                                                  "Sometimes useful if the Ostrich gets confused.");
            toolTip1.SetToolTip(nudVIDNumber , "Ostrich vendor ID number to reset.");
            toolTip1.SetToolTip(btnLoadBinToOstrich , "Upload loaded bin to emulator.");
            toolTip1.SetToolTip(cboxRenameOnChange , "Use this option to automatically rename the ASM references everytime it is decompiled.\r\n" +
                                                     "This can be slow if there are many items to rename in the XML file.");
            toolTip1.SetToolTip(cboxLockRamWatch , "Use this option to watch the RAM address in the RAM watch box.\r\n" +
                                                    "The default behavior is to watch the last address used in an operation.\r\n" +
                                                    "Initial RAM watch address changes are always 1 instruction cycle behind.");
            toolTip1.SetToolTip(cboxStepInto , "Use this option to have the debugger follow code into CAL, SCAL and VCAL instructions.");
            toolTip1.SetToolTip(cboxLockBP , "Use this option to reset the ECU and stop on the user entered breakpoint on every \"Step\"");
            toolTip1.SetToolTip(cboxAutoStep, "Use this option to automatically step through the code.\r\n" +
                                              "To stop, uncheck the box.  It may take more than 1 click while the PC is auto-stepping");
            toolTip1.SetToolTip(cboxVCALReRoute, "Use this option to \"Re-Route\" one of the VCAL subs so the debugger code can be called \r\n" +
                                                 "using only 1 byte.  This will allow breakpoints on addresses that would otherwise crash the ECU\r\n" +
                                                 "This adds a larger footprint to the ROM debugger code and adds some CPU time to the VCAL routine\r\n" +
                                                 "that's being re-routed.  This shouldn't matter unless you are debugging very time sensitive issues.");
                                                 
        }
        //-------------------------------------------------------------------------



        //Refresh dasm button clicked. Make a new dasm using the user supplied arguments
        private void refreshDasm_Click(object sender, EventArgs e)
        {
            decompileBIN(strBINPath,CurrentASMPath,false, getDasmArgs());
            LoadASMFileIntoArraysRenameIfNeededAndUpdateASMWindow();
            int highlightTHis = Convert.ToInt32(nudBPAddress.Value);
            if (intCurrent_BP > 0) highlightTHis = intCurrent_BP;
            rtboxASMFile.Focus();
            highlightAfterValidating(highlightTHis);
            getVCALroutingInfoFromROM();

        }

        private void cboxLockBP_CheckedChanged(object sender, EventArgs e)
        {
            if(cboxLockBP.Checked == true)
            {
                cboxAutoStep.Checked = false;
                cboxAutoStep.Enabled = false;
            }
            else
            {
                cboxAutoStep.Enabled = true;
            }
        }

        private void btnFindAddress_Click(object sender, EventArgs e)
        {
            int intAddressLookup = Convert.ToInt32(nudBPAddress.Value);
            int intAddressIndex = 0;
            getIndex_ValidateAddress(ref intAddressIndex, ref intAddressLookup);
            HighlightLineInASMTextBox(intAddressIndex);
        }


        //------------------ BIN CHange monitor -----------------------------------
        //Timer that monitors BIN file for changes, then uploads and redasms when found
        private void binMonitorTimer_Tick(object sender, EventArgs e)
        {
            
            //return if a bin hasnt been uploaded yet
            if(!BinHasBeenUploadedSinceCreation)
            {
                return;
            }

            //If using an asm file for source code, see if it changed then compile it.
            if (usingASMFile)
            {
                readAllLinesInASM(ASMtoCompilePath, ref strAryNewASM);
                
                //If no changes to asm file, return;
                if (ASMSourceCodeArray.SequenceEqual(strAryNewASM))
                {
                    return;
                }
                
                //ASM was changed, update the source array and compile the ASM file
                ASMSourceCodeArray = new string[strAryNewASM.Length];
                Array.Copy(strAryNewASM, ASMSourceCodeArray, strAryNewASM.Length);
                compileASMtoBIN(ASMtoCompilePath, Path.GetDirectoryName(strBINPath) + Path.GetFileNameWithoutExtension(strBINPath), cboxQuietCompiler.Checked);
                //compileASMtoBIN(ASMtoCompilePath, strBINPath, cboxQuietCompiler.Checked);
            }
            
            //Read the original BIN file that was being monitored, or the newly compiled Bin from the ASM being monitored
            readBINFile(strBINPath, ref newBinArray);          

            //Return if the BIN size is not 32kb
            if(newBinArray.Length != 0x8000)
            {
                MessageBox.Show("Bin monitor detected an Invalid Bin size");
                return;
            }

            //return if bin hasn't changed
            if (currentBINArray.SequenceEqual(newBinArray))
            {
                return;
            }

            //Bin has changed
            lblChangedBytes.Text = "";
            byte[] blockMap = new byte[128];
            int[] changeRange = new int[256];
            
            //Map the changes and how many
            int intChanges = compareAndMapBinChanges(currentBINArray, newBinArray, ref blockMap, ref changeRange);

            //Update the current bin array, re-dasm and update the asm window
            currentBINArray = new byte[newBinArray.Length];
            Array.Copy(newBinArray, currentBINArray, newBinArray.Length);
            CreateBINArraysDecompileBINandUpdateASMWindow();

            //Update emu to reflect the new bin
            updateBinChangesToEmu(newBinArray, blockMap, changeRange);

            highlightAfterValidating(Convert.ToInt32(nudBPAddress.Value));
            lblChangedBytes.Text = "Bytes Changed: " + intChanges;
            //ReDasmBiN();

            
        }
        private void readBINFile(string path, ref byte[] destArray)
        {
            int ioRetries = 3;
            int retryDelay = 200;
            for (int i = 0; i < ioRetries; i++)
            {
                try
                {
                    //newBinArray = System.IO.File.ReadAllBytes(strBINPath);
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        int numBytesToRead = (int)stream.Length;
                        destArray = new byte[numBytesToRead];
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(destArray, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                catch (IOException) when (i < ioRetries)
                {
                    Thread.Sleep(retryDelay);
                }
            }
        }
        
        //Compare new bin file to the previously read one and map out the changes 
        //1 in blockMap = there is a change in this block
        //changeRange is an array of address pairs
        //changeRange[0] = First changed address in first block
        //changeRange[1] = Last changed address in first block
        private int compareAndMapBinChanges(byte[] prevBin, byte[] newBin, ref byte[] blockMap, ref int[] changeRange )
        {
            Array.Clear(blockMap, 0, blockMap.Length);
            Array.Clear(changeRange, 0, changeRange.Length);
            int changedByteCount = 0;
            int blockCount = 0;
            int prevBlockCount = 0;
            bool isNewBlock = true;

            //Loop through 2 bin arrays byte by byte
            for (int i = 0; i < 0x8000; i++)
            {
                //split bin mapping into 256 byte blocks
                prevBlockCount = blockCount;
                blockCount = i / 256;
                if(prevBlockCount != blockCount)
                {
                    isNewBlock = true;
                }
                
                //Change detected
                if (prevBin[i] != newBin[i])
                {
                    //Store address of the first change in the changeRange array
                    if (isNewBlock)
                    {
                        changeRange[blockCount * 2] = i;
                        isNewBlock = false;
                    }
                    //Store the last changed address in the changeRange array
                    changeRange[(blockCount * 2) + 1] = i;
                    
                    //Mark block as changed
                    blockMap[blockCount] = 1;
                    
                    changedByteCount++;
                }

            }
            return changedByteCount;

        }

        //Update the ostrich as efficiently as possible
        private void updateBinChangesToEmu (byte[] newBin, byte[] blocks, int[] range)
        {
            //Count how many blocks are changed. Write entire Bin to emu if over threshold
            int changedBlocks = 0;
            foreach  (byte block in blocks)
            {
                if (block == 1) changedBlocks++;
            }
            
            //too many changes, re write the entire EMU
            if(changedBlocks > 6)
            {
                //write entire bin then return
                quietBulkWrite = true;
                AddDebuggerCodeAndPatchesToRomAndUpload();
                return;
            }

            //Write blocks individually
            int blockNumber = 0;
            foreach (byte block in blocks)
            {
                if (block == 1)
                {
                    OstrichWriteSpecific_256Max(newBin, range[blockNumber], range[blockNumber], ((range[blockNumber + 1] - range[blockNumber]) + 1), true);
                }
                blockNumber++;
            }
            
            //Re upload debugger code, just incase the area where it was got modified
            uploadDebuggerCodeToEMU();
        }


        //--------------- VCAL ReRoute --------------------------------------------


        private void cboxVCALReRoute_CheckedChanged(object sender, EventArgs e)
        {
            //This will clear any current breakpoints and "wake" the ECU up, if a bin has been uploaded already
            //Kind of sloppy because this assumes that the ostrich port is still open, but it may not be
            if (BinHasBeenUploadedSinceCreation)
            {
                OstrichRestoreArrayAndRestoreDistantROMJumps();
                stepDebugger();
            }    
                if (cboxVCALReRoute.Checked)
                {

                    usingVCALreRoute = true;
                    CreateRomCallArraysBasedOnRomAddress(Convert.ToInt32(nudROMCodeAddress.Value));
                    uploadDebuggerCodeToEMU();
                    return;
                }

                usingVCALreRoute = false;
                if (BinHasBeenUploadedSinceCreation)
                {
                    reRouteVCALinRomVectorTable(BinWithPatchesAndDebuggerCode, VCALVectorTableLoc, realVCALAddress, true);
                }
                CreateRomCallArraysBasedOnRomAddress(Convert.ToInt32(nudROMCodeAddress.Value));
                uploadDebuggerCodeToEMU();
            
            //Remove the vcal labels from sight
            lblSizeWithVCAL.Text = "";
            lblVCALbyteSize.Text = "";
            lblVCALnumber.Text = "";

        }

        private void getVCALroutingInfoFromROM()
        {
            ImportVCAlvectorTable(currentBINArray, ref VCALvectorTable);
            FindBestVCAltoUse(ASMFileArray, VCALvectorTable, ref VCALNumber, ref VCALcallCount);
            realVCALAddress = VCALvectorTable[VCALNumber];
            VCALVectorTableLoc = BEGIN_VCAL_VECTOR_TABLE + (VCALNumber * 2); 
            CreateVCALwhiteList(ASMFileArray, VCALvectorTable, VCALNumber,VCALWhiteList);
            UpdateVCALLabels();

        }
        
        //Import the VCAL table addresses into an array. VCAL 0-7 from ROM address 28-37
        private void ImportVCAlvectorTable(byte[] binArray, ref int[] vTable)
        {
            int vcalTableStart = 0x28;
            int loByte;
            int hiByte;
            for (int i = 0; i < vTable.Length; i++)
            {
                hiByte = binArray[(i * 2) + vcalTableStart + 1];
                loByte = binArray[(i * 2) + vcalTableStart];
                vTable[i] = (hiByte << 8) + loByte;
            }
        }

        //Check every line in the asm to see if it contains the vcal addresses in the vector table
        //Count the references for each and compare to find the index of the (first) lowest VCAL count
        //Vcalnum and callCOunt = the vcal with the lowest references and callCount is the number of references (less 1 for the vcal address)
        private void FindBestVCAltoUse(string[] dasm, int[] vTable, ref int VCALnum, ref int callCount)
        {
            //Count how many times each vcal is called and return which one is the lowest
            string strvcalAddress;
            VCALnum = 0;
            int[] individualCallCount = new int[vTable.Length];
            
            //Get ref count for each vcal address
            for (int i = 0; i < vTable.Length; i++)
            {
                strvcalAddress = "; " + vTable[i].ToString("X4");
                foreach (string line in dasm)
                {
                    if (line.Contains(strvcalAddress)) individualCallCount[i]++;
                }
                //remove one reference for the actual vcal address
                individualCallCount[i]--;
            }
            
            //find the index for the lowest call count (first index of the lowest count if more than one)
            for (int j = 0; j < vTable.Length; j++)
            {
                if (individualCallCount[j] < individualCallCount[VCALnum]) VCALnum = j;
            }
            callCount = individualCallCount[VCALnum];
        }
        
        private void UpdateVCALLabels()
        {
            if (!cboxVCALReRoute.Checked) return;
            if (BINOpened && codeFileHasBeenOpened )
            {
                lblVCALnumber.Text = "VCAL: " + VCALNumber.ToString();
                lblSizeWithVCAL.Text = "Size using VCAL:";
                lblVCALbyteSize.Text = (ROMSettingsAndDebuggerCodeArray.Length - intDebuggerCodeStubSourceOffset).ToString() + " Bytes";
                return;
            }
                lblSizeWithVCAL.Text = "Size using VCAL:";
                lblVCALbyteSize.Text = "Open BIN";
                lblVCALnumber.Text = "VCAL: #";
 
        }
        
        //Create a white list of all the addresses that are calling the VCAL in the rom code
        private void CreateVCALwhiteList (string[] dasm, int[] vTable, int VCALnum, List <int> whiteList)
        {
                whiteList.Clear();
                string strvcalAddress = "; " + vTable[VCALnum].ToString("X4");
                string caller;
                foreach (string line in dasm)
                {
                    if (line.Contains(strvcalAddress) && line.Contains("from"))
                    {
                        caller = line.Substring(line.IndexOf("from") + 5, 4);
                        whiteList.Add(Convert.ToInt32(caller, 16));
                    }
                }
                //0 for ECU to ID the end of the list
                whiteList.Add(0);
        }

        //check to see if the requested BP is on the white list
        private bool CheckListForItem(int address, List <int> list)            
        {
            if (address == 0) return false;
            int ind = list.FindIndex(a => list.Contains(address));
            if (ind >= 0) return true;
            return false;

            
        }

        private void reRouteVCALinRomVectorTable(byte[] romArray, int tableLocation, int newAddress, bool writeNow = false)
        {
            if (BINOpened)
            {
                romArray[tableLocation] = (byte)(newAddress & 0xff);
                romArray[tableLocation + 1] = (byte)(newAddress >> 8);
                if (writeNow) OstrichWriteSpecific_256Max(romArray, tableLocation, tableLocation, 2, true);
            }

        }

        private void addVCALcodeToDebuggerCode()
        {
            //Add whitelist to the tail of the debugger code
            for (int i = 0; i < VCALWhiteList.Count; i++)
            {
                int listOffset = i * 2;
                int wlAddress = VCALWhiteList[i] & 0xffff;
                ROMSettingsAndDebuggerCodeArray[romOffsetWhiteList + listOffset] = (byte)(wlAddress & 0xff);
                ROMSettingsAndDebuggerCodeArray[romOffsetWhiteList + listOffset + 1] = (byte)(wlAddress >> 8);
            }
            //Add the original location of the now re-routed VCAL routine to the debugger code
            //This allows the ECU to execute the VCAL function normally when a caller from the whitelist calls it
           
            ROMSettingsAndDebuggerCodeArray[romOffsetRealVcalSub] = (byte)(realVCALAddress & 0xff);
            ROMSettingsAndDebuggerCodeArray[romOffsetRealVcalSub + 1] = (byte)(realVCALAddress >> 8);
            UpdateVCALLabels();
        }

        private void UpdateWhiteListOnRom(List <int> whtList)
        {
            byte[] ROMwhiteList = new byte[whtList.Count * 2];
            
            //Add whitelist to the tail of the debugger code
            for (int i = 0; i < whtList.Count; i++)
            {
                int listOffset = i * 2;
                int wlAddress = VCALWhiteList[i] & 0xffff;
                ROMwhiteList[listOffset] = (byte)(wlAddress & 0xff);
                ROMwhiteList[listOffset + 1] = (byte)(wlAddress >> 8);
            }
            int RomWhiteListLocation = (intCodeStubAddress + romOffsetWhiteList) - intDebuggerCodeStubSourceOffset;
            OstrichWriteSpecific_256Max(ROMwhiteList, RomWhiteListLocation , 0, ROMwhiteList.Length, true);
            
        }
    }
}
