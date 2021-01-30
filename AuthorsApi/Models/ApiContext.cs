using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorsApi.Models;

namespace AuthorsApi.Models
{
    public class ApiContext:DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }
     




    }

}
