using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Models.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int BookCount { get; set; }
        public DateTime? ReleaseDateOfFirstBook { get; set; }


    }
}
