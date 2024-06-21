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
using MySql.Data.MySqlClient;
using The_Book_Store.Admin.classes;

namespace The_Book_Store.Admin
{
    public partial class FormGenreList : Form
    {
        private readonly SqlConnection _connection;
        private readonly DBConnection _db = new DBConnection();
        private BookManager _bookManager;
        public FormGenreList()
        {
            InitializeComponent();
            _connection = new SqlConnection(_db.MyConnection());
            _bookManager = new BookManager();
            LoadRecords();
        }

        private void BtnNewGenre_Click(object sender, EventArgs e)
        {
            FormGenreModule formGenreModule = new FormGenreModule(this, true);
            formGenreModule.ShowDialog();
        }
        public void LoadRecords()
        {
            int i = 0;
            dataGridViewGenre.Rows.Clear();
            _connection.Open(); 
            SqlCommand cm = new SqlCommand("SELECT * FROM tblGenre WHERE genre LIKE '" + textBoxSearch.Text + "%'", _connection);
            SqlDataReader reader = cm.ExecuteReader();
            while (reader.Read())
            {
                i++;
                dataGridViewGenre.Rows.Add(i, reader["id"].ToString(), reader["genre"].ToString());
            }
            reader.Close();
            _connection.Close();
        }

        private void dataGridViewGenre_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridViewGenre.Columns[e.ColumnIndex].Name;
            if (colName != "Edit" && colName != "Delete")
            {
                return;
            }
            if (colName == "Edit")
            {
                FormGenreModule formGenreModule = new FormGenreModule(this, false);
                formGenreModule.labelID.Text = dataGridViewGenre[1, e.RowIndex].Value.ToString();
                formGenreModule.textBoxGenre.Text = dataGridViewGenre[2, e.RowIndex].Value.ToString();
                formGenreModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        _connection.Open();
                        SqlCommand cm = new SqlCommand("DELETE FROM tblGenre WHERE id LIKE '" + dataGridViewGenre[1, e.RowIndex].Value.ToString() + "'", _connection);
                        cm.ExecuteNonQuery();
                        _connection.Close();
                        MessageBox.Show("Publisher has been successfully deleted!", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRecords();
                    }
                    catch (Exception ex)
                    {
                        _connection.Close();
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
