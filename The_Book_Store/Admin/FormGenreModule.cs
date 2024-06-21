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
    public partial class FormGenreModule : Form
    {
        private bool isNew;
        private readonly SqlConnection _connection;
        private readonly DBConnection _db = new DBConnection();
        private FormGenreList formGenreList;
        private BookManager _bookManager;
        public FormGenreModule(FormGenreList formGenreList, bool isNew)
        {
            InitializeComponent();
            _connection = new SqlConnection(_db.MyConnection());
            _bookManager = new BookManager();
            this.formGenreList = formGenreList;
            this.isNew = isNew;
        }
        private void ClearTextBox()
        {
            textBoxGenre.Clear();
            textBoxGenre.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string genreName = textBoxGenre.Text;
            if (!string.IsNullOrWhiteSpace(genreName))
            {
                bool success = _bookManager.SaveGenre(genreName);
                if (success)
                {
                    MessageBox.Show("Genre has been saved successfully!");
                    ClearTextBox();
                    formGenreList.LoadRecords();
                }
            }
            else
            {
                MessageBox.Show("Genre name must be filled!", "Failed Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxGenre.Text))
            {
                int genreId = int.Parse(labelID.Text);
                bool success = _bookManager.EditGenre(genreId,textBoxGenre.Text);
                if (success)
                {
                    MessageBox.Show("Genre has been successfully updated!");
                    ClearTextBox();
                    formGenreList.LoadRecords();
                    this.Dispose();
                }
            }
            else
            {
                MessageBox.Show("Genre name must be filled!", "Failed Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void FormGenreModule_Load(object sender, EventArgs e)
        {
            if (isNew)
            {
                BtnUpdate.Enabled = false;
            }
            else
            {
                BtnSave.Enabled = false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxGenre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (isNew)
                {
                    BtnSave.PerformClick();
                }
                else
                {
                    BtnUpdate.PerformClick();
                }
            }
        }
    }
}
