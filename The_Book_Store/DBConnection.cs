using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Book_Store.Admin
{
    //public class DBConnection
    //{
    //    public string MyConnection ()
    //    {
    //        string installationDirectory = AppDomain.CurrentDomain.BaseDirectory;
    //        string connectionStringFilePath = Path.Combine(installationDirectory, "ConnectionString.txt");


    //        string connectionString = "";
    //        try
    //        {
    //            connectionString = File.ReadAllText(connectionStringFilePath);
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show("Error reading the connection string file: " + ex.Message);
    //        }

    //        return connectionString;
    //    }

    //    public static implicit operator DBConnection(SqlConnection v)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class DBConnection
    {
        public string MyConnection()
        {

            //string connectionString = Properties.Settings.Default.ConnectionString;
            var th = Properties.Settings.Default.ConnectionString = "";

            string connectionString = th;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("You will have to select file ConnectionString.txt");
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select Connection String File";
                openFileDialog.Filter = "Text files (*.txt)|*.txt";

                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    try
                    {
                        connectionString = File.ReadAllText(openFileDialog.FileName);
                        Properties.Settings.Default.ConnectionString = connectionString;
                        Properties.Settings.Default.Save();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading the connection string file: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("No file selected. Application cannot proceed.");
                }
            }

            return connectionString;
        }
        public void ClearConnectionString()
        {
            Properties.Settings.Default.ConnectionString = "";
            Properties.Settings.Default.Save();
            MessageBox.Show("Connection string cleared. You will need to select a new file.");
        }
    }
}
