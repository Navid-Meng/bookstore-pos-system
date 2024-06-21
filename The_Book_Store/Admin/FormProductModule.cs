using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Book_Store.Admin.classes;

namespace The_Book_Store.Admin
{
    public partial class FormProductModule : Form
    {
        private bool isNew;
        private FormManageBook formManageBook;
        private readonly BookManager _bookManager;
        public FormProductModule(FormManageBook formManageBook, bool isNew)
        {
            InitializeComponent();
            this.isNew = isNew;
            this.formManageBook = formManageBook;
            _bookManager = new BookManager();
        }
        public void LoadGenre()
        {
            List<string> genres = _bookManager.GetGenres();
            comboBoxGenre.Items.Clear();
            comboBoxGenre.Items.AddRange(genres.ToArray());
        }
        public void LoadPublisher()
        {
            List<string> publishers = _bookManager.GetPublishers();
            comboBoxPublisher.Items.Clear();    
            comboBoxPublisher.Items.AddRange(publishers.ToArray());
        }
        public void Clear()
        {
            textBoxBookCode.Clear();
            textBoxTitle.Clear();
            textBoxAuthor.Clear();
            comboBoxGenre.Items.Clear();
            comboBoxPublisher.Items.Clear();
            textBoxPrice.Clear();
            textBoxQty.Clear();
            textBoxBookCode.Focus();
        }
        private bool IsTextBoxValid(TextBox textBox)
        {
            return !string.IsNullOrWhiteSpace(textBox.Text);
        }
        private bool IsDropDownListValid(ComboBox dropDown)
        {
            return dropDown.SelectedItem != null;
        }
        private bool IsNumeric(string input)
        {
            return decimal.TryParse(input, out _);
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (IsTextBoxValid(textBoxBookCode) &&
                IsTextBoxValid(textBoxTitle) &&
                IsTextBoxValid(textBoxAuthor) &&
                IsDropDownListValid(comboBoxPublisher) &&
                IsDropDownListValid(comboBoxGenre) &&
                IsTextBoxValid(textBoxPrice) &&
                IsTextBoxValid(textBoxQty))
            {
                if (IsNumeric(textBoxPrice.Text) && IsNumeric(textBoxQty.Text))
                {
                    try
                    {
                        if (MessageBox.Show("Are you sure you want to save this book?", "Save Book", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {

                            string genreName = comboBoxGenre.Text;
                            string publisherName = comboBoxPublisher.Text;
                            Book newBook = new Book
                            {
                                BookCode = textBoxBookCode.Text,
                                BookTitle = textBoxTitle.Text,
                                BookAuthor = textBoxAuthor.Text,
                                BookPublisher = new Publisher { PublisherName = publisherName },
                                BookGenre = new Genre { GenreName = genreName },
                                Price = decimal.Parse(textBoxPrice.Text),
                                Quantity = int.Parse(textBoxQty.Text),
                            };

                            _bookManager.SaveBook(newBook);

                            MessageBox.Show("Book has been saved successfully!");
                            Clear();
                            formManageBook.LoadRecords();
                            LoadPublisher();
                            LoadGenre();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("The value of Price and Qty must be numeric.", "Failed Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("One or more fields are null or empty!", "Failed Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxBookCode.Focus();
            }

        }

        private void FormProductModule_Load(object sender, EventArgs e)
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

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (IsTextBoxValid(textBoxBookCode) &&
                IsTextBoxValid(textBoxTitle) &&
                IsTextBoxValid(textBoxAuthor) &&
                IsDropDownListValid(comboBoxPublisher) &&
                IsDropDownListValid(comboBoxGenre) &&
                IsTextBoxValid(textBoxPrice) &&
                IsTextBoxValid(textBoxQty))
            {
                if (IsNumeric(textBoxPrice.Text) && IsNumeric(textBoxQty.Text))
                {
                    try
                    {
                        if (MessageBox.Show("Are you sure you want to update this book?", "Update Book", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Book editedBook = new Book
                            {
                                BookCode = textBoxBookCode.Text,
                                BookTitle = textBoxTitle.Text,
                                BookAuthor = textBoxAuthor.Text,
                                BookPublisher = new Publisher { PublisherName = comboBoxPublisher.Text },
                                BookGenre = new Genre { GenreName = comboBoxGenre.Text },
                                Price = decimal.Parse(textBoxPrice.Text),
                                Quantity = int.Parse(textBoxQty.Text),
                            };
                            _bookManager.EditBook(editedBook);
                            MessageBox.Show("Book has been updated successfully!");
                            Clear();
                            formManageBook.LoadRecords();
                            this.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        
                    }
                }
                else
                {
                    MessageBox.Show("The value of Price and Qty must be numeric.", "Failed Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("One or more fields are null or empty!", "Failed Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxBookCode.Focus();
            }

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void textBoxQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
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
