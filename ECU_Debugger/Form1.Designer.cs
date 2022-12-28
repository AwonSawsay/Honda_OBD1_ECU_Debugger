namespace ECU_Debugger
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tboxHexToSend = new System.Windows.Forms.TextBox();
            this.btnSendByte = new System.Windows.Forms.Button();
            this.btnCloseCOMPort = new System.Windows.Forms.Button();
            this.btnOpenCOMPort = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cBoxParityBits = new System.Windows.Forms.ComboBox();
            this.cBoxStopBits = new System.Windows.Forms.ComboBox();
            this.cBoxDataBits = new System.Windows.Forms.ComboBox();
            this.CBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.cBoxCOMPORT = new System.Windows.Forms.ComboBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsOpenRenamingForm = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.btnRefreshDasm = new System.Windows.Forms.Button();
            this.label43 = new System.Windows.Forms.Label();
            this.tboxDasmArgForce = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.tboxDasmArgIgnore = new System.Windows.Forms.TextBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.lblVCALnumber = new System.Windows.Forms.Label();
            this.cboxVCALReRoute = new System.Windows.Forms.CheckBox();
            this.lblVCALbyteSize = new System.Windows.Forms.Label();
            this.lblSizeWithVCAL = new System.Windows.Forms.Label();
            this.lblDebuggerCodeSize = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.cmboxRomType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nudROMCodeAddress = new System.Windows.Forms.NumericUpDown();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.tboxLA1_2 = new System.Windows.Forms.TextBox();
            this.tboxLA1_1 = new System.Windows.Forms.TextBox();
            this.lblRamWatchBinary = new System.Windows.Forms.Label();
            this.lblLA11 = new System.Windows.Forms.Label();
            this.lblRamWatch = new System.Windows.Forms.Label();
            this.lblLA12 = new System.Windows.Forms.Label();
            this.tboxRamWatch = new System.Windows.Forms.TextBox();
            this.lblXram1Binary = new System.Windows.Forms.Label();
            this.lblXram2Binary = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.tboxStackPointer = new System.Windows.Forms.TextBox();
            this.tboxStack1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tboxStack2 = new System.Windows.Forms.TextBox();
            this.tboxStack3 = new System.Windows.Forms.TextBox();
            this.tboxStack4 = new System.Windows.Forms.TextBox();
            this.tboxStack5 = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.btnReloadDebuggerCode = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cboxDebuggerLRB = new System.Windows.Forms.CheckBox();
            this.cboxDebuggerIE = new System.Windows.Forms.CheckBox();
            this.cboxDebuggerPSW = new System.Windows.Forms.CheckBox();
            this.nudDebuggerLRB = new System.Windows.Forms.NumericUpDown();
            this.nudDebuggerIE = new System.Windows.Forms.NumericUpDown();
            this.nudDebuggerPSW = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tboxECUReturnAddress = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.tboxDPConts_ROM = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.lblDPContsBinary = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.tboxDP = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tboxDPconts = new System.Windows.Forms.TextBox();
            this.lblDPContents = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.tboxLRB = new System.Windows.Forms.TextBox();
            this.tboxACC = new System.Windows.Forms.TextBox();
            this.tboxX2 = new System.Windows.Forms.TextBox();
            this.tboxR0 = new System.Windows.Forms.TextBox();
            this.tboxX1 = new System.Windows.Forms.TextBox();
            this.tboxR1 = new System.Windows.Forms.TextBox();
            this.tboxR7 = new System.Windows.Forms.TextBox();
            this.tboxR2 = new System.Windows.Forms.TextBox();
            this.tboxR6 = new System.Windows.Forms.TextBox();
            this.tboxR3 = new System.Windows.Forms.TextBox();
            this.tboxR5 = new System.Windows.Forms.TextBox();
            this.tboxR4 = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label41 = new System.Windows.Forms.Label();
            this.tboxPSWL5 = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.tboxPSWL4 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.tboxPSWL = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tboxPSWH = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tboxSCB = new System.Windows.Forms.TextBox();
            this.tboxSF = new System.Windows.Forms.TextBox();
            this.tboxMIE = new System.Windows.Forms.TextBox();
            this.tboxBCB = new System.Windows.Forms.TextBox();
            this.tboxHC = new System.Windows.Forms.TextBox();
            this.tboxDD = new System.Windows.Forms.TextBox();
            this.tboxZF = new System.Windows.Forms.TextBox();
            this.tboxCF = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboxLockRamWatch = new System.Windows.Forms.CheckBox();
            this.label45 = new System.Windows.Forms.Label();
            this.nudRAMWatch = new System.Windows.Forms.NumericUpDown();
            this.btnFindAddress = new System.Windows.Forms.Button();
            this.cboxLiveEngine = new System.Windows.Forms.CheckBox();
            this.nudBPAddress = new System.Windows.Forms.NumericUpDown();
            this.cboxLockBP = new System.Windows.Forms.CheckBox();
            this.cboxAutoStep = new System.Windows.Forms.CheckBox();
            this.cboxStepInto = new System.Windows.Forms.CheckBox();
            this.btnStepFwd = new System.Windows.Forms.Button();
            this.btnRemoveBreakPoint = new System.Windows.Forms.Button();
            this.btnSetBreakpoint = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.lblChangedBytes = new System.Windows.Forms.Label();
            this.cboxQuietCompiler = new System.Windows.Forms.CheckBox();
            this.label46 = new System.Windows.Forms.Label();
            this.cboxMonitorBin = new System.Windows.Forms.CheckBox();
            this.cboxSlowUpload = new System.Windows.Forms.CheckBox();
            this.cboxAutoUploadOnBINLoad = new System.Windows.Forms.CheckBox();
            this.btnLoadBinToOstrich = new System.Windows.Forms.Button();
            this.btnLoadASMFile = new System.Windows.Forms.Button();
            this.tboxOpenFileSimpleName = new System.Windows.Forms.TextBox();
            this.btnLoadBinFile = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnResetOstrich = new System.Windows.Forms.Button();
            this.nudVIDNumber = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.tboxOstrichSerialNumber = new System.Windows.Forms.TextBox();
            this.btnCloseOstrichCom = new System.Windows.Forms.Button();
            this.label36 = new System.Windows.Forms.Label();
            this.btnOpenOstrichCom = new System.Windows.Forms.Button();
            this.cboxOstrichComPortNumber = new System.Windows.Forms.ComboBox();
            this.rtboxASMFile = new System.Windows.Forms.RichTextBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.cboxRenameOnChange = new System.Windows.Forms.CheckBox();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.btnSaveRenamedASM = new System.Windows.Forms.Button();
            this.btnSwap = new System.Windows.Forms.Button();
            this.btnOpenXMLFile = new System.Windows.Forms.Button();
            this.btnApplyRenamingMask = new System.Windows.Forms.Button();
            this.cboxSaveTrace = new System.Windows.Forms.CheckBox();
            this.tboxTraceWindow = new System.Windows.Forms.TextBox();
            this.btnClearText = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.binMonitorTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudROMCodeAddress)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDebuggerLRB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDebuggerIE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDebuggerPSW)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRAMWatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBPAddress)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVIDNumber)).BeginInit();
            this.groupBox12.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tboxHexToSend);
            this.groupBox1.Controls.Add(this.btnSendByte);
            this.groupBox1.Controls.Add(this.btnCloseCOMPort);
            this.groupBox1.Controls.Add(this.btnOpenCOMPort);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cBoxParityBits);
            this.groupBox1.Controls.Add(this.cBoxStopBits);
            this.groupBox1.Controls.Add(this.cBoxDataBits);
            this.groupBox1.Controls.Add(this.CBoxBaudRate);
            this.groupBox1.Controls.Add(this.cBoxCOMPORT);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 246);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ECU Serial";
            // 
            // tboxHexToSend
            // 
            this.tboxHexToSend.Location = new System.Drawing.Point(12, 194);
            this.tboxHexToSend.Name = "tboxHexToSend";
            this.tboxHexToSend.Size = new System.Drawing.Size(165, 20);
            this.tboxHexToSend.TabIndex = 26;
            this.tboxHexToSend.Text = "Enter hex to send , no spaces";
            this.tboxHexToSend.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tboxHexToSend_MouseDown);
            // 
            // btnSendByte
            // 
            this.btnSendByte.Location = new System.Drawing.Point(114, 217);
            this.btnSendByte.Name = "btnSendByte";
            this.btnSendByte.Size = new System.Drawing.Size(63, 23);
            this.btnSendByte.TabIndex = 25;
            this.btnSendByte.Text = "Send";
            this.btnSendByte.UseVisualStyleBackColor = true;
            this.btnSendByte.Click += new System.EventHandler(this.btnSendByte_Click);
            // 
            // btnCloseCOMPort
            // 
            this.btnCloseCOMPort.Location = new System.Drawing.Point(102, 169);
            this.btnCloseCOMPort.Name = "btnCloseCOMPort";
            this.btnCloseCOMPort.Size = new System.Drawing.Size(75, 21);
            this.btnCloseCOMPort.TabIndex = 13;
            this.btnCloseCOMPort.Text = "CLOSE";
            this.btnCloseCOMPort.UseVisualStyleBackColor = true;
            this.btnCloseCOMPort.Click += new System.EventHandler(this.btnCloseCOMPort_Click);
            // 
            // btnOpenCOMPort
            // 
            this.btnOpenCOMPort.Location = new System.Drawing.Point(12, 169);
            this.btnOpenCOMPort.Name = "btnOpenCOMPort";
            this.btnOpenCOMPort.Size = new System.Drawing.Size(75, 21);
            this.btnOpenCOMPort.TabIndex = 12;
            this.btnOpenCOMPort.Text = "OPEN";
            this.btnOpenCOMPort.UseVisualStyleBackColor = true;
            this.btnOpenCOMPort.Click += new System.EventHandler(this.btnOpenCOMPort_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 155);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(164, 10);
            this.progressBar1.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "PARITY BITS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "STOP BITS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "DATA BITS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "BAUD RATE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "COM PORT";
            // 
            // cBoxParityBits
            // 
            this.cBoxParityBits.FormattingEnabled = true;
            this.cBoxParityBits.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even"});
            this.cBoxParityBits.Location = new System.Drawing.Point(88, 127);
            this.cBoxParityBits.Name = "cBoxParityBits";
            this.cBoxParityBits.Size = new System.Drawing.Size(99, 21);
            this.cBoxParityBits.TabIndex = 4;
            this.cBoxParityBits.Text = "None";
            // 
            // cBoxStopBits
            // 
            this.cBoxStopBits.FormattingEnabled = true;
            this.cBoxStopBits.Items.AddRange(new object[] {
            "One",
            "Two"});
            this.cBoxStopBits.Location = new System.Drawing.Point(88, 100);
            this.cBoxStopBits.Name = "cBoxStopBits";
            this.cBoxStopBits.Size = new System.Drawing.Size(99, 21);
            this.cBoxStopBits.TabIndex = 3;
            this.cBoxStopBits.Text = "One";
            // 
            // cBoxDataBits
            // 
            this.cBoxDataBits.FormattingEnabled = true;
            this.cBoxDataBits.Items.AddRange(new object[] {
            "6",
            "7",
            "8"});
            this.cBoxDataBits.Location = new System.Drawing.Point(88, 73);
            this.cBoxDataBits.Name = "cBoxDataBits";
            this.cBoxDataBits.Size = new System.Drawing.Size(99, 21);
            this.cBoxDataBits.TabIndex = 2;
            this.cBoxDataBits.Text = "8";
            // 
            // CBoxBaudRate
            // 
            this.CBoxBaudRate.FormattingEnabled = true;
            this.CBoxBaudRate.Items.AddRange(new object[] {
            "38400",
            "62500",
            "156250",
            "312500"});
            this.CBoxBaudRate.Location = new System.Drawing.Point(88, 46);
            this.CBoxBaudRate.Name = "CBoxBaudRate";
            this.CBoxBaudRate.Size = new System.Drawing.Size(99, 21);
            this.CBoxBaudRate.TabIndex = 1;
            this.CBoxBaudRate.Text = "39063";
            // 
            // cBoxCOMPORT
            // 
            this.cBoxCOMPORT.FormattingEnabled = true;
            this.cBoxCOMPORT.Location = new System.Drawing.Point(88, 19);
            this.cBoxCOMPORT.Name = "cBoxCOMPORT";
            this.cBoxCOMPORT.Size = new System.Drawing.Size(99, 21);
            this.cBoxCOMPORT.TabIndex = 0;
            this.cBoxCOMPORT.Text = "COM8";
            this.cBoxCOMPORT.DropDown += new System.EventHandler(this.cBoxCOMPORT_DropDown);
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 38400;
            this.serialPort1.ReadBufferSize = 200;
            this.serialPort1.ReceivedBytesThreshold = 28;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1334, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.tsOpenRenamingForm,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // tsOpenRenamingForm
            // 
            this.tsOpenRenamingForm.Name = "tsOpenRenamingForm";
            this.tsOpenRenamingForm.Size = new System.Drawing.Size(163, 22);
            this.tsOpenRenamingForm.Text = "ASM Renaming";
            this.tsOpenRenamingForm.Click += new System.EventHandler(this.tsOpenRenamingForm_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox11);
            this.panel1.Controls.Add(this.groupBox9);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.groupBox8);
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.rtboxASMFile);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox12);
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1322, 787);
            this.panel1.TabIndex = 8;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.btnRefreshDasm);
            this.groupBox11.Controls.Add(this.label43);
            this.groupBox11.Controls.Add(this.tboxDasmArgForce);
            this.groupBox11.Controls.Add(this.label42);
            this.groupBox11.Controls.Add(this.tboxDasmArgIgnore);
            this.groupBox11.Location = new System.Drawing.Point(3, 733);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(923, 36);
            this.groupBox11.TabIndex = 41;
            this.groupBox11.TabStop = false;
            // 
            // btnRefreshDasm
            // 
            this.btnRefreshDasm.Location = new System.Drawing.Point(753, 10);
            this.btnRefreshDasm.Name = "btnRefreshDasm";
            this.btnRefreshDasm.Size = new System.Drawing.Size(161, 23);
            this.btnRefreshDasm.TabIndex = 39;
            this.btnRefreshDasm.Text = "Refresh Dasm with Args";
            this.btnRefreshDasm.UseVisualStyleBackColor = true;
            this.btnRefreshDasm.Click += new System.EventHandler(this.refreshDasm_Click);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(353, 15);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(116, 13);
            this.label43.TabIndex = 39;
            this.label43.Text = "Force Dasm Addresses";
            // 
            // tboxDasmArgForce
            // 
            this.tboxDasmArgForce.Location = new System.Drawing.Point(475, 12);
            this.tboxDasmArgForce.Name = "tboxDasmArgForce";
            this.tboxDasmArgForce.Size = new System.Drawing.Size(201, 20);
            this.tboxDasmArgForce.TabIndex = 40;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(15, 15);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(119, 13);
            this.label42.TabIndex = 36;
            this.label42.Text = "Ignore Dasm Addresses";
            // 
            // tboxDasmArgIgnore
            // 
            this.tboxDasmArgIgnore.Location = new System.Drawing.Point(138, 12);
            this.tboxDasmArgIgnore.Name = "tboxDasmArgIgnore";
            this.tboxDasmArgIgnore.Size = new System.Drawing.Size(201, 20);
            this.tboxDasmArgIgnore.TabIndex = 38;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.lblVCALnumber);
            this.groupBox9.Controls.Add(this.cboxVCALReRoute);
            this.groupBox9.Controls.Add(this.lblVCALbyteSize);
            this.groupBox9.Controls.Add(this.lblSizeWithVCAL);
            this.groupBox9.Controls.Add(this.lblDebuggerCodeSize);
            this.groupBox9.Controls.Add(this.label50);
            this.groupBox9.Controls.Add(this.cmboxRomType);
            this.groupBox9.Controls.Add(this.label6);
            this.groupBox9.Controls.Add(this.nudROMCodeAddress);
            this.groupBox9.Location = new System.Drawing.Point(3, 385);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(199, 145);
            this.groupBox9.TabIndex = 34;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "ECU type";
            // 
            // lblVCALnumber
            // 
            this.lblVCALnumber.AutoSize = true;
            this.lblVCALnumber.Location = new System.Drawing.Point(135, 41);
            this.lblVCALnumber.Name = "lblVCALnumber";
            this.lblVCALnumber.Size = new System.Drawing.Size(50, 13);
            this.lblVCALnumber.TabIndex = 41;
            this.lblVCALnumber.Text = "VCAL:  #";
            // 
            // cboxVCALReRoute
            // 
            this.cboxVCALReRoute.AutoSize = true;
            this.cboxVCALReRoute.Checked = true;
            this.cboxVCALReRoute.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxVCALReRoute.Location = new System.Drawing.Point(6, 40);
            this.cboxVCALReRoute.Name = "cboxVCALReRoute";
            this.cboxVCALReRoute.Size = new System.Drawing.Size(124, 17);
            this.cboxVCALReRoute.TabIndex = 40;
            this.cboxVCALReRoute.Text = "Use VCAL Re-Route";
            this.cboxVCALReRoute.UseVisualStyleBackColor = true;
            this.cboxVCALReRoute.CheckedChanged += new System.EventHandler(this.cboxVCALReRoute_CheckedChanged);
            // 
            // lblVCALbyteSize
            // 
            this.lblVCALbyteSize.AutoSize = true;
            this.lblVCALbyteSize.Location = new System.Drawing.Point(119, 121);
            this.lblVCALbyteSize.Name = "lblVCALbyteSize";
            this.lblVCALbyteSize.Size = new System.Drawing.Size(42, 13);
            this.lblVCALbyteSize.TabIndex = 39;
            this.lblVCALbyteSize.Text = "0 Bytes";
            // 
            // lblSizeWithVCAL
            // 
            this.lblSizeWithVCAL.AutoSize = true;
            this.lblSizeWithVCAL.Location = new System.Drawing.Point(26, 121);
            this.lblSizeWithVCAL.Name = "lblSizeWithVCAL";
            this.lblSizeWithVCAL.Size = new System.Drawing.Size(88, 13);
            this.lblSizeWithVCAL.TabIndex = 38;
            this.lblSizeWithVCAL.Text = "Size using VCAL:";
            // 
            // lblDebuggerCodeSize
            // 
            this.lblDebuggerCodeSize.AutoSize = true;
            this.lblDebuggerCodeSize.Location = new System.Drawing.Point(119, 103);
            this.lblDebuggerCodeSize.Name = "lblDebuggerCodeSize";
            this.lblDebuggerCodeSize.Size = new System.Drawing.Size(42, 13);
            this.lblDebuggerCodeSize.TabIndex = 37;
            this.lblDebuggerCodeSize.Text = "0 Bytes";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(7, 103);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(108, 13);
            this.label50.TabIndex = 36;
            this.label50.Text = "Debugger Code Size:";
            // 
            // cmboxRomType
            // 
            this.cmboxRomType.FormattingEnabled = true;
            this.cmboxRomType.Items.AddRange(new object[] {
            "P12-P13-P14",
            "P13 Based HTS",
            "ectune / HTS",
            "P72",
            "P30",
            "Custom ROM"});
            this.cmboxRomType.Location = new System.Drawing.Point(6, 13);
            this.cmboxRomType.Name = "cmboxRomType";
            this.cmboxRomType.Size = new System.Drawing.Size(158, 21);
            this.cmboxRomType.TabIndex = 27;
            this.cmboxRomType.Text = "Select ROM Type";
            this.cmboxRomType.SelectedIndexChanged += new System.EventHandler(this.cmboxRomType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(151, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Debugger Code ROM Address";
            // 
            // nudROMCodeAddress
            // 
            this.nudROMCodeAddress.Hexadecimal = true;
            this.nudROMCodeAddress.Location = new System.Drawing.Point(35, 75);
            this.nudROMCodeAddress.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.nudROMCodeAddress.Name = "nudROMCodeAddress";
            this.nudROMCodeAddress.Size = new System.Drawing.Size(92, 20);
            this.nudROMCodeAddress.TabIndex = 34;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox14);
            this.groupBox6.Controls.Add(this.groupBox13);
            this.groupBox6.Controls.Add(this.groupBox10);
            this.groupBox6.Controls.Add(this.groupBox3);
            this.groupBox6.Controls.Add(this.groupBox5);
            this.groupBox6.Controls.Add(this.groupBox4);
            this.groupBox6.Controls.Add(this.groupBox2);
            this.groupBox6.Location = new System.Drawing.Point(932, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(375, 723);
            this.groupBox6.TabIndex = 17;
            this.groupBox6.TabStop = false;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.tboxLA1_2);
            this.groupBox14.Controls.Add(this.tboxLA1_1);
            this.groupBox14.Controls.Add(this.lblRamWatchBinary);
            this.groupBox14.Controls.Add(this.lblLA11);
            this.groupBox14.Controls.Add(this.lblRamWatch);
            this.groupBox14.Controls.Add(this.lblLA12);
            this.groupBox14.Controls.Add(this.tboxRamWatch);
            this.groupBox14.Controls.Add(this.lblXram1Binary);
            this.groupBox14.Controls.Add(this.lblXram2Binary);
            this.groupBox14.Location = new System.Drawing.Point(155, 199);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(102, 220);
            this.groupBox14.TabIndex = 61;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "RAM";
            // 
            // tboxLA1_2
            // 
            this.tboxLA1_2.Location = new System.Drawing.Point(32, 104);
            this.tboxLA1_2.Name = "tboxLA1_2";
            this.tboxLA1_2.ReadOnly = true;
            this.tboxLA1_2.Size = new System.Drawing.Size(42, 20);
            this.tboxLA1_2.TabIndex = 45;
            // 
            // tboxLA1_1
            // 
            this.tboxLA1_1.Location = new System.Drawing.Point(32, 39);
            this.tboxLA1_1.Name = "tboxLA1_1";
            this.tboxLA1_1.ReadOnly = true;
            this.tboxLA1_1.Size = new System.Drawing.Size(42, 20);
            this.tboxLA1_1.TabIndex = 43;
            // 
            // lblRamWatchBinary
            // 
            this.lblRamWatchBinary.AutoSize = true;
            this.lblRamWatchBinary.Location = new System.Drawing.Point(24, 193);
            this.lblRamWatchBinary.Name = "lblRamWatchBinary";
            this.lblRamWatchBinary.Size = new System.Drawing.Size(58, 13);
            this.lblRamWatchBinary.TabIndex = 59;
            this.lblRamWatchBinary.Text = "1111 0000";
            // 
            // lblLA11
            // 
            this.lblLA11.AutoSize = true;
            this.lblLA11.Location = new System.Drawing.Point(32, 22);
            this.lblLA11.Name = "lblLA11";
            this.lblLA11.Size = new System.Drawing.Size(40, 13);
            this.lblLA11.TabIndex = 44;
            this.lblLA11.Text = "RAM 1";
            // 
            // lblRamWatch
            // 
            this.lblRamWatch.AutoSize = true;
            this.lblRamWatch.Location = new System.Drawing.Point(29, 153);
            this.lblRamWatch.Name = "lblRamWatch";
            this.lblRamWatch.Size = new System.Drawing.Size(66, 13);
            this.lblRamWatch.TabIndex = 58;
            this.lblRamWatch.Text = "RAM Watch";
            // 
            // lblLA12
            // 
            this.lblLA12.AutoSize = true;
            this.lblLA12.Location = new System.Drawing.Point(31, 87);
            this.lblLA12.Name = "lblLA12";
            this.lblLA12.Size = new System.Drawing.Size(40, 13);
            this.lblLA12.TabIndex = 46;
            this.lblLA12.Text = "RAM 2";
            // 
            // tboxRamWatch
            // 
            this.tboxRamWatch.Location = new System.Drawing.Point(32, 170);
            this.tboxRamWatch.Name = "tboxRamWatch";
            this.tboxRamWatch.ReadOnly = true;
            this.tboxRamWatch.Size = new System.Drawing.Size(42, 20);
            this.tboxRamWatch.TabIndex = 57;
            this.tboxRamWatch.TextChanged += new System.EventHandler(this.tboxRamWatch_TextChanged);
            // 
            // lblXram1Binary
            // 
            this.lblXram1Binary.AutoSize = true;
            this.lblXram1Binary.Location = new System.Drawing.Point(24, 62);
            this.lblXram1Binary.Name = "lblXram1Binary";
            this.lblXram1Binary.Size = new System.Drawing.Size(58, 13);
            this.lblXram1Binary.TabIndex = 54;
            this.lblXram1Binary.Text = "1111 0000";
            // 
            // lblXram2Binary
            // 
            this.lblXram2Binary.AutoSize = true;
            this.lblXram2Binary.Location = new System.Drawing.Point(24, 127);
            this.lblXram2Binary.Name = "lblXram2Binary";
            this.lblXram2Binary.Size = new System.Drawing.Size(58, 13);
            this.lblXram2Binary.TabIndex = 55;
            this.lblXram2Binary.Text = "1111 0000";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.tboxStackPointer);
            this.groupBox13.Controls.Add(this.tboxStack1);
            this.groupBox13.Controls.Add(this.label7);
            this.groupBox13.Controls.Add(this.tboxStack2);
            this.groupBox13.Controls.Add(this.tboxStack3);
            this.groupBox13.Controls.Add(this.tboxStack4);
            this.groupBox13.Controls.Add(this.tboxStack5);
            this.groupBox13.Controls.Add(this.label44);
            this.groupBox13.Location = new System.Drawing.Point(263, 198);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(106, 220);
            this.groupBox13.TabIndex = 60;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Stack";
            // 
            // tboxStackPointer
            // 
            this.tboxStackPointer.Location = new System.Drawing.Point(18, 40);
            this.tboxStackPointer.Name = "tboxStackPointer";
            this.tboxStackPointer.ReadOnly = true;
            this.tboxStackPointer.Size = new System.Drawing.Size(42, 20);
            this.tboxStackPointer.TabIndex = 41;
            // 
            // tboxStack1
            // 
            this.tboxStack1.Location = new System.Drawing.Point(18, 85);
            this.tboxStack1.Name = "tboxStack1";
            this.tboxStack1.ReadOnly = true;
            this.tboxStack1.Size = new System.Drawing.Size(42, 20);
            this.tboxStack1.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Contents";
            // 
            // tboxStack2
            // 
            this.tboxStack2.Location = new System.Drawing.Point(18, 111);
            this.tboxStack2.Name = "tboxStack2";
            this.tboxStack2.ReadOnly = true;
            this.tboxStack2.Size = new System.Drawing.Size(42, 20);
            this.tboxStack2.TabIndex = 37;
            // 
            // tboxStack3
            // 
            this.tboxStack3.Location = new System.Drawing.Point(18, 136);
            this.tboxStack3.Name = "tboxStack3";
            this.tboxStack3.ReadOnly = true;
            this.tboxStack3.Size = new System.Drawing.Size(42, 20);
            this.tboxStack3.TabIndex = 38;
            // 
            // tboxStack4
            // 
            this.tboxStack4.Location = new System.Drawing.Point(18, 162);
            this.tboxStack4.Name = "tboxStack4";
            this.tboxStack4.ReadOnly = true;
            this.tboxStack4.Size = new System.Drawing.Size(42, 20);
            this.tboxStack4.TabIndex = 39;
            // 
            // tboxStack5
            // 
            this.tboxStack5.Location = new System.Drawing.Point(18, 188);
            this.tboxStack5.Name = "tboxStack5";
            this.tboxStack5.ReadOnly = true;
            this.tboxStack5.Size = new System.Drawing.Size(44, 20);
            this.tboxStack5.TabIndex = 40;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(24, 24);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(28, 13);
            this.label44.TabIndex = 42;
            this.label44.Text = "SSP";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.btnReloadDebuggerCode);
            this.groupBox10.Controls.Add(this.label8);
            this.groupBox10.Controls.Add(this.cboxDebuggerLRB);
            this.groupBox10.Controls.Add(this.cboxDebuggerIE);
            this.groupBox10.Controls.Add(this.cboxDebuggerPSW);
            this.groupBox10.Controls.Add(this.nudDebuggerLRB);
            this.groupBox10.Controls.Add(this.nudDebuggerIE);
            this.groupBox10.Controls.Add(this.nudDebuggerPSW);
            this.groupBox10.Location = new System.Drawing.Point(6, 569);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(363, 143);
            this.groupBox10.TabIndex = 32;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Advanced ROM Debugger Options";
            // 
            // btnReloadDebuggerCode
            // 
            this.btnReloadDebuggerCode.Location = new System.Drawing.Point(90, 94);
            this.btnReloadDebuggerCode.Name = "btnReloadDebuggerCode";
            this.btnReloadDebuggerCode.Size = new System.Drawing.Size(161, 23);
            this.btnReloadDebuggerCode.TabIndex = 38;
            this.btnReloadDebuggerCode.Text = "Reload Debugger Code";
            this.btnReloadDebuggerCode.UseVisualStyleBackColor = true;
            this.btnReloadDebuggerCode.Click += new System.EventHandler(this.btnReloadDebuggerCode_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(70, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(224, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "Reload Debugger Code after making changes";
            // 
            // cboxDebuggerLRB
            // 
            this.cboxDebuggerLRB.AutoSize = true;
            this.cboxDebuggerLRB.Location = new System.Drawing.Point(73, 21);
            this.cboxDebuggerLRB.Name = "cboxDebuggerLRB";
            this.cboxDebuggerLRB.Size = new System.Drawing.Size(97, 17);
            this.cboxDebuggerLRB.TabIndex = 36;
            this.cboxDebuggerLRB.Text = "Debugger LRB";
            this.cboxDebuggerLRB.UseVisualStyleBackColor = true;
            this.cboxDebuggerLRB.CheckedChanged += new System.EventHandler(this.cboxDebuggerLRB_CheckedChanged);
            // 
            // cboxDebuggerIE
            // 
            this.cboxDebuggerIE.AutoSize = true;
            this.cboxDebuggerIE.Location = new System.Drawing.Point(73, 71);
            this.cboxDebuggerIE.Name = "cboxDebuggerIE";
            this.cboxDebuggerIE.Size = new System.Drawing.Size(86, 17);
            this.cboxDebuggerIE.TabIndex = 35;
            this.cboxDebuggerIE.Text = "Debugger IE";
            this.cboxDebuggerIE.UseVisualStyleBackColor = true;
            this.cboxDebuggerIE.CheckedChanged += new System.EventHandler(this.cboxDebuggerIE_CheckedChanged);
            // 
            // cboxDebuggerPSW
            // 
            this.cboxDebuggerPSW.AutoSize = true;
            this.cboxDebuggerPSW.Location = new System.Drawing.Point(73, 46);
            this.cboxDebuggerPSW.Name = "cboxDebuggerPSW";
            this.cboxDebuggerPSW.Size = new System.Drawing.Size(97, 17);
            this.cboxDebuggerPSW.TabIndex = 34;
            this.cboxDebuggerPSW.Text = "Debugger SCB";
            this.cboxDebuggerPSW.UseVisualStyleBackColor = true;
            this.cboxDebuggerPSW.CheckedChanged += new System.EventHandler(this.cboxDebuggerPSW_CheckedChanged);
            // 
            // nudDebuggerLRB
            // 
            this.nudDebuggerLRB.Hexadecimal = true;
            this.nudDebuggerLRB.Location = new System.Drawing.Point(182, 20);
            this.nudDebuggerLRB.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudDebuggerLRB.Name = "nudDebuggerLRB";
            this.nudDebuggerLRB.Size = new System.Drawing.Size(92, 20);
            this.nudDebuggerLRB.TabIndex = 30;
            // 
            // nudDebuggerIE
            // 
            this.nudDebuggerIE.Hexadecimal = true;
            this.nudDebuggerIE.Location = new System.Drawing.Point(182, 71);
            this.nudDebuggerIE.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudDebuggerIE.Name = "nudDebuggerIE";
            this.nudDebuggerIE.Size = new System.Drawing.Size(92, 20);
            this.nudDebuggerIE.TabIndex = 29;
            // 
            // nudDebuggerPSW
            // 
            this.nudDebuggerPSW.Hexadecimal = true;
            this.nudDebuggerPSW.Location = new System.Drawing.Point(182, 46);
            this.nudDebuggerPSW.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudDebuggerPSW.Name = "nudDebuggerPSW";
            this.nudDebuggerPSW.Size = new System.Drawing.Size(92, 20);
            this.nudDebuggerPSW.TabIndex = 28;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox3.Controls.Add(this.tboxECUReturnAddress);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Location = new System.Drawing.Point(7, 11);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(143, 32);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            // 
            // tboxECUReturnAddress
            // 
            this.tboxECUReturnAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxECUReturnAddress.Location = new System.Drawing.Point(70, 9);
            this.tboxECUReturnAddress.Name = "tboxECUReturnAddress";
            this.tboxECUReturnAddress.ReadOnly = true;
            this.tboxECUReturnAddress.Size = new System.Drawing.Size(65, 20);
            this.tboxECUReturnAddress.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "BP Address";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label49);
            this.groupBox5.Controls.Add(this.label48);
            this.groupBox5.Controls.Add(this.tboxDPConts_ROM);
            this.groupBox5.Controls.Add(this.label47);
            this.groupBox5.Controls.Add(this.label39);
            this.groupBox5.Controls.Add(this.label35);
            this.groupBox5.Controls.Add(this.label34);
            this.groupBox5.Controls.Add(this.lblDPContsBinary);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label38);
            this.groupBox5.Controls.Add(this.tboxDP);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.tboxDPconts);
            this.groupBox5.Controls.Add(this.lblDPContents);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label25);
            this.groupBox5.Controls.Add(this.label26);
            this.groupBox5.Controls.Add(this.label27);
            this.groupBox5.Controls.Add(this.label28);
            this.groupBox5.Controls.Add(this.label29);
            this.groupBox5.Controls.Add(this.label30);
            this.groupBox5.Controls.Add(this.label31);
            this.groupBox5.Controls.Add(this.label32);
            this.groupBox5.Controls.Add(this.label33);
            this.groupBox5.Controls.Add(this.tboxLRB);
            this.groupBox5.Controls.Add(this.tboxACC);
            this.groupBox5.Controls.Add(this.tboxX2);
            this.groupBox5.Controls.Add(this.tboxR0);
            this.groupBox5.Controls.Add(this.tboxX1);
            this.groupBox5.Controls.Add(this.tboxR1);
            this.groupBox5.Controls.Add(this.tboxR7);
            this.groupBox5.Controls.Add(this.tboxR2);
            this.groupBox5.Controls.Add(this.tboxR6);
            this.groupBox5.Controls.Add(this.tboxR3);
            this.groupBox5.Controls.Add(this.tboxR5);
            this.groupBox5.Controls.Add(this.tboxR4);
            this.groupBox5.Location = new System.Drawing.Point(6, 49);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(143, 369);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Registers";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(104, 308);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(38, 13);
            this.label49.TabIndex = 59;
            this.label49.Text = "(ROM)";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(105, 335);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(37, 13);
            this.label48.TabIndex = 43;
            this.label48.Text = "(RAM)";
            // 
            // tboxDPConts_ROM
            // 
            this.tboxDPConts_ROM.Location = new System.Drawing.Point(46, 304);
            this.tboxDPConts_ROM.Name = "tboxDPConts_ROM";
            this.tboxDPConts_ROM.ReadOnly = true;
            this.tboxDPConts_ROM.Size = new System.Drawing.Size(53, 20);
            this.tboxDPConts_ROM.TabIndex = 57;
            this.tboxDPConts_ROM.TextChanged += new System.EventHandler(this.tboxDPConts_ROM_TextChanged);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(12, 334);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(28, 13);
            this.label47.TabIndex = 58;
            this.label47.Text = "[DP]";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.Location = new System.Drawing.Point(59, 157);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(25, 13);
            this.label39.TabIndex = 34;
            this.label39.Text = "er3";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(59, 80);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(25, 13);
            this.label35.TabIndex = 33;
            this.label35.Text = "er1";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(59, 118);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(25, 13);
            this.label34.TabIndex = 32;
            this.label34.Text = "er2";
            // 
            // lblDPContsBinary
            // 
            this.lblDPContsBinary.AutoSize = true;
            this.lblDPContsBinary.Location = new System.Drawing.Point(43, 351);
            this.lblDPContsBinary.Name = "lblDPContsBinary";
            this.lblDPContsBinary.Size = new System.Drawing.Size(58, 13);
            this.lblDPContsBinary.TabIndex = 56;
            this.lblDPContsBinary.Text = "1111 0000";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(59, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "er0";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(12, 281);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(22, 13);
            this.label38.TabIndex = 30;
            this.label38.Text = "DP";
            // 
            // tboxDP
            // 
            this.tboxDP.Location = new System.Drawing.Point(46, 278);
            this.tboxDP.Name = "tboxDP";
            this.tboxDP.ReadOnly = true;
            this.tboxDP.Size = new System.Drawing.Size(53, 20);
            this.tboxDP.TabIndex = 29;
            this.tboxDP.TextChanged += new System.EventHandler(this.tboxDP_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 255);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(28, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "LRB";
            // 
            // tboxDPconts
            // 
            this.tboxDPconts.Location = new System.Drawing.Point(46, 330);
            this.tboxDPconts.Name = "tboxDPconts";
            this.tboxDPconts.ReadOnly = true;
            this.tboxDPconts.Size = new System.Drawing.Size(53, 20);
            this.tboxDPconts.TabIndex = 51;
            this.tboxDPconts.TextChanged += new System.EventHandler(this.tboxDPconts_TextChanged);
            // 
            // lblDPContents
            // 
            this.lblDPContents.AutoSize = true;
            this.lblDPContents.Location = new System.Drawing.Point(12, 308);
            this.lblDPContents.Name = "lblDPContents";
            this.lblDPContents.Size = new System.Drawing.Size(28, 13);
            this.lblDPContents.TabIndex = 52;
            this.lblDPContents.Text = "[DP]";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 229);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(20, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "X2";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 203);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(20, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "X1";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(12, 177);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(21, 13);
            this.label25.TabIndex = 25;
            this.label25.Text = "R7";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(101, 177);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(21, 13);
            this.label26.TabIndex = 24;
            this.label26.Text = "R6";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(12, 137);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(21, 13);
            this.label27.TabIndex = 23;
            this.label27.Text = "R5";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(101, 137);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(21, 13);
            this.label28.TabIndex = 22;
            this.label28.Text = "R4";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(12, 98);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(21, 13);
            this.label29.TabIndex = 21;
            this.label29.Text = "R3";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(101, 98);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(21, 13);
            this.label30.TabIndex = 20;
            this.label30.Text = "R2";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(12, 60);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(21, 13);
            this.label31.TabIndex = 19;
            this.label31.Text = "R1";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(101, 60);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(21, 13);
            this.label32.TabIndex = 18;
            this.label32.Text = "R0";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(6, 21);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(28, 13);
            this.label33.TabIndex = 17;
            this.label33.Text = "ACC";
            // 
            // tboxLRB
            // 
            this.tboxLRB.Location = new System.Drawing.Point(46, 252);
            this.tboxLRB.Name = "tboxLRB";
            this.tboxLRB.ReadOnly = true;
            this.tboxLRB.Size = new System.Drawing.Size(53, 20);
            this.tboxLRB.TabIndex = 14;
            // 
            // tboxACC
            // 
            this.tboxACC.Location = new System.Drawing.Point(46, 18);
            this.tboxACC.Name = "tboxACC";
            this.tboxACC.ReadOnly = true;
            this.tboxACC.Size = new System.Drawing.Size(53, 20);
            this.tboxACC.TabIndex = 3;
            // 
            // tboxX2
            // 
            this.tboxX2.Location = new System.Drawing.Point(46, 226);
            this.tboxX2.Name = "tboxX2";
            this.tboxX2.ReadOnly = true;
            this.tboxX2.Size = new System.Drawing.Size(53, 20);
            this.tboxX2.TabIndex = 13;
            this.tboxX2.TextChanged += new System.EventHandler(this.tboxX2_TextChanged);
            // 
            // tboxR0
            // 
            this.tboxR0.Location = new System.Drawing.Point(73, 57);
            this.tboxR0.Name = "tboxR0";
            this.tboxR0.ReadOnly = true;
            this.tboxR0.Size = new System.Drawing.Size(26, 20);
            this.tboxR0.TabIndex = 4;
            // 
            // tboxX1
            // 
            this.tboxX1.Location = new System.Drawing.Point(46, 200);
            this.tboxX1.Name = "tboxX1";
            this.tboxX1.ReadOnly = true;
            this.tboxX1.Size = new System.Drawing.Size(53, 20);
            this.tboxX1.TabIndex = 12;
            this.toolTip1.SetToolTip(this.tboxX1, "X1 Contents");
            this.tboxX1.TextChanged += new System.EventHandler(this.tboxX1_TextChanged);
            // 
            // tboxR1
            // 
            this.tboxR1.Location = new System.Drawing.Point(46, 57);
            this.tboxR1.Name = "tboxR1";
            this.tboxR1.ReadOnly = true;
            this.tboxR1.Size = new System.Drawing.Size(26, 20);
            this.tboxR1.TabIndex = 5;
            // 
            // tboxR7
            // 
            this.tboxR7.Location = new System.Drawing.Point(46, 174);
            this.tboxR7.Name = "tboxR7";
            this.tboxR7.ReadOnly = true;
            this.tboxR7.Size = new System.Drawing.Size(26, 20);
            this.tboxR7.TabIndex = 11;
            // 
            // tboxR2
            // 
            this.tboxR2.Location = new System.Drawing.Point(73, 95);
            this.tboxR2.Name = "tboxR2";
            this.tboxR2.ReadOnly = true;
            this.tboxR2.Size = new System.Drawing.Size(26, 20);
            this.tboxR2.TabIndex = 6;
            // 
            // tboxR6
            // 
            this.tboxR6.Location = new System.Drawing.Point(73, 174);
            this.tboxR6.Name = "tboxR6";
            this.tboxR6.ReadOnly = true;
            this.tboxR6.Size = new System.Drawing.Size(26, 20);
            this.tboxR6.TabIndex = 10;
            // 
            // tboxR3
            // 
            this.tboxR3.Location = new System.Drawing.Point(46, 95);
            this.tboxR3.Name = "tboxR3";
            this.tboxR3.ReadOnly = true;
            this.tboxR3.Size = new System.Drawing.Size(26, 20);
            this.tboxR3.TabIndex = 7;
            // 
            // tboxR5
            // 
            this.tboxR5.Location = new System.Drawing.Point(46, 134);
            this.tboxR5.Name = "tboxR5";
            this.tboxR5.ReadOnly = true;
            this.tboxR5.Size = new System.Drawing.Size(26, 20);
            this.tboxR5.TabIndex = 9;
            // 
            // tboxR4
            // 
            this.tboxR4.Location = new System.Drawing.Point(73, 134);
            this.tboxR4.Name = "tboxR4";
            this.tboxR4.ReadOnly = true;
            this.tboxR4.Size = new System.Drawing.Size(26, 20);
            this.tboxR4.TabIndex = 8;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label41);
            this.groupBox4.Controls.Add(this.tboxPSWL5);
            this.groupBox4.Controls.Add(this.label40);
            this.groupBox4.Controls.Add(this.tboxPSWL4);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.tboxPSWL);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.tboxPSWH);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.tboxSCB);
            this.groupBox4.Controls.Add(this.tboxSF);
            this.groupBox4.Controls.Add(this.tboxMIE);
            this.groupBox4.Controls.Add(this.tboxBCB);
            this.groupBox4.Controls.Add(this.tboxHC);
            this.groupBox4.Controls.Add(this.tboxDD);
            this.groupBox4.Controls.Add(this.tboxZF);
            this.groupBox4.Controls.Add(this.tboxCF);
            this.groupBox4.Location = new System.Drawing.Point(155, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(214, 177);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PSW";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(100, 69);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(47, 13);
            this.label41.TabIndex = 36;
            this.label41.Text = "PSWL.5";
            // 
            // tboxPSWL5
            // 
            this.tboxPSWL5.Location = new System.Drawing.Point(154, 66);
            this.tboxPSWL5.Name = "tboxPSWL5";
            this.tboxPSWL5.ReadOnly = true;
            this.tboxPSWL5.Size = new System.Drawing.Size(23, 20);
            this.tboxPSWL5.TabIndex = 35;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(6, 69);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(47, 13);
            this.label40.TabIndex = 34;
            this.label40.Text = "PSWL.4";
            // 
            // tboxPSWL4
            // 
            this.tboxPSWL4.Location = new System.Drawing.Point(60, 66);
            this.tboxPSWL4.Name = "tboxPSWL4";
            this.tboxPSWL4.ReadOnly = true;
            this.tboxPSWL4.Size = new System.Drawing.Size(23, 20);
            this.tboxPSWL4.TabIndex = 33;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(100, 147);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(38, 13);
            this.label24.TabIndex = 19;
            this.label24.Text = "PSWL";
            // 
            // tboxPSWL
            // 
            this.tboxPSWL.Location = new System.Drawing.Point(154, 144);
            this.tboxPSWL.Name = "tboxPSWL";
            this.tboxPSWL.ReadOnly = true;
            this.tboxPSWL.Size = new System.Drawing.Size(23, 20);
            this.tboxPSWL.TabIndex = 18;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 147);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(40, 13);
            this.label23.TabIndex = 17;
            this.label23.Text = "PSWH";
            // 
            // tboxPSWH
            // 
            this.tboxPSWH.Location = new System.Drawing.Point(60, 144);
            this.tboxPSWH.Name = "tboxPSWH";
            this.tboxPSWH.ReadOnly = true;
            this.tboxPSWH.Size = new System.Drawing.Size(23, 20);
            this.tboxPSWH.TabIndex = 16;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(100, 122);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(28, 13);
            this.label22.TabIndex = 15;
            this.label22.Text = "SCB";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 122);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(28, 13);
            this.label21.TabIndex = 14;
            this.label21.Text = "BCB";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(100, 43);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(26, 13);
            this.label20.TabIndex = 13;
            this.label20.Text = "MIE";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 43);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(20, 13);
            this.label19.TabIndex = 12;
            this.label19.Text = "ZF";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(100, 17);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 13);
            this.label18.TabIndex = 11;
            this.label18.Text = "DD";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 95);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(22, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "HC";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(100, 95);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(20, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "SF";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(20, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "CF";
            // 
            // tboxSCB
            // 
            this.tboxSCB.Location = new System.Drawing.Point(154, 118);
            this.tboxSCB.Name = "tboxSCB";
            this.tboxSCB.ReadOnly = true;
            this.tboxSCB.Size = new System.Drawing.Size(23, 20);
            this.tboxSCB.TabIndex = 7;
            // 
            // tboxSF
            // 
            this.tboxSF.Location = new System.Drawing.Point(154, 92);
            this.tboxSF.Name = "tboxSF";
            this.tboxSF.ReadOnly = true;
            this.tboxSF.Size = new System.Drawing.Size(23, 20);
            this.tboxSF.TabIndex = 6;
            // 
            // tboxMIE
            // 
            this.tboxMIE.Location = new System.Drawing.Point(154, 40);
            this.tboxMIE.Name = "tboxMIE";
            this.tboxMIE.ReadOnly = true;
            this.tboxMIE.Size = new System.Drawing.Size(23, 20);
            this.tboxMIE.TabIndex = 5;
            // 
            // tboxBCB
            // 
            this.tboxBCB.Location = new System.Drawing.Point(60, 118);
            this.tboxBCB.Name = "tboxBCB";
            this.tboxBCB.ReadOnly = true;
            this.tboxBCB.Size = new System.Drawing.Size(23, 20);
            this.tboxBCB.TabIndex = 4;
            // 
            // tboxHC
            // 
            this.tboxHC.Location = new System.Drawing.Point(60, 92);
            this.tboxHC.Name = "tboxHC";
            this.tboxHC.ReadOnly = true;
            this.tboxHC.Size = new System.Drawing.Size(23, 20);
            this.tboxHC.TabIndex = 3;
            // 
            // tboxDD
            // 
            this.tboxDD.Location = new System.Drawing.Point(154, 14);
            this.tboxDD.Name = "tboxDD";
            this.tboxDD.ReadOnly = true;
            this.tboxDD.Size = new System.Drawing.Size(23, 20);
            this.tboxDD.TabIndex = 2;
            // 
            // tboxZF
            // 
            this.tboxZF.Location = new System.Drawing.Point(60, 40);
            this.tboxZF.Name = "tboxZF";
            this.tboxZF.ReadOnly = true;
            this.tboxZF.Size = new System.Drawing.Size(23, 20);
            this.tboxZF.TabIndex = 1;
            // 
            // tboxCF
            // 
            this.tboxCF.Location = new System.Drawing.Point(60, 14);
            this.tboxCF.Name = "tboxCF";
            this.tboxCF.ReadOnly = true;
            this.tboxCF.Size = new System.Drawing.Size(23, 20);
            this.tboxCF.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboxLockRamWatch);
            this.groupBox2.Controls.Add(this.label45);
            this.groupBox2.Controls.Add(this.nudRAMWatch);
            this.groupBox2.Controls.Add(this.btnFindAddress);
            this.groupBox2.Controls.Add(this.cboxLiveEngine);
            this.groupBox2.Controls.Add(this.nudBPAddress);
            this.groupBox2.Controls.Add(this.cboxLockBP);
            this.groupBox2.Controls.Add(this.cboxAutoStep);
            this.groupBox2.Controls.Add(this.cboxStepInto);
            this.groupBox2.Controls.Add(this.btnStepFwd);
            this.groupBox2.Controls.Add(this.btnRemoveBreakPoint);
            this.groupBox2.Controls.Add(this.btnSetBreakpoint);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(6, 424);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 141);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // cboxLockRamWatch
            // 
            this.cboxLockRamWatch.AutoSize = true;
            this.cboxLockRamWatch.Location = new System.Drawing.Point(5, 117);
            this.cboxLockRamWatch.Name = "cboxLockRamWatch";
            this.cboxLockRamWatch.Size = new System.Drawing.Size(112, 17);
            this.cboxLockRamWatch.TabIndex = 37;
            this.cboxLockRamWatch.Text = "Lock RAM Watch";
            this.cboxLockRamWatch.UseVisualStyleBackColor = true;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(18, 87);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(66, 13);
            this.label45.TabIndex = 36;
            this.label45.Text = "RAM Watch";
            // 
            // nudRAMWatch
            // 
            this.nudRAMWatch.Hexadecimal = true;
            this.nudRAMWatch.Location = new System.Drawing.Point(12, 64);
            this.nudRAMWatch.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.nudRAMWatch.Name = "nudRAMWatch";
            this.nudRAMWatch.Size = new System.Drawing.Size(92, 20);
            this.nudRAMWatch.TabIndex = 35;
            this.nudRAMWatch.Value = new decimal(new int[] {
            1150,
            0,
            0,
            0});
            // 
            // btnFindAddress
            // 
            this.btnFindAddress.Location = new System.Drawing.Point(111, 61);
            this.btnFindAddress.Name = "btnFindAddress";
            this.btnFindAddress.Size = new System.Drawing.Size(114, 26);
            this.btnFindAddress.TabIndex = 34;
            this.btnFindAddress.Text = "Find Address";
            this.btnFindAddress.UseVisualStyleBackColor = true;
            this.btnFindAddress.Click += new System.EventHandler(this.btnFindAddress_Click);
            // 
            // cboxLiveEngine
            // 
            this.cboxLiveEngine.AutoSize = true;
            this.cboxLiveEngine.Enabled = false;
            this.cboxLiveEngine.Location = new System.Drawing.Point(247, 119);
            this.cboxLiveEngine.Name = "cboxLiveEngine";
            this.cboxLiveEngine.Size = new System.Drawing.Size(82, 17);
            this.cboxLiveEngine.TabIndex = 33;
            this.cboxLiveEngine.Text = "Live Engine";
            this.cboxLiveEngine.UseVisualStyleBackColor = true;
            this.cboxLiveEngine.Visible = false;
            // 
            // nudBPAddress
            // 
            this.nudBPAddress.Hexadecimal = true;
            this.nudBPAddress.Location = new System.Drawing.Point(12, 29);
            this.nudBPAddress.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.nudBPAddress.Name = "nudBPAddress";
            this.nudBPAddress.Size = new System.Drawing.Size(92, 20);
            this.nudBPAddress.TabIndex = 27;
            this.nudBPAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudBPAddress_KeyDown);
            // 
            // cboxLockBP
            // 
            this.cboxLockBP.AutoSize = true;
            this.cboxLockBP.Location = new System.Drawing.Point(123, 119);
            this.cboxLockBP.Name = "cboxLockBP";
            this.cboxLockBP.Size = new System.Drawing.Size(104, 17);
            this.cboxLockBP.TabIndex = 32;
            this.cboxLockBP.Text = "Lock Breakpoint";
            this.cboxLockBP.UseVisualStyleBackColor = true;
            this.cboxLockBP.CheckedChanged += new System.EventHandler(this.cboxLockBP_CheckedChanged);
            // 
            // cboxAutoStep
            // 
            this.cboxAutoStep.AutoSize = true;
            this.cboxAutoStep.Location = new System.Drawing.Point(247, 94);
            this.cboxAutoStep.Name = "cboxAutoStep";
            this.cboxAutoStep.Size = new System.Drawing.Size(70, 17);
            this.cboxAutoStep.TabIndex = 31;
            this.cboxAutoStep.Text = "AutoStep";
            this.cboxAutoStep.UseVisualStyleBackColor = true;
            this.cboxAutoStep.CheckedChanged += new System.EventHandler(this.cboxAutoStep_CheckedChanged);
            // 
            // cboxStepInto
            // 
            this.cboxStepInto.AutoSize = true;
            this.cboxStepInto.Location = new System.Drawing.Point(123, 94);
            this.cboxStepInto.Name = "cboxStepInto";
            this.cboxStepInto.Size = new System.Drawing.Size(102, 17);
            this.cboxStepInto.TabIndex = 30;
            this.cboxStepInto.Text = "Step INTO Calls";
            this.cboxStepInto.UseVisualStyleBackColor = true;
            this.cboxStepInto.CheckStateChanged += new System.EventHandler(this.cboxStepInto_CheckedChanged);
            // 
            // btnStepFwd
            // 
            this.btnStepFwd.Location = new System.Drawing.Point(233, 61);
            this.btnStepFwd.Name = "btnStepFwd";
            this.btnStepFwd.Size = new System.Drawing.Size(124, 26);
            this.btnStepFwd.TabIndex = 19;
            this.btnStepFwd.Text = "Step instruction";
            this.btnStepFwd.UseVisualStyleBackColor = true;
            this.btnStepFwd.Click += new System.EventHandler(this.btnStepFwd_Click);
            // 
            // btnRemoveBreakPoint
            // 
            this.btnRemoveBreakPoint.Location = new System.Drawing.Point(233, 23);
            this.btnRemoveBreakPoint.Name = "btnRemoveBreakPoint";
            this.btnRemoveBreakPoint.Size = new System.Drawing.Size(124, 29);
            this.btnRemoveBreakPoint.TabIndex = 18;
            this.btnRemoveBreakPoint.Text = "Remove Breakpoints";
            this.btnRemoveBreakPoint.UseVisualStyleBackColor = true;
            this.btnRemoveBreakPoint.Click += new System.EventHandler(this.btnRemoveBreakPoint_Click);
            // 
            // btnSetBreakpoint
            // 
            this.btnSetBreakpoint.Location = new System.Drawing.Point(111, 23);
            this.btnSetBreakpoint.Name = "btnSetBreakpoint";
            this.btnSetBreakpoint.Size = new System.Drawing.Size(114, 29);
            this.btnSetBreakpoint.TabIndex = 16;
            this.btnSetBreakpoint.Text = "Set Breakpoint";
            this.btnSetBreakpoint.UseVisualStyleBackColor = true;
            this.btnSetBreakpoint.Click += new System.EventHandler(this.btnSetBreakpoint_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Breakpoint Address";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lblChangedBytes);
            this.groupBox8.Controls.Add(this.cboxQuietCompiler);
            this.groupBox8.Controls.Add(this.label46);
            this.groupBox8.Controls.Add(this.cboxMonitorBin);
            this.groupBox8.Controls.Add(this.cboxSlowUpload);
            this.groupBox8.Controls.Add(this.cboxAutoUploadOnBINLoad);
            this.groupBox8.Controls.Add(this.btnLoadBinToOstrich);
            this.groupBox8.Controls.Add(this.btnLoadASMFile);
            this.groupBox8.Controls.Add(this.tboxOpenFileSimpleName);
            this.groupBox8.Controls.Add(this.btnLoadBinFile);
            this.groupBox8.Location = new System.Drawing.Point(3, 535);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(200, 204);
            this.groupBox8.TabIndex = 33;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Open Source";
            // 
            // lblChangedBytes
            // 
            this.lblChangedBytes.AutoSize = true;
            this.lblChangedBytes.Location = new System.Drawing.Point(17, 182);
            this.lblChangedBytes.Name = "lblChangedBytes";
            this.lblChangedBytes.Size = new System.Drawing.Size(79, 13);
            this.lblChangedBytes.TabIndex = 36;
            this.lblChangedBytes.Text = "Bytes Changed";
            // 
            // cboxQuietCompiler
            // 
            this.cboxQuietCompiler.AutoSize = true;
            this.cboxQuietCompiler.Location = new System.Drawing.Point(35, 162);
            this.cboxQuietCompiler.Name = "cboxQuietCompiler";
            this.cboxQuietCompiler.Size = new System.Drawing.Size(129, 17);
            this.cboxQuietCompiler.TabIndex = 39;
            this.cboxQuietCompiler.Text = "Quiet Compiler Output";
            this.cboxQuietCompiler.UseVisualStyleBackColor = true;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(89, 25);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(22, 13);
            this.label46.TabIndex = 31;
            this.label46.Text = "-or-";
            // 
            // cboxMonitorBin
            // 
            this.cboxMonitorBin.AutoSize = true;
            this.cboxMonitorBin.Location = new System.Drawing.Point(20, 146);
            this.cboxMonitorBin.Name = "cboxMonitorBin";
            this.cboxMonitorBin.Size = new System.Drawing.Size(172, 17);
            this.cboxMonitorBin.TabIndex = 38;
            this.cboxMonitorBin.Text = "Monitor BIN/ASM For changes";
            this.cboxMonitorBin.UseVisualStyleBackColor = true;
            this.cboxMonitorBin.CheckedChanged += new System.EventHandler(this.cboxMonitorBin_CheckedChanged);
            // 
            // cboxSlowUpload
            // 
            this.cboxSlowUpload.AutoSize = true;
            this.cboxSlowUpload.Location = new System.Drawing.Point(20, 127);
            this.cboxSlowUpload.Name = "cboxSlowUpload";
            this.cboxSlowUpload.Size = new System.Drawing.Size(86, 17);
            this.cboxSlowUpload.TabIndex = 37;
            this.cboxSlowUpload.Text = "Slow Upload";
            this.cboxSlowUpload.UseVisualStyleBackColor = true;
            // 
            // cboxAutoUploadOnBINLoad
            // 
            this.cboxAutoUploadOnBINLoad.AutoSize = true;
            this.cboxAutoUploadOnBINLoad.Location = new System.Drawing.Point(20, 109);
            this.cboxAutoUploadOnBINLoad.Name = "cboxAutoUploadOnBINLoad";
            this.cboxAutoUploadOnBINLoad.Size = new System.Drawing.Size(85, 17);
            this.cboxAutoUploadOnBINLoad.TabIndex = 36;
            this.cboxAutoUploadOnBINLoad.Text = "Auto Upload";
            this.cboxAutoUploadOnBINLoad.UseVisualStyleBackColor = true;
            // 
            // btnLoadBinToOstrich
            // 
            this.btnLoadBinToOstrich.Location = new System.Drawing.Point(55, 75);
            this.btnLoadBinToOstrich.Name = "btnLoadBinToOstrich";
            this.btnLoadBinToOstrich.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnLoadBinToOstrich.Size = new System.Drawing.Size(85, 25);
            this.btnLoadBinToOstrich.TabIndex = 34;
            this.btnLoadBinToOstrich.Text = "Re-Upload";
            this.btnLoadBinToOstrich.UseVisualStyleBackColor = true;
            this.btnLoadBinToOstrich.Click += new System.EventHandler(this.btnLoadBinToOstrich_Click);
            // 
            // btnLoadASMFile
            // 
            this.btnLoadASMFile.Location = new System.Drawing.Point(118, 19);
            this.btnLoadASMFile.Name = "btnLoadASMFile";
            this.btnLoadASMFile.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnLoadASMFile.Size = new System.Drawing.Size(69, 25);
            this.btnLoadASMFile.TabIndex = 25;
            this.btnLoadASMFile.Text = "Load ASM";
            this.btnLoadASMFile.UseVisualStyleBackColor = true;
            this.btnLoadASMFile.Click += new System.EventHandler(this.btnLoadASMFile_Click);
            // 
            // tboxOpenFileSimpleName
            // 
            this.tboxOpenFileSimpleName.Location = new System.Drawing.Point(13, 49);
            this.tboxOpenFileSimpleName.Name = "tboxOpenFileSimpleName";
            this.tboxOpenFileSimpleName.Size = new System.Drawing.Size(174, 20);
            this.tboxOpenFileSimpleName.TabIndex = 28;
            // 
            // btnLoadBinFile
            // 
            this.btnLoadBinFile.Location = new System.Drawing.Point(14, 19);
            this.btnLoadBinFile.Name = "btnLoadBinFile";
            this.btnLoadBinFile.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnLoadBinFile.Size = new System.Drawing.Size(69, 25);
            this.btnLoadBinFile.TabIndex = 27;
            this.btnLoadBinFile.Text = "Load BIN";
            this.btnLoadBinFile.UseVisualStyleBackColor = true;
            this.btnLoadBinFile.Click += new System.EventHandler(this.btnLoadBinFile_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnResetOstrich);
            this.groupBox7.Controls.Add(this.nudVIDNumber);
            this.groupBox7.Controls.Add(this.label37);
            this.groupBox7.Controls.Add(this.progressBar2);
            this.groupBox7.Controls.Add(this.tboxOstrichSerialNumber);
            this.groupBox7.Controls.Add(this.btnCloseOstrichCom);
            this.groupBox7.Controls.Add(this.label36);
            this.groupBox7.Controls.Add(this.btnOpenOstrichCom);
            this.groupBox7.Controls.Add(this.cboxOstrichComPortNumber);
            this.groupBox7.Location = new System.Drawing.Point(3, 252);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 127);
            this.groupBox7.TabIndex = 32;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Ostrich";
            // 
            // btnResetOstrich
            // 
            this.btnResetOstrich.Location = new System.Drawing.Point(110, 95);
            this.btnResetOstrich.Name = "btnResetOstrich";
            this.btnResetOstrich.Size = new System.Drawing.Size(51, 23);
            this.btnResetOstrich.TabIndex = 29;
            this.btnResetOstrich.Text = "RESET";
            this.btnResetOstrich.UseVisualStyleBackColor = true;
            this.btnResetOstrich.Click += new System.EventHandler(this.btnResetOstrich_Click);
            // 
            // nudVIDNumber
            // 
            this.nudVIDNumber.Hexadecimal = true;
            this.nudVIDNumber.Location = new System.Drawing.Point(162, 98);
            this.nudVIDNumber.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudVIDNumber.Name = "nudVIDNumber";
            this.nudVIDNumber.Size = new System.Drawing.Size(30, 20);
            this.nudVIDNumber.TabIndex = 30;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(3, 38);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(73, 13);
            this.label37.TabIndex = 28;
            this.label37.Text = "Serial Number";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(4, 80);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(187, 10);
            this.progressBar2.TabIndex = 25;
            // 
            // tboxOstrichSerialNumber
            // 
            this.tboxOstrichSerialNumber.Location = new System.Drawing.Point(3, 54);
            this.tboxOstrichSerialNumber.Name = "tboxOstrichSerialNumber";
            this.tboxOstrichSerialNumber.ReadOnly = true;
            this.tboxOstrichSerialNumber.Size = new System.Drawing.Size(189, 20);
            this.tboxOstrichSerialNumber.TabIndex = 27;
            // 
            // btnCloseOstrichCom
            // 
            this.btnCloseOstrichCom.Location = new System.Drawing.Point(55, 95);
            this.btnCloseOstrichCom.Name = "btnCloseOstrichCom";
            this.btnCloseOstrichCom.Size = new System.Drawing.Size(51, 23);
            this.btnCloseOstrichCom.TabIndex = 26;
            this.btnCloseOstrichCom.Text = "CLOSE";
            this.btnCloseOstrichCom.UseVisualStyleBackColor = true;
            this.btnCloseOstrichCom.Click += new System.EventHandler(this.btnCloseOstrichCom_Click);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(7, 20);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(64, 13);
            this.label36.TabIndex = 26;
            this.label36.Text = "COM PORT";
            // 
            // btnOpenOstrichCom
            // 
            this.btnOpenOstrichCom.Location = new System.Drawing.Point(3, 95);
            this.btnOpenOstrichCom.Name = "btnOpenOstrichCom";
            this.btnOpenOstrichCom.Size = new System.Drawing.Size(50, 23);
            this.btnOpenOstrichCom.TabIndex = 25;
            this.btnOpenOstrichCom.Text = "OPEN";
            this.btnOpenOstrichCom.UseVisualStyleBackColor = true;
            this.btnOpenOstrichCom.Click += new System.EventHandler(this.btnOpenOstrichCom_Click);
            // 
            // cboxOstrichComPortNumber
            // 
            this.cboxOstrichComPortNumber.FormattingEnabled = true;
            this.cboxOstrichComPortNumber.Location = new System.Drawing.Point(86, 16);
            this.cboxOstrichComPortNumber.Name = "cboxOstrichComPortNumber";
            this.cboxOstrichComPortNumber.Size = new System.Drawing.Size(99, 21);
            this.cboxOstrichComPortNumber.TabIndex = 25;
            this.cboxOstrichComPortNumber.Text = "COM9";
            this.cboxOstrichComPortNumber.DropDown += new System.EventHandler(this.cboxOstrichComPortNumber_DropDown);
            // 
            // rtboxASMFile
            // 
            this.rtboxASMFile.BackColor = System.Drawing.SystemColors.Window;
            this.rtboxASMFile.Font = new System.Drawing.Font("DejaVu Sans Mono", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtboxASMFile.Location = new System.Drawing.Point(209, 3);
            this.rtboxASMFile.Name = "rtboxASMFile";
            this.rtboxASMFile.ReadOnly = true;
            this.rtboxASMFile.ShortcutsEnabled = false;
            this.rtboxASMFile.Size = new System.Drawing.Size(717, 415);
            this.rtboxASMFile.TabIndex = 25;
            this.rtboxASMFile.Text = "";
            this.rtboxASMFile.Click += new System.EventHandler(this.rtboxASMFile_Click);
            this.rtboxASMFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtboxASMFile_MouseDown);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.cboxRenameOnChange);
            this.groupBox12.Controls.Add(this.progressBar3);
            this.groupBox12.Controls.Add(this.btnSaveRenamedASM);
            this.groupBox12.Controls.Add(this.btnSwap);
            this.groupBox12.Controls.Add(this.btnOpenXMLFile);
            this.groupBox12.Controls.Add(this.btnApplyRenamingMask);
            this.groupBox12.Controls.Add(this.cboxSaveTrace);
            this.groupBox12.Controls.Add(this.tboxTraceWindow);
            this.groupBox12.Controls.Add(this.btnClearText);
            this.groupBox12.Location = new System.Drawing.Point(209, 424);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(729, 295);
            this.groupBox12.TabIndex = 7;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Trace and Renaming";
            // 
            // cboxRenameOnChange
            // 
            this.cboxRenameOnChange.AutoSize = true;
            this.cboxRenameOnChange.Location = new System.Drawing.Point(107, 29);
            this.cboxRenameOnChange.Name = "cboxRenameOnChange";
            this.cboxRenameOnChange.Size = new System.Drawing.Size(148, 17);
            this.cboxRenameOnChange.TabIndex = 33;
            this.cboxRenameOnChange.Text = "Rename on dasm change";
            this.cboxRenameOnChange.UseVisualStyleBackColor = true;
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(274, 37);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(399, 10);
            this.progressBar3.TabIndex = 32;
            // 
            // btnSaveRenamedASM
            // 
            this.btnSaveRenamedASM.Location = new System.Drawing.Point(588, 10);
            this.btnSaveRenamedASM.Name = "btnSaveRenamedASM";
            this.btnSaveRenamedASM.Size = new System.Drawing.Size(85, 23);
            this.btnSaveRenamedASM.TabIndex = 31;
            this.btnSaveRenamedASM.Text = "Save ASM";
            this.btnSaveRenamedASM.UseVisualStyleBackColor = true;
            this.btnSaveRenamedASM.Click += new System.EventHandler(this.btnSaveRenamedASM_Click);
            // 
            // btnSwap
            // 
            this.btnSwap.Location = new System.Drawing.Point(497, 10);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(85, 23);
            this.btnSwap.TabIndex = 30;
            this.btnSwap.Text = "Swap";
            this.btnSwap.UseVisualStyleBackColor = true;
            this.btnSwap.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnOpenXMLFile
            // 
            this.btnOpenXMLFile.Location = new System.Drawing.Point(274, 10);
            this.btnOpenXMLFile.Name = "btnOpenXMLFile";
            this.btnOpenXMLFile.Size = new System.Drawing.Size(126, 23);
            this.btnOpenXMLFile.TabIndex = 29;
            this.btnOpenXMLFile.Text = "Open Rename File";
            this.btnOpenXMLFile.UseVisualStyleBackColor = true;
            this.btnOpenXMLFile.Click += new System.EventHandler(this.btnOpenXMLFile_Click);
            // 
            // btnApplyRenamingMask
            // 
            this.btnApplyRenamingMask.Location = new System.Drawing.Point(406, 10);
            this.btnApplyRenamingMask.Name = "btnApplyRenamingMask";
            this.btnApplyRenamingMask.Size = new System.Drawing.Size(85, 23);
            this.btnApplyRenamingMask.TabIndex = 28;
            this.btnApplyRenamingMask.Text = "Rename ";
            this.btnApplyRenamingMask.UseVisualStyleBackColor = true;
            this.btnApplyRenamingMask.Click += new System.EventHandler(this.btnApplyRenamingMask_Click);
            // 
            // cboxSaveTrace
            // 
            this.cboxSaveTrace.AutoSize = true;
            this.cboxSaveTrace.Location = new System.Drawing.Point(107, 14);
            this.cboxSaveTrace.Name = "cboxSaveTrace";
            this.cboxSaveTrace.Size = new System.Drawing.Size(161, 17);
            this.cboxSaveTrace.TabIndex = 25;
            this.cboxSaveTrace.Text = "Save Trace to ECUTrace.txt";
            this.cboxSaveTrace.UseVisualStyleBackColor = true;
            // 
            // tboxTraceWindow
            // 
            this.tboxTraceWindow.Font = new System.Drawing.Font("DejaVu Sans Mono", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxTraceWindow.Location = new System.Drawing.Point(9, 51);
            this.tboxTraceWindow.Margin = new System.Windows.Forms.Padding(6);
            this.tboxTraceWindow.MaximumSize = new System.Drawing.Size(797, 271);
            this.tboxTraceWindow.Multiline = true;
            this.tboxTraceWindow.Name = "tboxTraceWindow";
            this.tboxTraceWindow.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tboxTraceWindow.Size = new System.Drawing.Size(708, 244);
            this.tboxTraceWindow.TabIndex = 26;
            // 
            // btnClearText
            // 
            this.btnClearText.Location = new System.Drawing.Point(8, 13);
            this.btnClearText.Name = "btnClearText";
            this.btnClearText.Size = new System.Drawing.Size(93, 29);
            this.btnClearText.TabIndex = 25;
            this.btnClearText.Text = "Clear Trace";
            this.btnClearText.UseVisualStyleBackColor = true;
            this.btnClearText.Click += new System.EventHandler(this.btnClearText_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // serialPort2
            // 
            this.serialPort2.BaudRate = 921600;
            this.serialPort2.ReadTimeout = 250;
            this.serialPort2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialport2_DataReceived);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 1;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 1;
            this.toolTip1.ReshowDelay = 0;
            // 
            // binMonitorTimer
            // 
            this.binMonitorTimer.Interval = 1000;
            this.binMonitorTimer.Tick += new System.EventHandler(this.binMonitorTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1150, 650);
            this.ClientSize = new System.Drawing.Size(1334, 811);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1350, 850);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Honda ECU Debugger v3.2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudROMCodeAddress)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDebuggerLRB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDebuggerIE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDebuggerPSW)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRAMWatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBPAddress)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVIDNumber)).EndInit();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cBoxParityBits;
        private System.Windows.Forms.ComboBox cBoxStopBits;
        private System.Windows.Forms.ComboBox cBoxDataBits;
        private System.Windows.Forms.ComboBox CBoxBaudRate;
        private System.Windows.Forms.ComboBox cBoxCOMPORT;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSetBreakpoint;
        private System.Windows.Forms.Button btnRemoveBreakPoint;
        private System.Windows.Forms.Button btnStepFwd;
        private System.Windows.Forms.Button btnLoadASMFile;
        private System.Windows.Forms.TextBox tboxOpenFileSimpleName;
        private System.Windows.Forms.Button btnLoadBinFile;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tboxECUReturnAddress;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tboxPSWL;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tboxPSWH;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tboxSCB;
        private System.Windows.Forms.TextBox tboxSF;
        private System.Windows.Forms.TextBox tboxMIE;
        private System.Windows.Forms.TextBox tboxBCB;
        private System.Windows.Forms.TextBox tboxHC;
        private System.Windows.Forms.TextBox tboxDD;
        private System.Windows.Forms.TextBox tboxZF;
        private System.Windows.Forms.TextBox tboxCF;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox tboxLRB;
        private System.Windows.Forms.TextBox tboxACC;
        private System.Windows.Forms.TextBox tboxX2;
        private System.Windows.Forms.TextBox tboxR0;
        private System.Windows.Forms.TextBox tboxX1;
        private System.Windows.Forms.TextBox tboxR1;
        private System.Windows.Forms.TextBox tboxR7;
        private System.Windows.Forms.TextBox tboxR2;
        private System.Windows.Forms.TextBox tboxR6;
        private System.Windows.Forms.TextBox tboxR3;
        private System.Windows.Forms.TextBox tboxR5;
        private System.Windows.Forms.TextBox tboxR4;
        private System.Windows.Forms.RichTextBox rtboxASMFile;
        private System.Windows.Forms.Button btnCloseCOMPort;
        private System.Windows.Forms.Button btnOpenCOMPort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cboxStepInto;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox cboxAutoStep;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.TextBox tboxOstrichSerialNumber;
        private System.Windows.Forms.Button btnCloseOstrichCom;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Button btnOpenOstrichCom;
        private System.Windows.Forms.ComboBox cboxOstrichComPortNumber;
        private System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnLoadBinToOstrich;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox tboxDP;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tboxStack1;
        private System.Windows.Forms.CheckBox cboxLockBP;
        private System.Windows.Forms.NumericUpDown nudBPAddress;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudROMCodeAddress;
        private System.Windows.Forms.ComboBox cmboxRomType;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.CheckBox cboxDebuggerLRB;
        private System.Windows.Forms.CheckBox cboxDebuggerIE;
        private System.Windows.Forms.CheckBox cboxDebuggerPSW;
        private System.Windows.Forms.NumericUpDown nudDebuggerLRB;
        private System.Windows.Forms.NumericUpDown nudDebuggerIE;
        private System.Windows.Forms.NumericUpDown nudDebuggerPSW;
        private System.Windows.Forms.Button btnSendByte;
        private System.Windows.Forms.TextBox tboxHexToSend;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cboxAutoUploadOnBINLoad;
        private System.Windows.Forms.Button btnReloadDebuggerCode;
        private System.Windows.Forms.CheckBox cboxSlowUpload;
        private System.Windows.Forms.Button btnResetOstrich;
        private System.Windows.Forms.NumericUpDown nudVIDNumber;
        private System.Windows.Forms.ToolStripMenuItem tsOpenRenamingForm;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.CheckBox cboxSaveTrace;
        private System.Windows.Forms.TextBox tboxTraceWindow;
        private System.Windows.Forms.Button btnClearText;
        private System.Windows.Forms.Button btnOpenXMLFile;
        private System.Windows.Forms.Button btnApplyRenamingMask;
        private System.Windows.Forms.Button btnSwap;
        private System.Windows.Forms.Button btnSaveRenamedASM;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cboxLiveEngine;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox tboxPSWL5;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox tboxPSWL4;
        private System.Windows.Forms.TextBox tboxDasmArgIgnore;
        private System.Windows.Forms.Button btnRefreshDasm;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox tboxDasmArgForce;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label lblLA12;
        private System.Windows.Forms.TextBox tboxLA1_2;
        private System.Windows.Forms.Label lblLA11;
        private System.Windows.Forms.TextBox tboxLA1_1;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox tboxStackPointer;
        private System.Windows.Forms.TextBox tboxStack5;
        private System.Windows.Forms.TextBox tboxStack4;
        private System.Windows.Forms.TextBox tboxStack3;
        private System.Windows.Forms.TextBox tboxStack2;
        private System.Windows.Forms.Label lblDPContents;
        private System.Windows.Forms.TextBox tboxDPconts;
        private System.Windows.Forms.Label lblDPContsBinary;
        private System.Windows.Forms.Label lblXram2Binary;
        private System.Windows.Forms.Label lblXram1Binary;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.Button btnFindAddress;
        private System.Windows.Forms.CheckBox cboxLockRamWatch;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.NumericUpDown nudRAMWatch;
        private System.Windows.Forms.Label lblRamWatchBinary;
        private System.Windows.Forms.Label lblRamWatch;
        private System.Windows.Forms.TextBox tboxRamWatch;
        private System.Windows.Forms.CheckBox cboxMonitorBin;
        private System.Windows.Forms.Timer binMonitorTimer;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.CheckBox cboxQuietCompiler;
        private System.Windows.Forms.Label lblChangedBytes;
        private System.Windows.Forms.CheckBox cboxRenameOnChange;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox tboxDPConts_ROM;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label lblDebuggerCodeSize;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.CheckBox cboxVCALReRoute;
        private System.Windows.Forms.Label lblVCALbyteSize;
        private System.Windows.Forms.Label lblSizeWithVCAL;
        private System.Windows.Forms.Label lblVCALnumber;
    }
}

