using MySqlX.XDevAPI.Relational;
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
using The_Book_Store.Admin.classes;

namespace The_Book_Store.Admin
{
    public partial class FormManageBook : Form
    {
        private readonly BookManager _bookManager = new BookManager();
        public FormManageBook()
        {
            InitializeComponent();
        }

        private void BtnNewBook_Click(object sender, EventArgs e)
        {
            FormProductModule formProductModule = new FormProductModule(this, true);
            formProductModule.LoadGenre();
            formProductModule.LoadPublisher();
            formProductModule.ShowDialog();
        }
        public void LoadRecords()
        {
            int i = 0;
            dataGridViewManageBook.Rows.Clear();
            var books = _bookManager.SearchBooks(textBoxSearch.Text);
            foreach(var book in books )
            {
                i++;
                dataGridViewManageBook.Rows.Add(i, book.BookCode, book.BookTitle, book.BookAuthor, book.BookPublisher.PublisherName, book.BookGenre.GenreName, book.Price, book.Quantity);
            }
            
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void dataGridViewManageBook_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridViewManageBook.Columns[e.ColumnIndex].Name;
            if (colName != "Edit" && colName != "Delete")
            {
                return;
            }
            DataGridViewRow row = dataGridViewManageBook.Rows[e.RowIndex];
            if (colName == "Edit")
            {
                FormProductModule formProductModule = new FormProductModule(this, false);
                formProductModule.LoadGenre();
                formProductModule.LoadPublisher();

                formProductModule.textBoxBookCode.Text = dataGridViewManageBook.Rows[e.RowIndex].Cells[1].Value.ToString();
                formProductModule.textBoxTitle.Text = dataGridViewManageBook.Rows[e.RowIndex].Cells[2].Value.ToString();
                formProductModule.textBoxAuthor.Text = dataGridViewManageBook.Rows[e.RowIndex].Cells[3].Value.ToString();
                formProductModule.comboBoxPublisher.SelectedItem = dataGridViewManageBook.Rows[e.RowIndex].Cells[4].Value.ToString();

                formProductModule.comboBoxGenre.SelectedItem = dataGridViewManageBook.Rows[e.RowIndex].Cells[5].Value.ToString();
             
                formProductModule.textBoxPrice.Text = dataGridViewManageBook.Rows[e.RowIndex].Cells[6].Value.ToString();
                formProductModule.textBoxQty.Text = dataGridViewManageBook.Rows[e.RowIndex].Cells[7].Value.ToString();
                formProductModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                string bookCode = row.Cells["BookCode"].Value.ToString();
                _bookManager.DeleteBook(bookCode);  
                LoadRecords();
            }
        }
    }
}
