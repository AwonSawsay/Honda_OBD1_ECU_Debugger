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
        string dataIN;
        string strReturnAddress;
        string strBinInput;
        string Dasm662Path;
        string CurrentASMPath;
        int intBreakPointDec;
        int intNextInstructionAddress;
        int intJumpAddress;
        int intCallAddress;
        int ASMIndex;
        int intReturnAddress = 0;
        int intPacketByteCount;
        int intBufferLength;
        int intPacketsRecd = 0;
        int OstrichStatus;
        int InitialOstrichBP = 0;
        int PreviousNextInstruction = 0;
        int PreviousJumpAddress = 0;
        int PreviousCallAddress = 0;
        int StartChunkAddress = 0;
        int ROMWhichLoopAddress;
        int ROMTrapLoop1BreakByte;
        int ROMTrapLoop2BreakByte;
        int BulkChunkNumber = 0;
        int LowerBoundary = 0;
        int UpperBoundary = 32768;
        int BaudRateAddress1;
        int BaudRateAddress2;
        int TimerTickCount;
        int PreviousPacketNumber = 0;
        int SecondOperandStringLength;
        Byte[] OriginalBinArray;
        Byte[] BinWithPatchesAndDebuggerCode;
        Byte[] OstrichBinArrayOut;
        byte[] OstrichSerialNumber = new byte[9];
        byte[] P13BaudRateReloadValues = new byte[] { 20, 7, 4, 1, 0 };
        byte[] P28nP72nP30BaudRateReloadValues = new byte[] { 223, 248, 251, 254, 255 };
        byte[] BaudRateReloadValuesToPatch = new byte[5];
        byte MMSB = (byte)(00);
        byte MSB = (byte)(112);
        byte ROMOffsetWhichLoop;
        byte ROMOffsetTrapLoop1BreakByte;
        byte ROMOffsetTrapLoop2BreakByte;
        byte DebuggerLRBOffsetFromRom;
        byte DebuggerPSWOffsetFromRom;
        byte DebuggerIEOffsetFromRom;
        byte[] RomCallArray = new byte[] { 50, 0, 117, 3 };
        byte[] RomModifiedJumpArray = new byte[] { 4, 50, 0, 117, 3, 50, 0, 117, 255 };
        byte[] ROMSettingsAndDebuggerCodeArray;
        bool UseFirstBuffer = true;
        bool CheckSumPassed = false;
        bool OstrichSentError = false;
        bool InitialBPAlreadyRemoved;
        bool DDFlag = false;
        bool DDFlagCorrection = false;
        bool InitialOstrichUploadComplete = false;
        bool BinHasBeenUploadedSinceCreation = false;
        bool StopWriteDueToError = false;
        bool ROMWhichLoopEquals1 = false;
        List<int> dataBuffer = new List<int>();
        List<int> dataBuffer1 = new List<int>();
        List<int> OstrichRXBuffer = new List<int>();
        List<int> WorkingBuffer = new List<int>();
        List<int> BufferBeingProcessed = new List<int>();
        string[] ASMFileArray;
        string[] RenamedASMFileArray;
        string ParsedPSWFlags = "";
        string InstructionLine = "";
        string ROMSettingsAndDebuggerCodePath;
        string PointerContentsHexValue = "";
        StreamWriter objStreamWriter;
        string pathFile;
        bool state_AppendText = true;



        #region My Own Method
        private void SaveDataToTxtFile()
        {
            if (cboxSaveTrace.Checked)
            {
                try
                {
                    string combined = dataIN + ParsedPSWFlags + InstructionLine;
                    objStreamWriter = new StreamWriter(pathFile, state_AppendText);
                    // objStreamWriter.WriteLine(dataIN);
                    //objStreamWriter.WriteLine(ParsedPSWFlags);
                    objStreamWriter.WriteLine(combined);
                    //objStreamWriter.WriteLine(InstructionLine);
                    objStreamWriter.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        #endregion

        #region ASM Handling

        private void CreateAsmFileFromBin()
        {
            try
            {
                strBinInput =OPENBINDialog.FileName;
                CurrentASMPath = System.IO.Path.GetDirectoryName(OPENBINDialog.FileName) +"\\"+System.IO.Path.GetFileNameWithoutExtension(OPENBINDialog.FileName) + ".asm";

                Dasm662Path = Directory.GetCurrentDirectory();
                Dasm662Path += @"\dasm662.exe";
                var proc = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = Dasm662Path,
                        Arguments ="\"" + strBinInput + "\"" + " " + "\"" + CurrentASMPath + "\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                while (!proc.StandardError.EndOfStream)
                {
                    string error = proc.StandardError.ReadLine();
                    MessageBox.Show(error, "dasm662 Error");
                }

                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    MessageBox.Show(line, "dasm662 Error");
                }
                if (proc.ExitCode == 0)
                {
                    if (!InitialOstrichUploadComplete || !cboxAutoUploadOnBINLoad.Checked)
                    {
                        MessageBox.Show("The BIN dasm process has completed.", "dasm662");
                    }
                   
                   
                }
                /*ASMFileArray = System.IO.File.ReadAllLines(CurrentASMPath);
                rtboxASMFile.Lines = ASMFileArray;*/
                LoadASMFileIntoStringArray();
                tboxASMSimpleName.Text = System.IO.Path.GetFileNameWithoutExtension(OPENBINDialog.FileName) + ".asm";
                ASMOpened = true;
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message + "\r\n\r\n" + "Is dasm662.exe located int the same directory as ECU Debugger? \r\n " +
                    "Also make sure the bin path does not contain illegal characters. \r\n Try moving the bin the root directory and using a simple name.");
            }
        
        }

        OpenFileDialog OPENAsmDialog = new OpenFileDialog();
        bool ASMOpened = false;
        private void btnLoadASMFile_Click(object sender, EventArgs e)
        {

            OPENAsmDialog.Filter = "ASM Files | *.asm";
            if (OPENAsmDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentASMPath = OPENAsmDialog.FileName;
                LoadASMFileIntoStringArray();
                tboxASMSimpleName.Text = OPENAsmDialog.SafeFileName;
                ASMOpened = true;
            }

        }
        
        private void LoadASMFileIntoStringArray()
        {
            ASMFileArray = System.IO.File.ReadAllLines(CurrentASMPath);
            RenamedASMFileArray = System.IO.File.ReadAllLines(CurrentASMPath);
            rtboxASMFile.Lines = ASMFileArray;
        }

        
        private void ParseASMArrayLineAndHighlightText(string HexAddress)
        {
            if (ASMOpened)
            {
                ASMIndex = LastIndexOfHexStringInArray(ASMFileArray, HexAddress);
                if (ASMIndex != -1)
                {
                    rtboxASMFile.Focus();
                    SelectLineInRichTextBox(rtboxASMFile, ASMIndex);
                    ParseArrayLine();
                }
                else
                {
                    MessageBox.Show("Can't Locate hex address in ASM File.");
                }

            }

        }
        private int LastIndexOfHexStringInArray(string[] myarray, string HexString)
        {
            int Index = 0;
            string strSearchString = "; " + HexString;
            try
            {
                Index = Array.FindLastIndex(myarray, s => s.Contains(strSearchString));
            }
            catch { }
            return Index;
        }
        private void SelectLineInRichTextBox(RichTextBox myRichTextBox, Int32 lineToGo)
        {
            Int32 j = 0;
            lineToGo++;
            String text = myRichTextBox.Text;
            if (lineToGo > myRichTextBox.Lines.Length)
                lineToGo = myRichTextBox.Lines.Length;
            for (Int32 i = 1; i < lineToGo; i++)
            {
                j = text.IndexOf('\n', j + 1);
                if (j == -1) break;
            }
            if (lineToGo > 1)
            {
                myRichTextBox.Select(j + 1, myRichTextBox.Lines[lineToGo - 1].Length);
            }
            else
            {
                myRichTextBox.Select(j, 60);
            }
        }
        private void ParseArrayLine()
        {
            intJumpAddress = 0;
            int intConditionAddress = 0;
            int intConditionAddressBit = 0;
            int intCurrentAddress = 0;
            int intNextLineAddress = 0;
            int intNextInstructionIndex;
            intCallAddress = 0;
            intNextInstructionAddress = 0;
            string strCurrentAddress = "0000";
            string strOpCode = "";
            string strJumpAddress = "0000";
            string strCallAddress = "0000";
            string strConditionAddress = "N/A";
            string strConditionAddressBit = "N/A";
            string strNextLineAddress = "N/A";
            string strNextInstructionAddress = "";
            PointerContentsHexValue = "";
            SecondOperandStringLength = 0;
            try
            {
                strOpCode = ASMFileArray[ASMIndex].Substring(16, 5).Trim();
                strCurrentAddress = ASMFileArray[ASMIndex].Substring(ASMFileArray[ASMIndex].IndexOf("; ") + 2, 4);
                intCurrentAddress = Convert.ToInt32(strCurrentAddress, 16);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message + "\r\n Did you type a valid instruction address?");
                return;
            }

            string DetermineNextInstructionAddress()
            {

                try
                {
                    strNextLineAddress = ASMFileArray[ASMIndex + 1].Substring(ASMFileArray[ASMIndex + 1].IndexOf("; ") + 2, 4);
                    intNextLineAddress = Convert.ToInt32(strNextLineAddress, 16);

                    intNextInstructionIndex = LastIndexOfHexStringInArray(ASMFileArray, strNextLineAddress);
                    strNextInstructionAddress = ASMFileArray[intNextInstructionIndex].Substring(ASMFileArray[intNextInstructionIndex].IndexOf("; ") + 2, 4);
                }
                catch
                {
                    try
                    {
                        strNextLineAddress = ASMFileArray[ASMIndex + 2].Substring(ASMFileArray[ASMIndex + 2].IndexOf("; ") + 2, 4);
                        intNextLineAddress = Convert.ToInt32(strNextLineAddress, 16);
                        intNextInstructionIndex = LastIndexOfHexStringInArray(ASMFileArray, strNextLineAddress);
                        strNextInstructionAddress = ASMFileArray[intNextInstructionIndex].Substring(ASMFileArray[intNextInstructionIndex].IndexOf("; ") + 2, 4);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message + "\r\n Be sure that you are using a clean asm file produced by asm662.");
                        strNextInstructionAddress = "0000";
                    }
                }
                return strNextInstructionAddress;
            }


            intNextInstructionAddress = Convert.ToInt32(DetermineNextInstructionAddress(), 16);

            try
            {
                switch (strOpCode)
                {
                    case "J":
                        string IndirectJumpCheck = ASMFileArray[ASMIndex].Substring(25, 2);
                        if (IndirectJumpCheck == "DP")
                        {
                            strNextInstructionAddress = tboxDP.Text;
                            intNextInstructionAddress = Convert.ToInt32(strNextInstructionAddress, 16);
                        }
                        else
                        {
                            strNextInstructionAddress = ASMFileArray[ASMIndex].Substring(30, 4);
                            intNextInstructionAddress = Convert.ToInt32(strNextInstructionAddress, 16);
                        }
                        break;
                    case "SJ":
                        strNextInstructionAddress = ASMFileArray[ASMIndex].Substring(30, 4);
                        intNextInstructionAddress = Convert.ToInt32(strNextInstructionAddress, 16);
                        break;
                    case "JEQ":
                    case "JNE":
                    case "JLT":
                    case "JLE":
                    case "JGT":
                    case "JGE":
                        strJumpAddress = ASMFileArray[ASMIndex].Substring(30, 4);
                        break;
                    case "CAL":
                        strCallAddress = ASMFileArray[ASMIndex].Substring(30, 4);
                        // strReturnAddress = DetermineNextInstructionAddress();
                        //intReturnAddress = Convert.ToInt32(DetermineNextInstructionAddress(), 16);
                        break;
                    case "JBS":
                    case "JBR":
                        strJumpAddress = ASMFileArray[ASMIndex].Substring(45, 4);
                        strConditionAddress = ASMFileArray[ASMIndex].Substring(29, 4);
                        intConditionAddress = Convert.ToInt32(strConditionAddress, 16);
                        strConditionAddressBit = ASMFileArray[ASMIndex].Substring(36, 1);
                        intConditionAddressBit = Convert.ToInt32(strConditionAddressBit, 16);
                        break;
                    case "VCAL":
                        int intVCALNum;
                        intVCALNum = Convert.ToInt32(ASMFileArray[ASMIndex].Substring(24, 1));
                        strReturnAddress = DetermineNextInstructionAddress();
                        intReturnAddress = Convert.ToInt32(DetermineNextInstructionAddress(), 16);
                        switch (intVCALNum)
                        {
                            case 0:
                                strCallAddress = "439E";
                                break;
                            case 1:
                                strCallAddress = "4405";
                                break;
                            case 2:
                                strCallAddress = "43DA";
                                break;
                            case 3:
                                strCallAddress = "446B";
                                break;
                            case 4:
                                strCallAddress = "3692";
                                break;
                            case 5:
                                strCallAddress = "449F";
                                break;
                            case 6:
                                strCallAddress = "449E";
                                break;
                            case 7:
                                strCallAddress = "448E";
                                break;
                        }
                        break;
                    case "RT":
                        strNextInstructionAddress = tboxSSP.Text;
                        strJumpAddress = tboxSSP.Text;
                        intNextInstructionAddress = Convert.ToInt32(strNextInstructionAddress, 16);
                        //intJumpAddress = Convert.ToInt32(strNextInstructionAddress, 16);
                        
                        break;
                    case "CMPB":
                        /*string CMPMCheck = ASMFileArray[ASMIndex].Substring(24, 1);
                        if (DDFlag && CMPMCheck == "A")
                        {

                            DDFlagCorrection = true;
                        }*/
                        string CMPMCheck = ASMFileArray[ASMIndex].Substring(64, 2);
                        if (DDFlag)
                        {
                            if(CMPMCheck == "C6" || CMPMCheck == "48" || CMPMCheck == "49" || CMPMCheck == "4A" || CMPMCheck == "4B"
                            || CMPMCheck == "92" || CMPMCheck == "90" || CMPMCheck == "91" || CMPMCheck == "A1" || CMPMCheck == "A0"
                            || CMPMCheck == "A4" || CMPMCheck == "C7" || CMPMCheck == "B5" || CMPMCheck == "B2" || CMPMCheck == "B3"
                            || CMPMCheck == "B0" || CMPMCheck == "B1")
                            {
                                DDFlagCorrection = true;
                            }
                            
                        }

                        break;
                    case "LCB":
                    case "CMPCB":
                        PointerContentsHexValue = GetHexValueFromPointerLocationInInstructionLine(ASMFileArray, ASMIndex, false);
                        break;
                    case "LC":
                    case "CMPC":
                        PointerContentsHexValue = GetHexValueFromPointerLocationInInstructionLine(ASMFileArray, ASMIndex, true);
                        break;

                    default:
                        strCallAddress = "0000";
                        strJumpAddress = "0000";
                        strConditionAddress = "N/A";
                        strConditionAddressBit = "N/A";
                        break;

                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message + "\r\n\r\n Couldn't parse the next possible address from the dasm.  This can be caused by " +
                    "a bad dasm file, or indirect jumps being used.");
            }

            
            intJumpAddress = Convert.ToInt32(strJumpAddress, 16);
            intCallAddress = Convert.ToInt32(strCallAddress, 16);
            
            




        }

        private string GetHexValueFromPointerLocationInInstructionLine(string[] ASMArray,int ArrayIndex,bool ReturnWord = false)
        {
            /* handles the following cases
             * , [Pointer] 
             * , 0xxxxh[Pointer]
             * , tbl_XXXX[Pointer]
             * , 0xxxxh     
             * , tbl_XXXX
             * This will check for "h", "tbl_", and "[]" and get the int value of each and then add them to get the address.
             * Then the contents of the adress is obtained and returned as a hex string. 
            

            */
            int hAddress = 0;
            int PointerRegisterAddress = 0;
            int TableAddress = 0;
            int ContentsAddress = 0;
            int ContentsByte1 = 0;
            int ContentsByte2 = 0;
            string ContentsHexString = "";
            bool boolNegativeOffset = false;

            /* string SecondInstructionOperand =
                            ASMFileArray[ArrayIndex].Substring(
                            ASMFileArray[ArrayIndex].IndexOf(",") + 2,
                            ASMFileArray[ArrayIndex].IndexOf(";") - 1).Trim();
            */
            string SecondInstructionOperand = ASMFileArray[ArrayIndex].Split(',', ';')[1].Trim();

            SecondOperandStringLength = SecondInstructionOperand.Length;
            if (SecondInstructionOperand.Contains("h"))
            {
                hAddress =
                          Convert.ToInt32(SecondInstructionOperand.Substring(
                          SecondInstructionOperand.IndexOf("h") - 4, 4),16);
                if (hAddress > 32768)
                {
                    hAddress = 65536 - hAddress;
                    boolNegativeOffset = true;

                }

            }

            if (SecondInstructionOperand.Contains("["))
            {
                /* string InstructionLinePointerRegister =
                            SecondInstructionOperand.Substring(
                            SecondInstructionOperand.IndexOf("[") + 1,
                            SecondInstructionOperand.IndexOf("]"));
                */
                string InstructionLinePointerRegister = SecondInstructionOperand.Split('[', ']')[1];
                switch (InstructionLinePointerRegister)
                {
                    case "X1":
                        PointerRegisterAddress = Convert.ToInt32(tboxX1.Text,16);
                        break;
                    case "X2":
                        PointerRegisterAddress = Convert.ToInt32(tboxX2.Text, 16);
                        break;
                    case "DP":
                        PointerRegisterAddress = Convert.ToInt32(tboxDP.Text, 16);
                        break;
                    case "ACC":
                        PointerRegisterAddress = Convert.ToInt32(tboxACC.Text, 16);
                        break;
                    default:
                        PointerRegisterAddress = 0;
                        break;
                }

            }
            if (SecondInstructionOperand.Contains("tbl_"))
            {
                string strTableAddress = SecondInstructionOperand.Substring(SecondInstructionOperand.IndexOf("tbl_") + 4, 4);
                TableAddress = Convert.ToInt32(strTableAddress, 16);
            }
           if(boolNegativeOffset)
            {
                ContentsAddress = TableAddress + PointerRegisterAddress - hAddress;
            }
            else
            {
                ContentsAddress = hAddress + TableAddress + PointerRegisterAddress;
            }
            
            
            if(ContentsAddress > 4096)
            {
                ContentsByte1 = BinWithPatchesAndDebuggerCode[ContentsAddress];
                ContentsByte2 = BinWithPatchesAndDebuggerCode[ContentsAddress + 1];
                if (ReturnWord)
                {
                    int WordValue = (ContentsByte2 << 8) + ContentsByte1;
                    ContentsHexString = WordValue.ToString("X2");
                }
                else
                {
                    ContentsHexString = ContentsByte1.ToString("X2");
                }
            }
            else
            {
                ContentsHexString = "";
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
            if(tboxECUReturnAddress.Text != "")
            {
                ParseASMArrayLineAndHighlightText(tboxECUReturnAddress.Text);
            }
            
        }

        private void rtboxASMFile_Click(object sender, EventArgs e)
        {
            if (tboxECUReturnAddress.Text != "")
            {
                ParseASMArrayLineAndHighlightText(tboxECUReturnAddress.Text);
            }
            
        }
        #endregion

        #region ASM Renaming
        DataSet dataset1 = new DataSet();
        OpenFileDialog OPENAsmRenamingFile = new OpenFileDialog();
        private void OpenXMLFile()
        {
            OPENAsmRenamingFile.Filter = "XML Files | *.xml";
            if (OPENAsmRenamingFile.ShowDialog() == DialogResult.OK)
            {
                ReadXMLFileIntoDataset();
            }
        }

        private void ReadXMLFileIntoDataset()
        {
            try
            {
                dataset1.Clear();
                dataset1.ReadXml(OPENAsmRenamingFile.FileName);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void RenameUsingValuesFromXML(string[] OrigArray, bool RestoreToOriginalValue = false)
        {
       
            
            for (int index = 0; index < OrigArray.Length; index++)
            {
                foreach(DataTable table in dataset1.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
        
                        if (RestoreToOriginalValue)
                        {
        
                            OrigArray[index] = CaseSenstiveReplace(OrigArray[index], Convert.ToString(dr[2]), Convert.ToString(dr[1]));
                        }
                       else if (Convert.ToString( dr[0]) == "True")
                        {
                            OrigArray[index] = CaseSenstiveReplace(OrigArray[index],Convert.ToString(dr[1]), Convert.ToString( dr[2]));
                        }

                        
                    }
                }
                
            }


        }



        public string CaseInsenstiveReplace(string originalString, string oldValue, string newValue)
        {
            //This works fine, but relies on regex.  When testing it was ignoring strings with "(" or ")" in searches
            //It was abandoned in favor of the Replace() method for convenience. Unfortunately searches are now case sensitive
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(oldValue,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
            return reg.Replace(originalString, newValue);
        }

        public string CaseSenstiveReplace(string originalString, string oldValue, string newValue)
        {
            if(oldValue != "")
            {
                return originalString.Replace(oldValue, newValue);
            }
            else
            {
                return originalString;
            }
        }


        private void btnOpenXMLFile_Click(object sender, EventArgs e)
        {
            OpenXMLFile();
        }

        private void btnApplyRenamingMask_Click(object sender, EventArgs e)
        {
            try
            {
                
                RenamedASMFileArray = System.IO.File.ReadAllLines(CurrentASMPath);
                ReadXMLFileIntoDataset();
                RenameUsingValuesFromXML(RenamedASMFileArray);
                rtboxASMFile.Lines = RenamedASMFileArray;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }


        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                ReadXMLFileIntoDataset();
                RenameUsingValuesFromXML(RenamedASMFileArray, true);
                rtboxASMFile.Lines = RenamedASMFileArray;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        private void btnSaveRenamedASM_Click(object sender, EventArgs e)
        {
            string RenamedAsmName = System.IO.Path.GetDirectoryName(CurrentASMPath) + "\\" + "RENAMED_" + tboxASMSimpleName.Text;
            using (StreamWriter outputfile = new StreamWriter(RenamedAsmName))
            {
                foreach (string line in RenamedASMFileArray)
                    outputfile.WriteLine(line);
            }
            MessageBox.Show("File saved as RENAMED_" + tboxASMSimpleName.Text);
        }

        #endregion

        #region GUI Method
        public Form1()
        {
            InitializeComponent();
        }


        public static byte[] StringToByteArray(String hex)
        {
            
                        
                int NumberChars = hex.Length;
                byte[] bytes = new byte[NumberChars / 2];
                for (int i = 0; i < NumberChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                return bytes;
            
         
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            serialPort1.DtrEnable = false;
            serialPort1.RtsEnable = false;
            chboxChecksum.Checked = true;
            cboxAutoStep.Checked = false;
            cboxAddCodeToRom.Checked = true;
            cboxAutoDasmBin.Checked = true;
            btnLoadASMFile.Enabled = false;
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
            
        }


        private string GetPointerValuesFromPointerTextBox(TextBox Textbox, string PointerName)
        {
            string Output ="";
            if(Textbox.Text != "" && Convert.ToInt32(Textbox.Text,16) > 12288 && Convert.ToInt32(Textbox.Text, 16) < 32769)
            {
                try
                {
                    int Pointer = Convert.ToInt32(Textbox.Text, 16);
                    string DP = BinWithPatchesAndDebuggerCode[Pointer].ToString("X2");
                    string DP1 = BinWithPatchesAndDebuggerCode[Pointer + 1].ToString("X2");
                    string DP2 = BinWithPatchesAndDebuggerCode[Pointer + 2].ToString("X2");
                    string DP3 = BinWithPatchesAndDebuggerCode[Pointer + 3].ToString("X2");
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


        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
        //Look for packet footer, then the 2 following bytes, switch buffers and call RXPacketProcessing
            int CurrentByte;
            int PacketStatus = 1;
            Thread.Sleep(5);            
            
            
            while (serialPort1.BytesToRead > 0)
            {
                
                try
                {
                    CurrentByte = (serialPort1.ReadByte());
                    if(UseFirstBuffer){dataBuffer.Add(CurrentByte);}
                    else {dataBuffer1.Add(CurrentByte); }
                    
                    switch (PacketStatus)
                    {
                        //Packet status 1 =  Looking for "DE" (dec 222)
                        case 1:
                            if (CurrentByte == 222) { PacketStatus++; }
                            break;
                        //Packet status 2 = Found "DE" now Looking for "AD" (dec 173)
                        case 2:
                            if(CurrentByte == 222)
                            { break; }
                            if (CurrentByte == 173) 
                            { PacketStatus++; } 
                            else 
                            { PacketStatus--; }
                            break;
                        //Packet status 3 = Found DE and AD. This is the checksum byte. Send packet for processing
                        case 3:
                            PacketStatus = 1;
                            intPacketsRecd++;
                            UseFirstBuffer = !UseFirstBuffer;
                            this.Invoke(new EventHandler(RXPacketProcessing));
                            break;
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }

        }



        private void RXPacketProcessing(object sender, EventArgs e)
       //Copy data from databuufer or databuffer1 to BufferBeingProcessed
        {
            try
            {
                int intPacketStartIndex;
                intPacketByteCount = 27;
                BufferBeingProcessed.Clear();
                if (UseFirstBuffer)
                {
                    intBufferLength = dataBuffer1.Count;
                    intPacketStartIndex = intBufferLength - intPacketByteCount;
                    BufferBeingProcessed = dataBuffer1.GetRange(intPacketStartIndex, intPacketByteCount);
                    dataBuffer1.Clear();
                }
                else
                {
                    intBufferLength = dataBuffer.Count;
                    intPacketStartIndex = intBufferLength - intPacketByteCount;
                    BufferBeingProcessed = dataBuffer.GetRange(intPacketStartIndex, intPacketByteCount);
                    dataBuffer.Clear();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }








              CheckSumPassed = CheckCheckSum(BufferBeingProcessed);
           

                if (chboxChecksum.Checked)
                {
                    if (CheckSumPassed)
                    {
                        this.ShowData();
                    }
                    else
                    {
                    //RequestDataResend();
                    MessageBox.Show("Serial Packet from ECU failed Checksum");
                    }
                }
                else if (!chboxChecksum.Checked)
                {
                    this.ShowData();
                }

               
        }


      
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
                        else if(RXbyte == 222)
                            { break; } 
                        else 
                            { PacketStatus--; }
                        break;
                    //Packet status 3 = Found DE and AD. Get Byte count BYTE COUTN REMOVED FROM ROM
                    case 4:
                        PacketStatus++;
                        break;
                    case 3:
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
        
        private void ShowData()
        {
           
           
            dataIN = RxDataFormat(BufferBeingProcessed);

            tboxTraceWindow.Text +="\r\n" + dataIN;
            CreatePSWBitArray();
            ParsedPSWFlags = ParsePSWFlags();
            tboxTraceWindow.AppendText(ParsedPSWFlags);
            try
            {
                if (ASMOpened)
                {
                    if(RenamedASMFileArray != null)
                    {
                        InstructionLine = "\r\n\t\t" + RenamedASMFileArray[ASMIndex] + "\r\n";
                    }
                    else
                    {
                        InstructionLine = "\r\n\t\t" + ASMFileArray[ASMIndex] + "\r\n";
                    }
                    if(PointerContentsHexValue != "")
                    {
                        InstructionLine = InstructionLine.Insert(
                            InstructionLine.IndexOf(",") + SecondOperandStringLength + 4, 
                            "( " + PointerContentsHexValue + " )");
                    }    
                    tboxTraceWindow.AppendText(InstructionLine);
                   
                }
               
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            
            SaveDataToTxtFile();
            


        }
        System.Collections.BitArray PSWBitArray;
        private System.Collections.BitArray CreatePSWBitArray()
        {
            string strPSWHBits = Convert.ToString(BufferBeingProcessed[14], 2);
            string strPSWLBits = Convert.ToString(BufferBeingProcessed[15], 2);
            string strPSWBits = strPSWHBits.PadLeft(8, '0') + strPSWLBits.PadLeft(8, '0');
            PSWBitArray = new System.Collections.BitArray(strPSWBits.Select(s => s == '1').ToArray());
            return PSWBitArray;
         }

        private string ParsePSWFlags()
        {
            int ReversedOrderBitNumber = 0;
            string strParsedFlags = "";
            string BCB = "";
            string SCB = "";
            foreach(bool element in PSWBitArray)
            {
                
                switch (ReversedOrderBitNumber)
                {
                    case 0:
                        if (tboxCF.Text != "" && tboxCF.Text != Convert.ToString(Convert.ToInt32(element)))
                        {
                            strParsedFlags = "\r\n\t\t" + "   CF = [" + Convert.ToString(Convert.ToInt16(element)) + "]";
                        }
                        else
                        {
                            strParsedFlags = "\r\n\t\t" + "   CF = " + Convert.ToString(Convert.ToInt16(element));
                        }
                        tboxCF.Text = Convert.ToString(Convert.ToInt16(element));
                        break;
                    case 1:
                        if (tboxZF.Text != "" && tboxZF.Text != Convert.ToString(Convert.ToInt32(element)))
                        {
                            strParsedFlags += "  ZF = [" + Convert.ToString(Convert.ToInt16(element)) + "]";
                        }
                        else
                        {
                            strParsedFlags += "  ZF = " + Convert.ToString(Convert.ToInt16(element));
                        }
                        tboxZF.Text = Convert.ToString(Convert.ToInt16(element));
                        break;
                    case 2:
                        if (tboxHC.Text != "" && tboxHC.Text != Convert.ToString(Convert.ToInt32(element)))
                        {
                            strParsedFlags += "  HC = [" + Convert.ToString(Convert.ToInt16(element)) + "]";
                        }
                        else
                        {
                            strParsedFlags += "  HC = " + Convert.ToString(Convert.ToInt16(element));
                        }
                        tboxHC.Text = Convert.ToString(Convert.ToInt16(element));
                        break;
                    case 3:
                        if (tboxDD.Text != "" && tboxDD.Text != Convert.ToString(Convert.ToInt32(element)))
                        {
                            strParsedFlags += "  DD = [" + Convert.ToString(Convert.ToInt16(element)) + "]";
                        }
                        else
                        {
                            strParsedFlags += "  DD = " + Convert.ToString(Convert.ToInt16(element));
                        }
                        int DD = Convert.ToInt16(element);
                        tboxDD.Text = Convert.ToString(DD);
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        if (tboxSF.Text != "" && tboxSF.Text != Convert.ToString(Convert.ToInt32(element)))
                        {
                            strParsedFlags += "  SF = [" + Convert.ToString(Convert.ToInt16(element)) + "]";
                        }
                        else
                        {
                        strParsedFlags += "  SF = " + Convert.ToString(Convert.ToInt16(element));
                        }
                        tboxSF.Text = Convert.ToString(Convert.ToInt16(element));
                        break;
                    case 7:
                        if (tboxMIE.Text != "" && tboxMIE.Text != Convert.ToString(Convert.ToInt32(element)))
                        {
                            strParsedFlags += "  MIE = [" + Convert.ToString(Convert.ToInt16(element)) + "]";
                        }
                        else
                        {
                            strParsedFlags += "  MIE = " + Convert.ToString(Convert.ToInt16(element));
                        }
                        tboxMIE.Text = Convert.ToString(Convert.ToInt16(element));
                        break;
                    case 8:
                    case 9:
                        break;
                    case 10:
                        BCB = Convert.ToString(Convert.ToInt16(element));
                        break;
                    case 11:
                        BCB += Convert.ToString(Convert.ToInt16(element));
                        if (tboxBCB.Text != "" && tboxBCB.Text != Convert.ToString(Convert.ToInt32(BCB, 2)))
                        {
                            strParsedFlags += "  BCB = [" + Convert.ToString(Convert.ToInt32(BCB, 2)) + "]";
                        }
                        else
                        {
                            strParsedFlags += "  BCB = " + Convert.ToString(Convert.ToInt32(BCB, 2));
                        }
                        tboxBCB.Text = Convert.ToString( Convert.ToInt32(BCB, 2));
                        break;
                    case 12:
                        break;
                    case 13:
                        SCB = Convert.ToString(Convert.ToInt16(element));
                        break;
                    case 14:
                        SCB += Convert.ToString(Convert.ToInt16(element));
                        break;
                    case 15:
                        SCB += Convert.ToString(Convert.ToInt16(element));
                        if (tboxSCB.Text != "" && tboxSCB.Text != Convert.ToString(Convert.ToInt32(SCB, 2)))
                        {
                            strParsedFlags += " SCB = [" + Convert.ToString(Convert.ToInt32(SCB, 2)) + "]";
                        }
                        else
                        {
                            strParsedFlags += " SCB = " + Convert.ToString(Convert.ToInt32(SCB, 2));
                        }
                        tboxSCB.Text = Convert.ToString( Convert.ToInt32(SCB, 2));
                        break;
                }

                ReversedOrderBitNumber++;
            }
            return strParsedFlags;
            
        }

        private string RxDataFormat(List<int> dataInput)
        {
            string strOut = "";
            string ECUreturnAddress= "";
            int regNum = 0;
            strOut += intPacketsRecd + ")        ";
            bool DPChanged = false;
            string OldDP = "";
            bool X1Changed = false;
            string OldX1 = "";
            bool X2Changed = false;
            string OldX2 = "";
            bool PSWChanged = false;
            bool LRBChanged = false;
            string OldLRB = "";
            bool ACCChanged = false;
            string OldACC = "";
            bool SSPChanged = false;
            string OldSSP = "";


            foreach (int element in dataInput)
            {

                switch (regNum)
                {
                    case 0:
                        if(tboxR0.Text != element.ToString("X2"))
                        {
                            strOut += "    R0= [" + element.ToString("X2") + "]  ";
                        }
                        else
                        {
                            strOut += "    R0= " + element.ToString("X2") + "  ";
                        }
                        
                        tboxR0.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 1:
                        if (tboxR1.Text != element.ToString("X2"))
                        {
                            strOut += " R1= [" + element.ToString("X2") + "]  ";
                        }
                        else
                        {
                            strOut += " R1= " + element.ToString("X2") + "  ";
                        }
                        
                        tboxR1.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 2:
                        if (tboxR2.Text != element.ToString("X2"))
                        {
                            strOut += " R2= [" + element.ToString("X2") + "]  ";
                        }
                        else
                        {
                            strOut += " R2= " + element.ToString("X2") + "  ";
                        }
                        tboxR2.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 3:
                        if (tboxR3.Text != element.ToString("X2"))
                        {
                            strOut += " R3= [" + element.ToString("X2") + "]  ";
                        }
                        else
                        {
                            strOut += " R3= " + element.ToString("X2") + "  ";
                        }
                        tboxR3.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 4:
                        if (tboxR4.Text != element.ToString("X2"))
                        {
                            strOut += " R4= [" + element.ToString("X2") + "]  ";
                        }
                        else
                        {
                            strOut += " R4= " + element.ToString("X2") + "  ";
                        }
                        tboxR4.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 5:
                        if (tboxR5.Text != element.ToString("X2"))
                        {
                            strOut += " R5= [" + element.ToString("X2") + "]  ";
                        }
                        else
                        {
                            strOut += " R5= " + element.ToString("X2") + "  ";
                        }
                        tboxR5.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 6:
                        if (tboxR6.Text != element.ToString("X2"))
                        {
                            strOut += " R6= [" + element.ToString("X2") + "]  ";
                        }
                        else
                        {
                            strOut += " R6= " + element.ToString("X2") + "  ";
                        }
                        tboxR6.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 7:
                        if (tboxR7.Text != element.ToString("X2"))
                        {
                            strOut += " R7= [" + element.ToString("X2") + "]  ";
                        }
                        else
                        {
                            strOut += " R7= " + element.ToString("X2") + "  ";
                        }
                        tboxR7.Text = element.ToString("X2") ;
                        regNum++;
                        break;
                    case 8:
                        
                        if (tboxDP.Text.Length > 2 && tboxDP.Text.Substring(0,2) != element.ToString("X2"))
                        {
                           DPChanged = true;
                        }
                        strOut += "\r\n\t\t   DP" + "= " + element.ToString("X2");
                        OldDP = tboxDP.Text;
                        tboxDP.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 9:

                        strOut += element.ToString("X2") + "  ";
                        if (OldDP.Length > 2 && OldDP.Substring(2, 2) != element.ToString("X2") || DPChanged)
                        {
                            strOut = strOut.Insert(strOut.Length - 6, "[");
                            strOut = strOut.Insert(strOut.Length - 2, "]");
                        }
                        tboxDP.Text += element.ToString("X2");
                        regNum++;
                        break;
                    case 10:
                        if (tboxX1.Text.Length > 2 && tboxX1.Text.Substring(0,2) != element.ToString("X2"))
                        {
                            X1Changed = true;
                        }
                        strOut += "X1" + "= " + element.ToString("X2");
                        OldX1 = tboxX1.Text;
                        tboxX1.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 11:
                        strOut += element.ToString("X2") + "  ";
                        if (OldX1.Length > 2 && OldX1.Substring(2, 2) != element.ToString("X2") || X1Changed)
                        {
                            strOut = strOut.Insert(strOut.Length - 6, "[");
                            strOut = strOut.Insert(strOut.Length - 2, "]");
                        }
                        
                        tboxX1.Text += element.ToString("X2");
                        regNum++;
                        break;
                    case 12:
                        if (tboxX2.Text.Length > 2 && tboxX2.Text.Substring(0, 2) != element.ToString("X2"))
                        {
                            X2Changed = true;
                        }
                        strOut += "X2" + "= " + element.ToString("X2");
                        OldX2 = tboxX2.Text;
                        tboxX2.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 13:
                        strOut += element.ToString("X2") + "  ";
                        if (OldX2.Length > 2 && OldX2.Substring(2, 2) != element.ToString("X2") || X2Changed)
                        {
                            strOut = strOut.Insert(strOut.Length - 6, "[");
                            strOut = strOut.Insert(strOut.Length - 2, "]");
                        }
                        tboxX2.Text += element.ToString("X2");
                        regNum++;
                        break;
                    case 14:
                        if (tboxPSWH.Text != ""  && tboxPSWH.Text != element.ToString("X2"))
                        {
                            PSWChanged = true;
                        }
                        strOut += "PSW= " + element.ToString("X2");
                        tboxPSWH.Text = element.ToString("X2");
                        // Is the DD flag Set? This is used to compensate for some DASM discrepancies
                        int PSWand = element & 16;
                        if (PSWand == 16)
                        {
                            DDFlag = true;
                        }
                        else
                        {
                            DDFlag = false;
                        }

                        regNum++;
                        break;
                    case 15:
                        strOut += element.ToString("X2") + "  ";
                        if (tboxPSWL.Text != "" && tboxPSWL.Text != element.ToString("X2") || PSWChanged)
                        {
                            strOut = strOut.Insert(strOut.Length - 6, "[");
                            strOut = strOut.Insert(strOut.Length - 2, "]");
                        }
                        tboxPSWL.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 16:
                        if (tboxLRB.Text.Length > 2 && tboxLRB.Text.Substring(0, 2) != element.ToString("X2"))
                        {
                            LRBChanged = true;
                        }
                        strOut += "LRB= " + element.ToString("X2");
                        OldLRB = tboxLRB.Text;
                        tboxLRB.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 17:
                        strOut += element.ToString("X2") + "  ";
                        if (OldLRB.Length > 2 && OldLRB.Substring(2, 2) != element.ToString("X2") || LRBChanged)
                        {
                            strOut = strOut.Insert(strOut.Length - 6, "[");
                            strOut = strOut.Insert(strOut.Length - 2, "]");
                        }
                        tboxLRB.Text += element.ToString("X2");
                        regNum++;
                        break;
                    case 18:
                        if (tboxACC.Text.Length > 2 && tboxACC.Text.Substring(0, 2) != element.ToString("X2"))
                        {
                            ACCChanged = true;
                        }
                        strOut += " ACC= " + element.ToString("X2");
                        OldACC = tboxACC.Text;
                        tboxACC.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 19:
                        strOut += element.ToString("X2") + "  ";
                        if (OldACC.Length > 2 && OldACC.Substring(2, 2) != element.ToString("X2") || ACCChanged)
                        {
                            strOut = strOut.Insert(strOut.Length - 6, "[");
                            strOut = strOut.Insert(strOut.Length - 2, "]");
                        }
                        tboxACC.Text += element.ToString("X2");
                        regNum++;
                        break;
                    case 20:
                        string BP1 = "BP= " + element.ToString("X2");
                        strOut = strOut.Insert(11, BP1);
                        ECUreturnAddress = element.ToString("X2");
                        regNum++;
                        break;
                    case 21:
                        string BP2 = element.ToString("X2");
                        strOut = strOut.Insert(17, BP2);
                        ECUreturnAddress += element.ToString("X2");
                       /* if(tboxECUReturnAddress.Text != ECUreturnAddress)
                        {
                            tboxECUReturnAddress.Text = ECUreturnAddress;
                        }*///Moved this to SSP to try and correct some RT bugs after a push or pop command

                        regNum++;
                        break;
                    case 22:
                        if (tboxSSP.Text.Length > 2 && tboxSSP.Text.Substring(0, 2) != element.ToString("X2"))
                        {
                            SSPChanged = true;
                        }
                        strOut += "SSP= " + element.ToString("X2");
                        OldSSP = tboxSSP.Text;
                        tboxSSP.Text = element.ToString("X2");
                        regNum++;
                        break;
                    case 23:
                        strOut += element.ToString("X2") + "  ";
                        if (OldSSP.Length > 2 && OldSSP.Substring(2, 2) != element.ToString("X2") || SSPChanged)
                        {
                            strOut = strOut.Insert(strOut.Length - 6, "[");
                            strOut = strOut.Insert(strOut.Length - 2, "]");
                        }
                        tboxSSP.Text += element.ToString("X2");

                        regNum++;
                        break;
                    default:
                        break;
                }
                   
            }
            if (tboxECUReturnAddress.Text != ECUreturnAddress)
            {
                tboxECUReturnAddress.Text = ECUreturnAddress;
            }

            return strOut;
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This program will allow you to step through the instructions on " +
                "OBD 1 Honda ECUs while monitoring the main registers as they are changed " +
                "\r\n\r\n Created by Awon using code provided by Catur P.  " +
                "\r\n dasm662 by Andy Sloan and Doc is also used and distributed with ECU Debugger." ,
                "About ECU Debugger", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

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


        private void tBoxDataIN_TextChanged(object sender, EventArgs e)
        {

                tboxTraceWindow.Focus();
                tboxTraceWindow.SelectionStart = tboxTraceWindow.Text.Length + 1;
                tboxTraceWindow.ScrollToCaret();
           
        }

        private void cBoxCOMPORT_DropDown(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxCOMPORT.Items.Clear();
            cBoxCOMPORT.Items.AddRange(ports);
        }
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
                serialPort1.Close();
               
            }
        }
        private void tBoxDataOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            
                //In Hex format, the textbox only accepts the 0-9 key and A-F key
                //The lower case will convert to the upper case, so both can enter on the textbox
                char uppercase = char.ToUpper(c);

                //if it is not the numbers key pressed, not the backspace key pressed, not the delete key pressed,  not the A-F key pressed
                if (!char.IsDigit(uppercase) && uppercase != 8 && uppercase != 46  && !(uppercase >= 65 && uppercase <= 70))
                {
                    //Cancel the KeyPress Event
                    e.Handled = true;
                }

        }





        #endregion

       #region Ostrich

        private void serialport2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            OstrichRXBuffer.Clear();
            int CurrentByte;
            int OstrichRXChecksum = 0;
            int ByteCount = 0;
            while (serialPort2.BytesToRead > 0)
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
                                if (ByteCheckSum == CurrentByte)
                                {
                                }
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

        private void DisplayOstrichError(object sender, EventArgs e)
        {
            tboxOstrichSerialNumber.Text = "Ostrich Error";
            MessageBox.Show("Ostrich Sent an Error");
            OstrichSentError = false;
        }

        private void DisplayOstrichSerialNumber(object sender, EventArgs e)
        {


            tboxOstrichSerialNumber.Text = OstrichRXDataFormat(OstrichRXBuffer);
            OstrichRXBuffer.Clear();

        }
        private void CreateNewBinArraysFromOpenBINDialogFile()
        {
            tboxBinSimpleName.Text = OPENBINDialog.SafeFileName;
            OriginalBinArray = System.IO.File.ReadAllBytes(OPENBINDialog.FileName);
            OstrichBinArrayOut = System.IO.File.ReadAllBytes(OPENBINDialog.FileName);
            BinWithPatchesAndDebuggerCode = System.IO.File.ReadAllBytes(OPENBINDialog.FileName);
            if (cboxAutoDasmBin.Checked)
            {
                rtboxASMFile.Text = "";
                CreateAsmFileFromBin();
            }
            BinHasBeenUploadedSinceCreation = false;
        }

        private void PrepareBinAndUploadToOstrich()
        {
            if (BINOpened)
            {
                if (BinHasBeenUploadedSinceCreation)
                {
                    CreateNewBinArraysFromOpenBINDialogFile();
                }
                LoadRomSettingsAndDebuggerCode();
                CreateRomCallArraysBasedOnRomAddress(Convert.ToInt32(nudROMCodeAddress.Value));

                if (ROMSettingsAndDebuggerCodePath != null)
                {
                    try
                    {
                        if (cboxAddCodeToRom.Checked)
                        {
                            AddDebuggerCodeToRom();
                        }
                        SetECUBaudRateInROM(BaudRateReloadValuesToPatch, BinWithPatchesAndDebuggerCode, BaudRateAddress1, BaudRateAddress2);
                        AddConfigPatchesToRom();
                        if (cboxSlowUpload.Checked)
                        {
                            OstrichSlowWriteEntireRom(BinWithPatchesAndDebuggerCode);
                        }
                        else
                        {
                            OstrichWriteBulk(BinWithPatchesAndDebuggerCode);
                        }
                        InitialOstrichUploadComplete = true;
                        BinHasBeenUploadedSinceCreation = true;
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Problem with Debugger code path. Have you selected a valid ROM?");
                }
            }
            else
            {
                MessageBox.Show("Please open a BIN file first.");
            }
        }

        private void btnLoadBinToOstrich_Click(object sender, EventArgs e)
        {
            PrepareBinAndUploadToOstrich();
        }

        private void ContinueBulkWrite(object sender, EventArgs e)
        {

            StartChunkAddress = StartChunkAddress + 4096;
            OstrichWriteBulk(BinWithPatchesAndDebuggerCode);

        }

        private string OstrichRXDataFormat(List<int> dataInput)
        {
            string strOut = "";

            foreach (int element in dataInput)
            {
                strOut += element.ToString("X2") + " ";
            }

            return strOut;
        }

        private void cboxOstrichComPortNumber_DropDown(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cboxOstrichComPortNumber.Items.Clear();
            cboxOstrichComPortNumber.Items.AddRange(ports);
        }

        private void btnOpenOstrichCom_Click(object sender, EventArgs e)
        {
            Array.Clear(OstrichSerialNumber,0,OstrichSerialNumber.Length);
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



        private void SetECUBaudRateInROM(byte[] ReloadValues, byte[] DestinationArray, int RomAddress1, int RomAddress2 )
        {
        
                switch (CBoxBaudRate.Text)
                {
                    case "9600":
                        Array.Copy(ReloadValues, 0, DestinationArray, RomAddress1, 1);
                        Array.Copy(ReloadValues, 0, DestinationArray, RomAddress2, 1);
                        break;
                    case "38400":
                        Array.Copy(ReloadValues, 1, DestinationArray, RomAddress1, 1);
                        Array.Copy(ReloadValues, 1, DestinationArray, RomAddress2, 1);
                        break;
                    case "62500":
                        Array.Copy(ReloadValues, 2, DestinationArray, RomAddress1, 1);
                        Array.Copy(ReloadValues, 2, DestinationArray, RomAddress2, 1);
                        break;
                    case "156250":
                        Array.Copy(ReloadValues, 3, DestinationArray, RomAddress1, 1);
                        Array.Copy(ReloadValues, 3, DestinationArray, RomAddress2, 1);
                        break;
                    case "312500":
                        Array.Copy(ReloadValues, 4, DestinationArray, RomAddress1, 1);
                        Array.Copy(ReloadValues, 4, DestinationArray, RomAddress2, 1);
                        break;
                    default:
                        break;
                }
           
           
        }

        private void btnCloseOstrichCom_Click(object sender, EventArgs e)
        {
            tboxOstrichSerialNumber.Clear();
            if (serialPort2.IsOpen)
            {
                serialPort2.Close();
                progressBar2.Value = 0;
            }
        }


        private void OstrichRestoreArrayAndRestoreDistantROMJumps(bool WriteAll)
        {
            //This will attempt to restore the emulator to it's original Loaded bin code, with as few writes as possible.

            //This first part will set boundarys for a 256 byte write to the ostrich. 
            // We're going to write all the different changes to an array and then execute 1 write for all the changes in the 256 byte block
            //If something is outside of the boundaries, we will write it seperately
            
            //Setting boundaries
            if(intNextInstructionAddress < 129)
            {
                LowerBoundary = 0;
            }
            else
            {
                LowerBoundary = intNextInstructionAddress - 128;
            }
            if (intNextInstructionAddress > 32640)
            {
                UpperBoundary = 32768;
            }
            else
            {
               UpperBoundary = intNextInstructionAddress + 127;
            }
            //Get the current breakpoint address and then restore the original data if its outside of the boundaries
            int BP = 0;
            if (tboxECUReturnAddress.Text != "")
            {
                BP = Convert.ToInt32(tboxECUReturnAddress.Text, 16);
            }
            if(BP > 0 && (BP < (LowerBoundary) || BP > (UpperBoundary)))
            {
                OstrichWriteSpecific(BinWithPatchesAndDebuggerCode, BP, BP, (byte)RomCallArray.Length);
            }
            
            //Restore the original breakpoint address if it hasn't been done already and its outside the boundaries
            if(InitialOstrichBP > 0 && !InitialBPAlreadyRemoved && (InitialOstrichBP < (LowerBoundary) || InitialOstrichBP > (UpperBoundary)))
            {
                OstrichWriteSpecific(BinWithPatchesAndDebuggerCode, InitialOstrichBP, InitialOstrichBP, (byte)RomCallArray.Length);
                InitialBPAlreadyRemoved = true;
            }
            //Restore the previous jump address if its outside the boundaries
            if (PreviousJumpAddress > 0 && (PreviousJumpAddress < (LowerBoundary) ||PreviousJumpAddress > (UpperBoundary)))
            {
                OstrichWriteSpecific(BinWithPatchesAndDebuggerCode, PreviousJumpAddress, PreviousJumpAddress, (byte)RomCallArray.Length);
            }
            //Restore the previous call address if its outside the boundaries
            if (PreviousCallAddress > 0 && (PreviousCallAddress < (LowerBoundary) || PreviousCallAddress > (UpperBoundary)))
            {
                OstrichWriteSpecific(BinWithPatchesAndDebuggerCode, PreviousCallAddress, PreviousCallAddress, (byte)RomCallArray.Length);
            }
           //Copy our original bin file to the OstrichBinArrayOut array and write our 255 byte chunk if we're suppose to.
            Array.Copy(BinWithPatchesAndDebuggerCode, 0, OstrichBinArrayOut, 0, BinWithPatchesAndDebuggerCode.Length);
            if (WriteAll)
            {
                OstrichWriteSpecific(BinWithPatchesAndDebuggerCode, LowerBoundary,  LowerBoundary, 255);
            }
        }

        
        
        
       

        private void OstrichSetBreakpoint()
        {
            

            if (BINOpened)
            {
                
                OstrichRestoreArrayAndRestoreDistantROMJumps(true);

                intBreakPointDec = Convert.ToInt32(nudBPAddress.Value);
                
                //Write romcallarrary to ostrich
                OstrichWriteSpecific(RomCallArray, intBreakPointDec);

                InitialBPAlreadyRemoved = false;
                InitialOstrichBP = intBreakPointDec;
            }
            else
            {
                MessageBox.Show("Open a Bin file first");
            }
        }

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
        private void OstrichSlowWriteEntireRom(byte[] SourceRomArray)
        {
            
            int WriteStartAddress = 0;
            byte ByteBlockSize = 255;
            StopWriteDueToError = false;
            while(WriteStartAddress < 32513 && !StopWriteDueToError)
            {
                OstrichWriteSpecific(SourceRomArray, WriteStartAddress, WriteStartAddress, ByteBlockSize, false);
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
        private void OstrichWriteSpecific(byte[] SourceArray, int RomStartAddress, int SourceArrayStartAddress = 0, byte BytesToWrite = 0, bool WaitForResponse = true)
        {

            if (serialPort2.IsOpen)
            {
                OstrichSentError = false;
                int ActualBytesToWrite = Convert.ToInt32(SourceArray.Length);
                byte[] BytesToSend = BitConverter.GetBytes(SourceArray.Length);
                if (BytesToWrite > 0)
                {
                    if(BytesToWrite > 254)
                    {
                        BytesToSend[0] = 0;
                        ActualBytesToWrite = 256;
                        

                    }
                    else
                    {
                        BytesToSend[0] = BytesToWrite;
                        ActualBytesToWrite = Convert.ToInt32(BytesToWrite);
                    }
                    
                }

                RomStartAddress = RomStartAddress + 32768;
                byte[] OutputArray = new byte[ActualBytesToWrite + 5];
                byte MSB = (byte)(RomStartAddress >> 8);
                byte LSB = (byte)(RomStartAddress & 0xFF);
                int ByteCount = 0;


                bool LoopDone = false;
                while (!LoopDone)
                {

                    switch (ByteCount)
                    {
                        case 0:
                            OutputArray[ByteCount] = 87; //W for write
                            break;
                        case 1:
                            OutputArray[ByteCount] = BytesToSend[0];
                            break;
                        case 2:
                            OutputArray[ByteCount] = MSB;
                            break;
                        case 3:
                            OutputArray[ByteCount] = LSB;
                            break;
                        case 4:
                            Array.Copy(SourceArray, SourceArrayStartAddress, OutputArray, ByteCount, ActualBytesToWrite);
                            break;
                        case 5:
                            int ChecksumLocation = OutputArray.Length - 1;
                            int Checksum = 0;
                            foreach (byte element in OutputArray)
                            {
                                Checksum = Checksum + element;
                            }


                            byte[] ChecksumByte = BitConverter.GetBytes(Checksum);
                            OutputArray[ChecksumLocation] = ChecksumByte[0];
                            LoopDone = true;
                            break;
                    }

                    ByteCount++;
                }
                if (serialPort2.IsOpen)
                {
                    serialPort2.Write(OutputArray, 0, OutputArray.Length);
                    if (WaitForResponse)
                    {
                        try
                        {
                            serialPort2.ReadTimeout = 250;
                            int OstrichResponse = serialPort2.ReadByte();
                            if (OstrichResponse != 79)
                            {
                                MessageBox.Show("Bad response from Ostrich after write request");
                                cboxAutoStep.Checked = false;
                                StopWriteDueToError = true;
                               
                            }
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show(error.Message + "\r\n" + "If this continues to happen, you may need to unplug and replug the Ostrich and re-open the port");
                            StopWriteDueToError = true;
                        }


                    }

                }

            }
            else
            {
                MessageBox.Show("Open Ostrich COM port first.");
                StopWriteDueToError = true;
            }





        }
        
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
                        MessageBox.Show("BIN Uploaded to Ostrich without error.");
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

        
        private void WriteModifiedJump(byte ReturnAddressOffset)
        {
            RomModifiedJumpArray[RomModifiedJumpArray.Length - 1] = ReturnAddressOffset;
            //OstrichWriteSpecific(RomModifiedJumpArray, (intNextInstructionAddress - 1));
            //ModifiedJump = true;
            Array.Copy(RomModifiedJumpArray, 0, OstrichBinArrayOut, (intNextInstructionAddress - 1), RomModifiedJumpArray.Length);

        }
        
        private void OstrichWriteJumpsToRom()
        {   //This routine will evaluate the distance between the jump address and the next instruction address and then write the calls to the dumper routine
            // if there is not enough room inbetween, the jump offset will be changed to make room and the return address modifier will be changed to reflect the intended return address

            try
            {
                if (DDFlagCorrection) //This looks for DD related dasm errors and adjusts for them. EG- Listed as CMPB only looks at the next byte, but the ECU is using CMP and looking at the next 2
                {
                    intNextInstructionAddress++;
                    DDFlagCorrection = false;
                }
                
                if((intNextInstructionAddress & 255) == 222 || (intJumpAddress & 255) == 222 || (intCallAddress & 255) == 222)
                   
                {
                    //Can not write the start of a jump to any address that ends with DE, need to make accomodations or it crashes the ecu
                    //MessageBox.Show("The ostrich or ROM doesn't like having calls written to some addresses. This may be one of them. You may have to reload the ostrich and set a breakpoint ahead of this location.");
                }

                if (intJumpAddress > 0)
                {
                    if (intJumpAddress > intNextInstructionAddress && (intJumpAddress - intNextInstructionAddress) < 4)
                    {
                        int N = 7 - BinWithPatchesAndDebuggerCode[(intNextInstructionAddress - 1)];
                        WriteModifiedJump((byte)N);
                    }
                    else if (intNextInstructionAddress > intJumpAddress && (intNextInstructionAddress - intJumpAddress) < 7)
                    {
                        int N = 263 - BinWithPatchesAndDebuggerCode[(intNextInstructionAddress - 1)];
                        WriteModifiedJump((byte)N);
                    }
                    else
                    {
                        //OstrichWriteSpecific(RomCallArray, intJumpAddress);
                        //OstrichWriteSpecific(RomCallArray, intNextInstructionAddress);
                        Array.Copy(RomCallArray, 0, OstrichBinArrayOut, intJumpAddress, RomCallArray.Length);
                        Array.Copy(RomCallArray, 0, OstrichBinArrayOut, intNextInstructionAddress, RomCallArray.Length);
                    }
                }

                else
                {
                    //OstrichWriteSpecific(RomCallArray, intNextInstructionAddress);
                    Array.Copy(RomCallArray, 0, OstrichBinArrayOut, intNextInstructionAddress, RomCallArray.Length);
                }

                if (cboxStepInto.Checked && intCallAddress > 0)
                {
                    
                    if(intCallAddress < LowerBoundary || intCallAddress > UpperBoundary)
                    {
                        OstrichWriteSpecific(RomCallArray, intCallAddress);
                    }
                    else
                    {
                        Array.Copy(RomCallArray, 0, OstrichBinArrayOut, intCallAddress, RomCallArray.Length);
                    }
                    

                }
                if(intNextInstructionAddress > 0 && (intNextInstructionAddress <(LowerBoundary) || intNextInstructionAddress > (UpperBoundary)))
                {
                    OstrichWriteSpecific(RomCallArray, intNextInstructionAddress);
                }
                if(intJumpAddress >0 && intJumpAddress <(LowerBoundary) || intJumpAddress >(UpperBoundary))
                {
                    OstrichWriteSpecific(RomCallArray, intJumpAddress);
                }

                OstrichWriteSpecific(OstrichBinArrayOut, LowerBoundary, LowerBoundary, 255);

                PreviousNextInstruction = intNextInstructionAddress;
                PreviousJumpAddress = intJumpAddress;
                PreviousCallAddress = intCallAddress;
                if (cboxAutoStep.Checked) 
                {
                    timer1.Enabled = false;
                    timer1.Enabled = true;
                    Task.Delay(75).ContinueWith(_ => { BreakLoopUsingRomAddress(); });
                    
                
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

           
        }



        private void CleartboxECUReturnAddress(object sender, EventArgs e)
        {
            tboxECUReturnAddress.Text = "";
        }
        
        private void BreakLoopUsingRomAddress()
        {
            ROMWhichLoopAddress = Convert.ToInt32(nudROMCodeAddress.Value) + (ROMOffsetWhichLoop - ROMSettingsAndDebuggerCodeArray[0]);
            ROMTrapLoop1BreakByte = Convert.ToInt32(nudROMCodeAddress.Value) + (ROMOffsetTrapLoop1BreakByte - ROMSettingsAndDebuggerCodeArray[0]);
            ROMTrapLoop2BreakByte = Convert.ToInt32(nudROMCodeAddress.Value) + (ROMOffsetTrapLoop2BreakByte - ROMSettingsAndDebuggerCodeArray[0]);
            if (!cboxLockBP.Checked)
            {
                this.Invoke(new EventHandler(CleartboxECUReturnAddress));
            }

            if (ROMWhichLoopEquals1)
            {
                byte[] LoopBreak = new byte[1] { 0 };
                OstrichWriteSpecific(LoopBreak, ROMWhichLoopAddress, 0, 1, false);
                OstrichWriteSpecific(LoopBreak, ROMTrapLoop1BreakByte, 0, 1, false);
                OstrichWriteSpecific(LoopBreak, ROMTrapLoop2BreakByte, 0, 1, false);
                ROMWhichLoopEquals1 = false;
            }
            else
            {
                byte[] LoopBreak = new byte[1] { 1 };
                OstrichWriteSpecific(LoopBreak, ROMWhichLoopAddress, 0, 1, false);
                OstrichWriteSpecific(LoopBreak, ROMTrapLoop2BreakByte, 0, 1, false);
                OstrichWriteSpecific(LoopBreak, ROMTrapLoop1BreakByte, 0, 1, false);
                ROMWhichLoopEquals1 = true;
            }
            

        }

        #endregion

        #region P28 Compatibility
        private void CreateRomCallArraysBasedOnRomAddress(int ROMAddress)
        {
            int HiByte = ROMAddress >> 8;
            int LoByte = ROMAddress & 255;
            RomCallArray[1] = (byte)LoByte;
            RomCallArray[2] = (byte)HiByte;
            RomModifiedJumpArray[2] = (byte)LoByte;
            RomModifiedJumpArray[3] = (byte)HiByte;
            RomModifiedJumpArray[6] = (byte)LoByte;
            RomModifiedJumpArray[7] = (byte)HiByte;
        }

        private void LoadRomSettingsAndDebuggerCode()
        {

            switch (cmboxRomType.Text)
            {
                case "P12-P13-P14":
                    BaudRateReloadValuesToPatch = P13BaudRateReloadValues;
                    ROMSettingsAndDebuggerCodePath = Directory.GetCurrentDirectory();
                    ROMSettingsAndDebuggerCodePath += @"\P13DebuggerCode.code";
                    break;
                case "P13 Based HTS":
                    BaudRateReloadValuesToPatch = P13BaudRateReloadValues;
                    ROMSettingsAndDebuggerCodePath = Directory.GetCurrentDirectory();
                    ROMSettingsAndDebuggerCodePath += @"\P13HTSDebuggerCode.code";
                    break;
                case "ectune / HTS":
                    BaudRateReloadValuesToPatch = P28nP72nP30BaudRateReloadValues;
                    ROMSettingsAndDebuggerCodePath = Directory.GetCurrentDirectory();
                    ROMSettingsAndDebuggerCodePath += @"\HTSDebuggerCode.code";
                    break;
                case "P72":
                    BaudRateReloadValuesToPatch = P28nP72nP30BaudRateReloadValues;
                    ROMSettingsAndDebuggerCodePath = Directory.GetCurrentDirectory();
                    ROMSettingsAndDebuggerCodePath += @"\P72DebuggerCode.code";
                    break;
                case "P30":
                    BaudRateReloadValuesToPatch = P28nP72nP30BaudRateReloadValues;
                    ROMSettingsAndDebuggerCodePath = Directory.GetCurrentDirectory();
                    ROMSettingsAndDebuggerCodePath += @"\P30DebuggerCode.code";
                    break;
                case "Custom ROM":
                    ROMSettingsAndDebuggerCodePath = Directory.GetCurrentDirectory();
                    ROMSettingsAndDebuggerCodePath += @"\CustomDebuggerCode.code";
                    break;
                default:
                    MessageBox.Show("Check that you have selected a valid ROM Type");
                    return;
                   
            }

            if(ROMSettingsAndDebuggerCodePath != null)
            {
                try
                {
                    ROMSettingsAndDebuggerCodeArray = System.IO.File.ReadAllBytes(ROMSettingsAndDebuggerCodePath);
                    SetDefaultDebuggerAddressUsingRomValue();
                    GetBaudRatePatchAddressesFromRom();
                    GetDebuggerCodeOffsetsFromRom();

                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message + "\r\n There is a problem with " + ROMSettingsAndDebuggerCodePath);
                    return;
                }

            }
            else
            {
                MessageBox.Show("Please select a valid ROM type.");
            }

            


        }

        private void GetBaudRatePatchAddressesFromRom()
        {
            if(ROMSettingsAndDebuggerCodeArray[7]  != 112)
            {
                int bytecount = 0;
                foreach(byte element in BaudRateReloadValuesToPatch)
                {
                    BaudRateReloadValuesToPatch[bytecount] = ROMSettingsAndDebuggerCodeArray[7];
                    bytecount++;
                }
                
            }

            BaudRateAddress1 = (ROMSettingsAndDebuggerCodeArray[3] << 8) + (ROMSettingsAndDebuggerCodeArray[4]);
            BaudRateAddress2 = (ROMSettingsAndDebuggerCodeArray[5] << 8) + (ROMSettingsAndDebuggerCodeArray[6]);
        }


        private void SetDefaultDebuggerAddressUsingRomValue()
        {
            byte HiByte = ROMSettingsAndDebuggerCodeArray[1];
            byte LoByte = ROMSettingsAndDebuggerCodeArray[2];
            int DefaultDebuggerAddress = (HiByte << 8) + LoByte;
            nudROMCodeAddress.Value = DefaultDebuggerAddress;
        }

        private void GetDebuggerCodeOffsetsFromRom()
        {
            ROMOffsetWhichLoop = ROMSettingsAndDebuggerCodeArray[8];
            ROMOffsetTrapLoop1BreakByte = ROMSettingsAndDebuggerCodeArray[9];
            ROMOffsetTrapLoop2BreakByte = ROMSettingsAndDebuggerCodeArray[10];
            DebuggerLRBOffsetFromRom = ROMSettingsAndDebuggerCodeArray[11];
            DebuggerPSWOffsetFromRom = ROMSettingsAndDebuggerCodeArray[12];
            DebuggerIEOffsetFromRom = ROMSettingsAndDebuggerCodeArray[13];
        }

        private void AddConfigPatchesToRom()
        {
            byte NumberOfConfigBytes = ROMSettingsAndDebuggerCodeArray[0];
            int ConfigByteCount = 14;
            
            while (ConfigByteCount < NumberOfConfigBytes)
            {
                byte PatchAddressHiByte = ROMSettingsAndDebuggerCodeArray[ConfigByteCount];
                byte PatchAddressLoByte = ROMSettingsAndDebuggerCodeArray[ConfigByteCount + 1];
                int PatchByte = ConfigByteCount + 2;
                int PatchAddress = (PatchAddressHiByte << 8) + PatchAddressLoByte;
                Array.Copy(ROMSettingsAndDebuggerCodeArray, PatchByte, BinWithPatchesAndDebuggerCode, PatchAddress,1);
                ConfigByteCount = ConfigByteCount + 3;
            }
        }

       
        private void AddManuallyEnteredDebuggerValuesToROMCodeArray()
        {
            if (cboxDebuggerLRB.Checked)
            {
                int LRBValue = Convert.ToInt32(nudDebuggerLRB.Value);
                byte HiByte = (byte)(LRBValue >> 8);
                byte LoByte = (byte)(LRBValue & 255);
                ROMSettingsAndDebuggerCodeArray[DebuggerLRBOffsetFromRom] = LoByte;
                ROMSettingsAndDebuggerCodeArray[DebuggerLRBOffsetFromRom + 1] = HiByte;
            }
            if (cboxDebuggerPSW.Checked)
            {
                int PSWValue = Convert.ToInt32(nudDebuggerPSW.Value);
                byte HiByte = (byte)(PSWValue >> 8);
                byte LoByte = (byte)(PSWValue & 255);
                ROMSettingsAndDebuggerCodeArray[DebuggerPSWOffsetFromRom] = LoByte;
                ROMSettingsAndDebuggerCodeArray[DebuggerPSWOffsetFromRom + 1] = HiByte;
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
        
        private void AddDebuggerCodeToRom()
        {
            try
            {
                AddManuallyEnteredDebuggerValuesToROMCodeArray();
                Array.Copy(ROMSettingsAndDebuggerCodeArray, ROMSettingsAndDebuggerCodeArray[0],
                BinWithPatchesAndDebuggerCode, Convert.ToInt32(nudROMCodeAddress.Value),
                ROMSettingsAndDebuggerCodeArray.Length - ROMSettingsAndDebuggerCodeArray[0]);
                
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message + "\r\n \r\n Be sure the BIN selected is a 32kb Honda ECU BIN" );
            }
        }

        #endregion


        private void btnSetBreakpoint_Click(object sender, EventArgs e)
        {
                OstrichSetBreakpoint();
        }




        private void btnRemoveBreakPoint_Click(object sender, EventArgs e)
        {
                tboxECUReturnAddress.Text = "";
          
                OstrichRestoreArrayAndRestoreDistantROMJumps(true);
                cboxAutoStep.Checked = false;
                 cboxLockBP.Checked = false;


        }


        private void btnStepFwd_Click(object sender, EventArgs e)
        {
           
                if (cboxLockBP.Checked)
                {
                    if (cboxAutoStep.Checked)
                    {
                        timer1.Enabled = true;
                    }
                    OstrichRestoreArrayAndRestoreDistantROMJumps(true);
                    BreakLoopUsingRomAddress();
                    OstrichSetBreakpoint();

                }
                else
                {
                    BreakLoopUsingRomAddress();
                }


        }
        
    

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

       
        OpenFileDialog OPENBINDialog = new OpenFileDialog();
        bool BINOpened = false;
        private void btnLoadBinFile_Click(object sender, EventArgs e)
        {
            OPENBINDialog.Filter = "BIN Files | *.bin";
            if (OPENBINDialog.ShowDialog() == DialogResult.OK)
            {
                BINOpened = true;
                CreateNewBinArraysFromOpenBINDialogFile();
                if (cboxAutoUploadOnBINLoad.Checked)
                {
                    PrepareBinAndUploadToOstrich();
                }
                    
            }
        }


        
        private void tboxECUReturnAddress_TextChanged(object sender, EventArgs e)
        {
            if(tboxECUReturnAddress.Text != "")
            {

                if (cboxLiveEngine.Checked)
                {
                    int BP = Convert.ToInt32(nudBPAddress.Value);
                    ParseASMArrayLineAndHighlightText(tboxECUReturnAddress.Text);
                    OstrichWriteSpecific(BinWithPatchesAndDebuggerCode, BP, BP, (byte)RomCallArray.Length,true);
                    BreakLoopUsingRomAddress();
                    
                }
                else
                {
                    ParseASMArrayLineAndHighlightText(tboxECUReturnAddress.Text);
                    OstrichRestoreArrayAndRestoreDistantROMJumps(false);
                    OstrichWriteJumpsToRom();
                }

              
            }
               

            
        }


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
                if (cboxLockBP.Checked)
                {
                    OstrichRestoreArrayAndRestoreDistantROMJumps(true);
                    BreakLoopUsingRomAddress();
                    OstrichSetBreakpoint();

                }
                else
                {
                    BreakLoopUsingRomAddress();
                }

                if (timer1.Enabled)
                {
                    timer1.Enabled = false;
                    timer1.Enabled = true;
                }

                PreviousPacketNumber = intPacketsRecd;
            }


        }

        private void cboxAutoStep_CheckedChanged(object sender, EventArgs e)
        {

            if (!cboxAutoStep.Checked)
            {
                timer1.Enabled = false;
            }
        }



        

        private void cmboxRomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRomSettingsAndDebuggerCode();
        }

        private void cboxStepInto_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxStepInto.Checked && intCallAddress > 0)
            {
                OstrichWriteSpecific(RomCallArray, intCallAddress);
            }
            else if(!cboxStepInto.Checked && intCallAddress > 0)
            {
                OstrichWriteSpecific(BinWithPatchesAndDebuggerCode, intCallAddress, intCallAddress,(byte)RomCallArray.Length);
            }
        }

        private void CBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InitialOstrichUploadComplete)
            {
                MessageBox.Show("If changes are made to the baud rate, you must re-upload the bin to the rom, and power cycle the ECU.");
            }
            
        }

        private void cboxAutoDasmBin_CheckedChanged(object sender, EventArgs e)
        {
            if (!cboxAutoDasmBin.Checked)
            {
                btnLoadASMFile.Enabled = true;
            }
            else
            {
                btnLoadASMFile.Enabled = false;
            }
        }

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

        private void tboxHexToSend_MouseDown(object sender, MouseEventArgs e)
        {
            tboxHexToSend.Text = "";
        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void btnReloadDebuggerCode_Click(object sender, EventArgs e)
        {
            LoadRomSettingsAndDebuggerCode();
            AddManuallyEnteredDebuggerValuesToROMCodeArray();
            OstrichWriteSpecific(ROMSettingsAndDebuggerCodeArray, Convert.ToInt32(nudROMCodeAddress.Value), ROMSettingsAndDebuggerCodeArray[0], (byte)(ROMSettingsAndDebuggerCodeArray.Length - ROMSettingsAndDebuggerCodeArray[0]), true);
        }

        private void btnResetOstrich_Click(object sender, EventArgs e)
        {
            ResetOstrichVID();
        }





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

        private void tboxX1_TextChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(tboxX1, GetPointerValuesFromPointerTextBox(tboxX1, "X1"));
        }

        private void tboxX2_TextChanged(object sender, EventArgs e)
        {
            toolTip2.SetToolTip(tboxX2, GetPointerValuesFromPointerTextBox(tboxX2, "X2"));
        }

        private void tboxDP_TextChanged(object sender, EventArgs e)
        {
            toolTip3.SetToolTip(tboxDP, GetPointerValuesFromPointerTextBox(tboxDP, "DP"));
        }

        private void cboxLiveEngine_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxLiveEngine.Checked)
            {
                cboxAutoStep.Checked = false;
                cboxLockBP.Checked = false;
                cboxStepInto.Checked = false;
                cboxAutoStep.Enabled = false;
                cboxLockBP.Enabled = false;
                cboxStepInto.Enabled = false;
                btnRemoveBreakPoint.Enabled = false;
                btnStepFwd.Enabled = false;
            }
            else
            {
                cboxAutoStep.Enabled = true;
                cboxLockBP.Enabled = true;
                cboxStepInto.Enabled = true;
                btnRemoveBreakPoint.Enabled = true;
                btnStepFwd.Enabled = true;
            }
        }
    }
}
