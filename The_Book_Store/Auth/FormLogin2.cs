using Book_Storee.Forms.ChasierForm;
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
using The_Book_Store.Admin;

namespace The_Book_Store.Auth
{
    public partial class FormLogin2 : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        
        public FormLogin2()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string role = "";
            bool isAccountActive = false;
            try
            {
                bool found = false;
                cn.Open();
                cm = new SqlCommand("select * from tblUser where username = @username and password = @password ", cn);
                cm.Parameters.AddWithValue("@username", textBoxUsername.Text);
                cm.Parameters.AddWithValue("@password", textBoxPassword.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {

                    found = true;
                    role = dr["role"].ToString();
                    isAccountActive = Convert.ToBoolean(dr["isActive"]);
                }
                else { found = false; }

                if (found && isAccountActive)
                {

                    if (role == "Cashier")
                    {
                        this.Hide();
                        formCashier formCashier = new formCashier();
                        formCashier.lblUsername.Text = textBoxUsername.Text;
                        formCashier.lblRole.Text = role;
                        formCashier.Show();
                    }
                    else if (role == "Admin")
                    {
                        this.Hide();
                        FormAdminNew form = new FormAdminNew();
                        form.ShowDialog();

                    }
                }
                else if(found && !isAccountActive)
                {
                    MessageBox.Show("Your account is deactivated. Please contact an administrator.", "Account Deactivated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxPassword.Clear();
                }
                else
                {
                    MessageBox.Show("wrong credential!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPassword.Clear();
                }
                dr.Close();
                cn.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormLogin2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnLogin.PerformClick();
            }
        }
    }
}
