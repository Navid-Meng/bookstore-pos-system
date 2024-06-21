using Krypton.Toolkit;
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
using System.Xml.Linq;
using The_Book_Store.Admin.classes;

namespace The_Book_Store.Admin
{
    public partial class FormCreateAccount : Form
    {
        UserManager userManager;
        public FormCreateAccount()
        {
            InitializeComponent();
            userManager = new UserManager();
        }
        private void Clear()
        {
            textBoxName.Clear();
            textBoxPassword.Clear();
            textBoxConfirmPassword.Clear();
            textBoxUsername.Clear();
            comboBoxRole.Items.Clear();

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool IsTextBoxValid(TextBox textBox)
        {
            return !string.IsNullOrWhiteSpace(textBox.Text);
        }
        private bool IsComboBoxValid(ComboBox comboBox)
        {
            return comboBox.SelectedItem != null;
        }
        private bool IsTextBoxKrpytonValid(KryptonTextBox kryptonTextBox)
        {
            return !string.IsNullOrWhiteSpace(kryptonTextBox.Text);
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            User user = new User
            {
                Username = textBoxUsername.Text,
                Password = textBoxPassword.Text,
                Role = comboBoxRole.Text,
                Name = textBoxName.Text,

            };
            if(IsTextBoxValid(textBoxUsername) &&
                IsTextBoxKrpytonValid(textBoxPassword) &&
                IsTextBoxKrpytonValid(textBoxConfirmPassword) &&
                IsComboBoxValid(comboBoxRole) &&
                IsTextBoxValid(textBoxName))
            {
                try
                {
                   if (textBoxPassword.Text != textBoxConfirmPassword.Text)
                   {
                        MessageBox.Show("Password not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    bool success = userManager.CreateAccount(user);
                    if (success)
                    {
                        MessageBox.Show("New account has been saved successfully!");
                        Clear();
                        textBoxUsername.Focus();
                    }
                }
                catch (Exception ex)
                {
                    textBoxPassword.Clear();
                    textBoxConfirmPassword.Clear();
                    textBoxUsername.Focus();
                }
            }
            else
            {
                MessageBox.Show("One or more fields are null or empty!", "Failed Create Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxUsername.Focus();
            }
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    BtnSave.PerformClick();
                }
        }
    }
}
