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
    public partial class FormActivateDeactivate : Form
    {
        //string connectionString = @"Data Source=ViD3107\SQLEXPRESS;Initial Catalog=POS_BOOK;Integrated Security=True";
        UserManager userManager;
        
        public FormActivateDeactivate()
        {
            InitializeComponent();
            userManager = new UserManager();
        }
        public void ClearTextBox()
        {
            textBoxUsername.Clear();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            bool isActive = checkBoxIsActive.Checked;
            bool updatedStatus = userManager.UpdateAccountStatus(username, isActive);   
            if (updatedStatus)
            {
                MessageBox.Show($"Account {username} is now {(isActive ? "activated" : "deactivated")}.");
                ClearTextBox();
            }
            else
            {
                MessageBox.Show("Failed to update account status.");
            }
            
        }
    }
}
