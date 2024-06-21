using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Book_Store.Admin;

namespace The_Book_Store.Cashier
{
    internal class cashier
    {
        //field
        private int transno;
        private int bookCode;
        private double price;
        private int qty;
        private DateTime sdate;
        private string name;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();

        public  cashier() {
            cn = new SqlConnection(dbcon.MyConnection());
           

        }

        // method
        public void addBookToCart(string transno, string bookCode, double price, string qty, DateTime sdate, string name)
        {
            cn.Open();
            cm = new SqlCommand("insert into tblCart (transno, pid, price, qty, sdate, cashierName) values(@transno, @pid, @price, @qty, @sdate,@cashierName)", cn);
            cm.Parameters.AddWithValue("@transno", transno);
            cm.Parameters.AddWithValue("@pid", bookCode);
            cm.Parameters.AddWithValue("@price", price);
            cm.Parameters.AddWithValue("@qty", int.Parse(qty));
            cm.Parameters.AddWithValue("@sdate", DateTime.Now);
            cm.Parameters.AddWithValue("@cashierName", name);
            cm.ExecuteNonQuery();
            cn.Close();
         
        }

        private void updateStock(int qty, string bookCode)
        {
            cn.Open();
            cm = new SqlCommand("update tblBook set qty = qty - " + qty + "where bookCode = '" + bookCode + "'", cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }

        private void updateCardStatus(int invoiceNo)
        {
            cn.Open();
            cm = new SqlCommand("update tblCart set status = 'Sold' where id like '" + invoiceNo + "'", cn);
            cm.ExecuteNonQuery(); cn.Close();
        }

        public void payment(int qty, string bookCode, int invoiceNo)
        {
            updateStock(qty, bookCode);
            updateCardStatus(invoiceNo);
        }

    }
}
