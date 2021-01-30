using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorsApi.Models
{
    public class Author
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public List<Article> Articles { get; set; }
    }

    public class AuthorDTO
    {
        public String Name { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public List<Article> Articles { get; set; }
    }
}

