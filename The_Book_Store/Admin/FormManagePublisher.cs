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
    public partial class FormManagePublisher : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader mySqlDataReader = null;
        public FormManagePublisher()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadRecords();
        }
        private void BtnNewPublisher_Click(object sender, EventArgs e)
        {
            FormPublisherModule2 publisher = new FormPublisherModule2(this, true);
            publisher.ShowDialog();
        }
        public void LoadRecords()
        {
            int i = 0;
            dataGridViewPublisher.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblPublisher WHERE publisher LIKE '" + textBoxSearch.Text + "%' ORDER BY publisher", cn);
            mySqlDataReader = cm.ExecuteReader();
            while(mySqlDataReader.Read())
            {
                i++;
                dataGridViewPublisher.Rows.Add(i, mySqlDataReader["id"].ToString(), mySqlDataReader["publisher"].ToString());
            }
            mySqlDataReader.Close();
            cn.Close();
        }
        private void dataGridViewPublisher_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridViewPublisher.Columns[e.ColumnIndex].Name;
            if (colName != "Edit" && colName != "Delete")
            {
                return;
            }
            if (colName == "Edit")
            {
                FormPublisherModule2 formPublisherModule = new FormPublisherModule2(this, false);
                formPublisherModule.labelID.Text = dataGridViewPublisher[1, e.RowIndex].Value.ToString();
                formPublisherModule.textBoxPublisher.Text = dataGridViewPublisher[2, e.RowIndex].Value.ToString();
                formPublisherModule.ShowDialog();
            } else if (colName == "Delete")
            {
                if(MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        cn.Open();
                        cm = new SqlCommand("DELETE FROM tblPublisher WHERE id LIKE '" + dataGridViewPublisher[1, e.RowIndex].Value.ToString() + "'", cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Publisher has been successfully deleted!", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRecords();
                    }
                    catch(Exception ex)
                    {
                        cn.Close();
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }
    }
}
