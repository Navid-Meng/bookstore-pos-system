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
using System.Xml.Linq;
using The_Book_Store.Admin;

namespace The_Book_Store.Cashier
{
    public partial class formSettle : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        formCashier form;
        cashier order = new cashier();
        public formSettle(formCashier fc)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            form = fc;
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double sale = double.Parse(txtSale.Text);
                double cash = double.Parse(txtCash.Text);
                double change = cash - sale;
                if(change < 0) {
                    txtChange.Text = "0.00";
                }
                else
                {
                    txtChange.Text = change.ToString("#,##0.00");
                }
            }catch(Exception ex)
            {
                txtChange.Text = "0.00";
            }



        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn1.Text;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn2.Text;

        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn3.Text;

        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn4.Text;

        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn5.Text;

        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn6.Text;

        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn7.Text;

        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn8.Text;

        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn9.Text;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCash.Text = String.Empty;

        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn0.Text;

        }

        private void btn00_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn00.Text;

        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (double.Parse(txtCash.Text) <= 0 || double.Parse(txtCash.Text)<double.Parse(txtSale.Text))
                {
                    MessageBox.Show("insufficient amount", "warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    
                    for(int i = 0; i<form.dataGridView1.Rows.Count; i++)
                    {
                        order.payment(int.Parse(form.dataGridView1.Rows[i].Cells[4].Value.ToString()), form.dataGridView1.Rows[i].Cells[2].Value.ToString(), int.Parse(form.dataGridView1.Rows[i].Cells[1].Value.ToString()));

                        //cn.Open();
                        //cm = new SqlCommand("update tblBook set qty = qty - " + int.Parse(form.dataGridView1.Rows[i].Cells[4].Value.ToString())+"where bookCode = '" + form.dataGridView1.Rows[i].Cells[2].Value.ToString()+ "'", cn);
                        //cm.ExecuteNonQuery();
                        //cn.Close();

                        //cn.Open();
                        //cm = new SqlCommand("update tblCart set status = 'Sold' where id like '"+ int.Parse(form.dataGridView1.Rows[i].Cells[1].Value.ToString()) +"'", cn);
                        //cm.ExecuteNonQuery();
                        //cn.Close();
                    }
                    MessageBox.Show("Payment Saved Successfully!","Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    form.loadCart();
                    form.GetTransNo();
                    this.Dispose();

                    
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void formSettle_Load(object sender, EventArgs e)
        {

        }
    }
}
