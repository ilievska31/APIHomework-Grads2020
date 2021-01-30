using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorsApi.Models
{
    
    public class Article
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public DateTime DatePublished { get; set; }
        public String Level { get; set; }


    }

    public class ArticleDTO
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String Level { get; set; }
        public DateTime DatePublished { get; set; }

    }
}
