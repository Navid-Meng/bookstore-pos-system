using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Book_Store.Admin
{
    public partial class FormStockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader sqlDataReader = null;
        private bool unsavedChanges = false;
        public FormStockIn()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }
        
        private bool AreThereUnsavedChanges()
        {
            return dataGridViewStockIn.Rows.Count > 0;
        }
        private void CheckForChangesAndUpdateFlag()
        {
            unsavedChanges = AreThereUnsavedChanges();
        }
        private void BtnBrowseProduct_Click(object sender, EventArgs e)
        {
            FormBrowseProduct formBrowseProduct = new FormBrowseProduct(this);
            formBrowseProduct.LoadProduct();
            formBrowseProduct.ShowDialog();
        }

        private void dataGridViewStockIn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridViewStockIn.Columns[e.ColumnIndex].Name;
            if (colName != "Delete")
            {
                return;
            }
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblStockIn WHERE bookCode LIKE '" + dataGridViewStockIn.Rows[e.RowIndex].Cells[2].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadStockIn();
                }
            }
        }
        public void LoadStockIn()
        {
            int i = 0;
            dataGridViewStockIn.Rows.Clear();
            try
            {
                cn.Open();
                //cm = new SqlCommand("SELECT dbo.tblStockIn.id, dbo.tblStockIn.refNo, dbo.tblStockIn.bookCode, dbo.tblBook.bookTitle, dbo.tblStockIn.qty, dbo.tblStockIn.stockInDate, dbo.tblStockIn.stockInBy FROM dbo.tblBook INNER JOIN dbo.tblStockIn ON dbo.tblBook.bookCode = dbo.tblStockIn.bookCode");
                cm = new SqlCommand("SELECT SI.id, SI.refno, SI.bookCode, B.bookTitle, SI.qty, SI.stockInDate, SI.stockInBy " +
                    "FROM dbo.tblBook AS B " +
                    "INNER JOIN dbo.tblStockIn AS SI ON B.bookCode = SI.bookCode", cn); 
                sqlDataReader = cm.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    i++;
                    dataGridViewStockIn.Rows.Add(sqlDataReader[0].ToString(), sqlDataReader[1].ToString(), sqlDataReader[2].ToString(), sqlDataReader[3].ToString(), sqlDataReader[4].ToString(), sqlDataReader[5].ToString(), sqlDataReader[6].ToString());

                }
                sqlDataReader.Close();
                cn.Close();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                cn.Close();
            }
            CheckForChangesAndUpdateFlag();
        }
        private void SaveData()
        {
            try
            {
                if (dataGridViewStockIn.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure you want to save these records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cn.Open();
                        for (int i = 0; i < dataGridViewStockIn.Rows.Count; i++)
                        {
                            cm = new SqlCommand("UPDATE tblBook SET qty = " + int.Parse(dataGridViewStockIn.Rows[i].Cells[4].Value.ToString()) + " WHERE bookCode LIKE '" + dataGridViewStockIn.Rows[i].Cells[2].Value.ToString() + "'", cn);
                            cm.ExecuteNonQuery();
                        }
                        cn.Close();
                        cn.Open();
                        cm = new SqlCommand("DELETE FROM tblStockIn", cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadStockIn();
                        MessageBox.Show("Stock has been updated successfully!");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        private string GenreateReferenceNo()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            Random random = new Random();
            string randomDigits = random.Next(1000, 9999).ToString();
            string referenceNumber = $"REF-{timestamp}-{randomDigits}";
            return referenceNumber;
        }

        private void BtnGenerateRefNo_Click(object sender, EventArgs e)
        {
            string referenceNum = GenreateReferenceNo();
            textBoxRefNo.Text = referenceNum;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (dataGridViewStockIn.Rows.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to clear these records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dataGridViewStockIn.Rows.Clear();
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblStockIn", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    CheckForChangesAndUpdateFlag();
                }
            }
        }

        private void FormStockIn_Load(object sender, EventArgs e)
        {
            CheckForChangesAndUpdateFlag();
        }

        private void FormStockIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && unsavedChanges)
            {
                DialogResult result = MessageBox.Show("There are unsaved changes. Do you want to save before closing?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    e.Cancel = true;
                    SaveData();
                    this.Close();
                } 
                else if(result == DialogResult.No)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblStockIn", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                } 
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
