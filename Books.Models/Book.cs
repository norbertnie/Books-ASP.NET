using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public Author Author { get; set; }
        //I added the FirstName and LastName
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
