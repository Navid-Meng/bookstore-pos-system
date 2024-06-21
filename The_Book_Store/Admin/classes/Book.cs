using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Book_Store.Admin.classes
{
    public class Book
    {
        public string BookCode { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public Publisher BookPublisher { get; set; }
        public Genre BookGenre { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
