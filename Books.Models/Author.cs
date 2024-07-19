using System.Collections.Generic;

namespace Books.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public List<Book> Books { get; set; }
    }
}