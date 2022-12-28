
namespace ECU_Debugger
{
    partial class Form2
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
            this.dgvASMRenamed = new System.Windows.Forms.DataGridView();
            this.Enable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.OriginalName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOpenAsmXMLFile = new System.Windows.Forms.Button();
            this.btnSaveRenamingFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCurrentFile = new System.Windows.Forms.Label();
            this.tboxSearchBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvASMRenamed)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvASMRenamed
            // 
            this.dgvASMRenamed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvASMRenamed.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Enable,
            this.OriginalName,
            this.NewName});
            this.dgvASMRenamed.Location = new System.Drawing.Point(12, 76);
            this.dgvASMRenamed.Name = "dgvASMRenamed";
            this.dgvASMRenamed.Size = new System.Drawing.Size(713, 392);
            this.dgvASMRenamed.TabIndex = 0;
            // 
            // Enable
            // 
            this.Enable.HeaderText = "Enable";
            this.Enable.Name = "Enable";
            this.Enable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Enable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Enable.Width = 50;
            // 
            // OriginalName
            // 
            this.OriginalName.HeaderText = "OriginalName";
            this.OriginalName.Name = "OriginalName";
            this.OriginalName.Width = 200;
            // 
            // NewName
            // 
            this.NewName.HeaderText = "NewName";
            this.NewName.Name = "NewName";
            this.NewName.Width = 400;
            // 
            // btnOpenAsmXMLFile
            // 
            this.btnOpenAsmXMLFile.Location = new System.Drawing.Point(30, 486);
            this.btnOpenAsmXMLFile.Name = "btnOpenAsmXMLFile";
            this.btnOpenAsmXMLFile.Size = new System.Drawing.Size(113, 23);
            this.btnOpenAsmXMLFile.TabIndex = 1;
            this.btnOpenAsmXMLFile.Text = "Open Renaming File";
            this.btnOpenAsmXMLFile.UseVisualStyleBackColor = true;
            this.btnOpenAsmXMLFile.Click += new System.EventHandler(this.btnOpenAsmXMLFile_Click);
            // 
            // btnSaveRenamingFile
            // 
            this.btnSaveRenamingFile.Location = new System.Drawing.Point(149, 486);
            this.btnSaveRenamingFile.Name = "btnSaveRenamingFile";
            this.btnSaveRenamingFile.Size = new System.Drawing.Size(113, 23);
            this.btnSaveRenamingFile.TabIndex = 2;
            this.btnSaveRenamingFile.Text = "Save AS";
            this.btnSaveRenamingFile.UseVisualStyleBackColor = true;
            this.btnSaveRenamingFile.Click += new System.EventHandler(this.btnSaveRenamingFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current File:";
            // 
            // labelCurrentFile
            // 
            this.labelCurrentFile.AutoSize = true;
            this.labelCurrentFile.Location = new System.Drawing.Point(122, 9);
            this.labelCurrentFile.Name = "labelCurrentFile";
            this.labelCurrentFile.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentFile.TabIndex = 4;
            // 
            // tboxSearchBox
            // 
            this.tboxSearchBox.Location = new System.Drawing.Point(120, 39);
            this.tboxSearchBox.Name = "tboxSearchBox";
            this.tboxSearchBox.Size = new System.Drawing.Size(288, 20);
            this.tboxSearchBox.TabIndex = 5;
            this.tboxSearchBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tboxSearchBox_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Search:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(650, 486);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Clear \\ New";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 521);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tboxSearchBox);
            this.Controls.Add(this.labelCurrentFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSaveRenamingFile);
            this.Controls.Add(this.btnOpenAsmXMLFile);
            this.Controls.Add(this.dgvASMRenamed);
            this.Name = "Form2";
            this.Text = "ASM Renaming File Creator";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvASMRenamed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvASMRenamed;
        private System.Windows.Forms.Button btnOpenAsmXMLFile;
        private System.Windows.Forms.Button btnSaveRenamingFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCurrentFile;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enable;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewName;
        private System.Windows.Forms.TextBox tboxSearchBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}