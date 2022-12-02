using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace ECU_Debugger
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            
            InitializeComponent();
            

        }
        string OpenedXMLFile;
        DataSet dataset1 = new DataSet();
        DataView dv = new DataView();
        bool FirstRun = true;



        OpenFileDialog OPENAsmRenamingFile = new OpenFileDialog();
        private void btnOpenAsmXMLFile_Click(object sender, EventArgs e)
        {
           
            OPENAsmRenamingFile.Filter = "XML Files | *.xml";
            if (OPENAsmRenamingFile.ShowDialog() == DialogResult.OK)
            {
                dgvASMRenamed.DataSource = null;
                dgvASMRenamed.Rows.Clear();
                tboxSearchBox.Text = "";
                dv.RowFilter = null;
                if (dv.Table != null)
                {
                    dv.Table.Clear();
                    this.dgvASMRenamed.DataSource = null;
                    dgvASMRenamed.Rows.Clear();
                }

                OpenedXMLFile = OPENAsmRenamingFile.FileName;

                try
                {
                    labelCurrentFile.Text = OPENAsmRenamingFile.FileName;
                    dataset1.ReadXml(OpenedXMLFile);
                    dgvASMRenamed.AutoGenerateColumns = false;
                    dgvASMRenamed.Columns[0].DataPropertyName = "Enable";
                    dgvASMRenamed.Columns[1].DataPropertyName = "OriginalName";
                    dgvASMRenamed.Columns[2].DataPropertyName = "NewName";
                    dv.Table = dataset1.Tables[0];
                    dgvASMRenamed.DataSource = dv;
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }


        DataTable DT = new DataTable();
        private DataTable GetDataTableFromDGV(DataGridView dgv)
        {

            int j = 0;
            foreach(DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible)
                {
                    string headerText = dgv.Columns[j].HeaderText;
                    headerText = System.Text.RegularExpressions.Regex.Replace(headerText, "[-/, ]", "_");
                    if (FirstRun)
                    {
                        DataColumn column = new DataColumn(headerText);
                        DT.Columns.Add(column);
                    }

                    j++;
                }
            }

            object[] cellValues = new object[dgv.Columns.Count];
            foreach(DataGridViewRow row in dgv.Rows)
            {
                for(int i = 0; i < row.Cells.Count; i++)
                {
                    cellValues[i] = row.Cells[i].Value;
                }
                DT.Rows.Add(cellValues);
            }
            FirstRun = false;
            return DT;
        }

        SaveFileDialog SaveAsmRenamingFile = new SaveFileDialog();
        DataSet DS = new DataSet();
        private void btnSaveRenamingFile_Click(object sender, EventArgs e)
        {
                SaveAsmRenamingFile.Filter = "XML Files | *.xml";
                if (SaveAsmRenamingFile.ShowDialog() == DialogResult.OK)
                {
                labelCurrentFile.Text = SaveAsmRenamingFile.FileName;
                OpenedXMLFile = SaveAsmRenamingFile.FileName;
                tboxSearchBox.Text = "";
                dv.RowFilter = null;
                DT.Clear();
                DT = GetDataTableFromDGV(dgvASMRenamed);
                if (DS.Tables.CanRemove(DT))
                {
                    DS.Tables.Remove(DT);
                }
                DS.Clear();
                DS.Tables.Add(DT);
                
               
                              
                // Create the FileStream to write with.
                System.IO.FileStream stream = new System.IO.FileStream
                    (OpenedXMLFile, System.IO.FileMode.Create);

                // Create an XmlTextWriter with the fileStream.
                System.Xml.XmlTextWriter xmlWriter =
                    new System.Xml.XmlTextWriter(stream,
                    System.Text.Encoding.Unicode);

                // Write to the file with the WriteXml method.
                DS.WriteXml(xmlWriter);
                xmlWriter.Close();

                }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void tboxSearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            dv.RowFilter = "OriginalName Like '%" + tboxSearchBox.Text + "%' OR NewName Like '%" + tboxSearchBox.Text + "%'";
            this.dgvASMRenamed.DataSource = dv;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dv.Table != null)
            {
                dv.Table.Clear();
                this.dgvASMRenamed.DataSource = null;
                dgvASMRenamed.Rows.Clear();
            }
            labelCurrentFile.Text = "";
            OpenedXMLFile = null;
            tboxSearchBox.Text = "";
            dv.RowFilter = null;
            DT.Clear();
            DT = GetDataTableFromDGV(dgvASMRenamed);
            if (DS.Tables.CanRemove(DT))
            {
                DS.Tables.Remove(DT);
            }
            DS.Clear();
            DS.Tables.Add(DT);
        }
        
        
        OpenFileDialog OPENAsmDialog = new OpenFileDialog();
        string CurrentASMPath;
        string ASMSimpleName;
        bool ASMOpened = false;
        private void button2_Click(object sender, EventArgs e)
        {
            OPENAsmDialog.Filter = "ASM Files | *.asm";
            if (OPENAsmDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentASMPath = OPENAsmDialog.FileName;
                LoadASMFileIntoStringArray();
                ASMSimpleName = OPENAsmDialog.SafeFileName;
                ASMOpened = true;
            }
        }

        string[] ASMFileArray;
        string[] RenamedASMFileArray;
        private void LoadASMFileIntoStringArray()
        {
            ASMFileArray = System.IO.File.ReadAllLines(CurrentASMPath);
        }

        //StreamWriter objStreamWriter;
        private void button3_Click(object sender, EventArgs e)
        {
            if (ASMOpened)
            {
                try
                {

                    RenamedASMFileArray = System.IO.File.ReadAllLines(CurrentASMPath);
                    ReadXMLFileIntoDataset();
                    MessageBox.Show("The renamed file will be saved as RENAMED_" + ASMSimpleName+"\n Please be patient with large files and long rename lists." +
                        "\nYou will be notified when the process is complete.");
                    RenameUsingValuesFromXML(RenamedASMFileArray);
                    string RenamedAsmName = System.IO.Path.GetDirectoryName(CurrentASMPath) + "\\" + "RENAMED_" + ASMSimpleName;
                    using (StreamWriter outputfile = new StreamWriter(RenamedAsmName))
                    {
                        foreach (string line in RenamedASMFileArray)
                            outputfile.WriteLine(line);
                    }
                    MessageBox.Show("File saved as RENAMED_" + ASMSimpleName);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else
            {
                MessageBox.Show("Open an ASM file that you would like to rename first.");
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
                foreach (DataTable table in dataset1.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        if (RestoreToOriginalValue)
                        {

                            OrigArray[index] = CaseSenstiveReplace(OrigArray[index], Convert.ToString(dr[2]), Convert.ToString(dr[1]));
                        }
                        else if (Convert.ToString(dr[0]) == "True")
                        {
                            OrigArray[index] = CaseSenstiveReplace(OrigArray[index], Convert.ToString(dr[1]), Convert.ToString(dr[2]));
                        }


                    }
                }

            }


        }
        public string CaseSenstiveReplace(string originalString, string oldValue, string newValue)
        {
            if (oldValue != "")
            {
                return originalString.Replace(oldValue, newValue);
            }
            else
            {
                return originalString;
            }
        }
    }
}
