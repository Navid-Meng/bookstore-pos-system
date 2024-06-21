using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Book_Store.Admin.classes
{
    public class UserManager
    {
        private readonly DBConnection _db = new DBConnection();
        public bool CreateAccount(User newUser)
        {
            bool confirmMsg = MessageBox.Show("Are you sure you want to create this account?", "Creating Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            if (confirmMsg)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_db.MyConnection()))
                    {
                        connection.Open();
                        string query = "INSERT INTO tblUser (username, password, role, name) VALUES (@username, @password, @role, @name)";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@username", newUser.Username);
                        cmd.Parameters.AddWithValue("@password", newUser.Password);
                        cmd.Parameters.AddWithValue("@role", newUser.Role);
                        cmd.Parameters.AddWithValue("@name", newUser.Name);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Username [{newUser.Username}] has already taken.", "Failed Create Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return false;
        }
        public bool ChangePassword(string username, string currentPassword, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(_db.MyConnection()))
            {
                connection.Open();
                string query = "SELECT password FROM tblUser WHERE username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader["password"].ToString();
                            if (storedPassword == currentPassword)
                            {
                                reader.Close();
                                string updateQuery = "UPDATE tblUser SET password = @NewPassword WHERE username = @Username";
                                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                                    updateCommand.Parameters.AddWithValue("@Username", username);
                                    int rowsAffected = updateCommand.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }
        public bool UpdateAccountStatus(string username, bool isActive)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_db.MyConnection()))
                {
                    conn.Open();
                    string updateQuery = "UPDATE tblUser SET isActive = @IsActive WHERE username = @Username";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@IsActive", isActive);
                        cmd.Parameters.AddWithValue("@Username", username);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
