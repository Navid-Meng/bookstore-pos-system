using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Book_Store.Admin.classes
{
    public class BookManager
    {
        private readonly SqlConnection _connection;
        private readonly DBConnection _dbConnection = new DBConnection();
        public BookManager()
        {
            _connection = new SqlConnection(_dbConnection.MyConnection());
        }
        public List<Book> SearchBooks(string searchText)
        {
            var books = new List<Book>();
            var query = "SELECT p.bookCode, p.bookTitle, p.bookAuthor, b.publisher, c.genre, p.price, p.qty FROM tblBook AS p INNER JOIN tblPublisher AS b ON b.id = p.publisherID INNER JOIN tblGenre AS c ON c.id = p.genreID WHERE p.bookTitle LIKE @searchText";
            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@searchText", searchText + "%");
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        var publisher = new Publisher
                        {
                            PublisherName = reader["publisher"].ToString(),
                        };
                        var genre = new Genre
                        {
                            GenreName = reader["genre"].ToString()
                        };
                        var book = new Book
                        {
                            BookCode = reader[0].ToString(),
                            BookTitle = reader[1].ToString(),
                            BookAuthor = reader[2].ToString(),
                            BookPublisher = publisher,
                            BookGenre = genre,
                            Price = Convert.ToDecimal(reader[5]),
                            Quantity = Convert.ToInt32(reader[6]),

                        };
                        books.Add(book);
                    }
                }
                _connection.Close();
            }
            return books;
        }
        public void DeleteBook(string bookCode)
        {
            bool confirmDelete = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            if (confirmDelete)
            {
                string query = "DELETE FROM tblBook WHERE bookCode = @bookCode";
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@bookCode", bookCode);
                    _connection.Open();
                    command.ExecuteNonQuery();
                    _connection.Close();
                }
            }
        }
        public void EditBook(Book editedBook)
        {
            try
            {
                int genreId = GetGenreIdByName(editedBook.BookGenre.GenreName);

                int publisherId = GetPublisherIdByName(editedBook.BookPublisher.PublisherName);
                _connection.Open();
                string queryCheck = "SELECT COUNT(*) FROM tblBook WHERE bookCode = @bookCode";
                SqlCommand checkCmd = new SqlCommand(queryCheck, _connection);
                checkCmd.Parameters.AddWithValue("@bookCode", editedBook.BookCode);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {
                    string queryUpdate = "UPDATE tblBook SET bookTitle = @bookTitle, bookAuthor = @bookAuthor, publisherID = @publisherID, genreID = @genreID, price = @price, qty = @qty WHERE bookCode = @bookCode";
                    SqlCommand updateCmd = new SqlCommand(queryUpdate, _connection);
                    updateCmd.Parameters.AddWithValue("@bookCode", editedBook.BookCode);
                    updateCmd.Parameters.AddWithValue("@bookTitle", editedBook.BookTitle);
                    updateCmd.Parameters.AddWithValue("@bookAuthor", editedBook.BookAuthor);
                    updateCmd.Parameters.AddWithValue("@publisherID", publisherId);
                    updateCmd.Parameters.AddWithValue("@genreID", genreId);
                    updateCmd.Parameters.AddWithValue("@price", editedBook.Price);
                    updateCmd.Parameters.AddWithValue("@qty", editedBook.Quantity);
                    updateCmd.ExecuteNonQuery();
                }
                _connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public List<string> GetGenres()
        {
            List<string> genres = new List<string>();
            try
            {
                _connection.Open();
                string query = "SELECT genre FROM tblGenre";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            genres.Add(reader["genre"].ToString());
                        }
                    }
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _connection.Close();
            }
            return genres;
        }
        public List<string> GetPublishers()
        {
            List<string> publishers = new List<string>();
            try
            {
                _connection.Open();
                string query = "SELECT publisher FROM tblPublisher";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            publishers.Add(reader["publisher"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { _connection.Close();}


            return publishers;
        }
        public int GetGenreIdByName(string genreName)
        {
            int genreId = 0;
            try
            {
                _connection.Open();
                string query = "SELECT id FROM tblGenre WHERE genre LIKE @genreName";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@genreName", genreName);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    genreId = Convert.ToInt32(reader["id"]);
                }
                reader.Close();
                _connection.Close();
            }
           catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return genreId;
        }
        public int GetPublisherIdByName(string publisherName)
        {
            int publisherId = 0;
            try
            {
                _connection.Open();
                string query = "SELECT id FROM tblPublisher WHERE publisher LIKE @publisherName";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@publisherName", publisherName);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    publisherId = Convert.ToInt32(reader["id"]);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return publisherId;
        }
        public void SaveBook(Book newBook)
        {
            try
            {
                int genreId = GetGenreIdByName(newBook.BookGenre.GenreName);
                int publisherId = GetPublisherIdByName(newBook.BookPublisher.PublisherName);
                _connection.Open();
                string query = "INSERT INTO tblBook(bookCode, bookTitle, bookAuthor, publisherID, genreID, price, qty) VALUES (@bookCode, @bookTitle, @bookAuthor, @publisherID, @genreID, @price, @qty)";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@bookCode", newBook.BookCode);
                    command.Parameters.AddWithValue("@bookTitle", newBook.BookTitle);
                    command.Parameters.AddWithValue("@bookAuthor", newBook.BookAuthor);
                    command.Parameters.AddWithValue("@publisherId", publisherId);
                    command.Parameters.AddWithValue("@genreId", genreId);
                    command.Parameters.AddWithValue("@price", newBook.Price);
                    command.Parameters.AddWithValue("@qty", newBook.Quantity);

                    command.ExecuteNonQuery();
                }
                _connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool SaveGenre(string genre)
        {
            try
            {
                _connection.Open();
                string queryCheck = "SELECT COUNT(*) FROM tblGenre WHERE Genre = @genre";
                SqlCommand cmdCheck = new SqlCommand(queryCheck, _connection);
                cmdCheck.Parameters.AddWithValue("@genre", genre);
                int count = (int)cmdCheck.ExecuteScalar();
                _connection.Close();
                if (count > 0)
                {
                    MessageBox.Show("This genre already exists!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bool confirmMsg = MessageBox.Show("Are you sure you want to save this genre?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    if(confirmMsg)
                    {
                        _connection.Open();
                        string querySave = "INSERT INTO tblGenre(genre) VALUES(@genre)";
                        SqlCommand cmdSave = new SqlCommand(querySave, _connection);
                        cmdSave.Parameters.AddWithValue("@genre", genre);
                        cmdSave.ExecuteNonQuery();
                        _connection.Close();
                        return true;
                    }
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return false;
        }
        public bool EditGenre(int genreId,string newGenre)
        {
            try
            {
                _connection.Open();
                string queryCheck = "SELECT COUNT(*) FROM tblGenre WHERE Genre = @genre";
                SqlCommand cmdCheck = new SqlCommand(queryCheck, _connection);
                cmdCheck.Parameters.AddWithValue("@genre", newGenre);
                int count = (int)cmdCheck.ExecuteScalar();
                _connection.Close();
                if (count > 0)
                {
                    MessageBox.Show("This genre already exists!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bool confirmMsg = MessageBox.Show("Are you sure you want to save this genre?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    if (confirmMsg)
                    {
                        _connection.Open();
                        string query = "UPDATE tblGenre SET Genre = @genre WHERE Id = @genreId";
                        SqlCommand command = new SqlCommand(query, _connection);
                        command.Parameters.AddWithValue("@genre", newGenre);
                        command.Parameters.AddWithValue("@genreId", genreId);
                        command.ExecuteNonQuery();
                        _connection.Close();
                        return true;
                    }
                }
                _connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return false;
        }
        public bool DeleteGenre(int genreId)
        {
            //try
            //{
            //    _connection.Open();
            //    string queryCheck = "SELECT COUNT(*) FROM tblGenre WHERE Genre = @genre";
            //    SqlCommand cmdCheck = new SqlCommand(queryCheck, _connection);
            //    cmdCheck.Parameters.AddWithValue("@genre", genre);
            //    int count = (int)cmdCheck.ExecuteScalar();
            //    _connection.Close();
            //    if (count > 0)
            //    {
            //        MessageBox.Show("This genre already exists!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    else
            //    {
            //        _connection.Open();
            //        string query = "DELETE FROM tblGenre WHERE Id = @genreId";
            //        SqlCommand cmd = new SqlCommand(query, _connection);
            //        cmd.Parameters.AddWithValue("@genreId", genreId);
            //        cmd.ExecuteNonQuery();
            //        _connection.Close();
            //        return true;
            //    }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    return false;
            //}
            return false;
        }
    }
}
