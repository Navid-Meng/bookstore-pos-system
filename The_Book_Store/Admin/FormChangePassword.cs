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
    public partial class FormChangePassword : Form
    {
        UserManager UserManager;
        public FormChangePassword()
        {
            InitializeComponent();
            UserManager = new UserManager();
        }
        public void ClearTextBox()
        {
            textBoxUsername.Clear();
            textBoxOldPassword.Clear();
            textBoxNewPassword.Clear();
            textBoxConfirmPassword.Clear();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string oldPassword = textBoxOldPassword.Text;
            string newPassword = textBoxNewPassword.Text;
            string confirmPassword = textBoxConfirmPassword.Text;
            if (newPassword == confirmPassword)
            {
                bool passwordChanged = UserManager.ChangePassword(username, oldPassword, newPassword);
                if (passwordChanged)
                {
                    MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to change password. Please check your credentials.", "User Settings",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("New password and confirm password do not match.", "User Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnSave.PerformClick();
            }
        }
    }
}
