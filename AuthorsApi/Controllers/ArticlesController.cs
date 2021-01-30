using AuthorsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private readonly ApiContext _context;

        public ArticlesController(ApiContext context)
        {
            _context = context;

            if (!_context.Authors.Any())
            {
                List<Author> authors = new List<Author>();
                List<Article> articles = new List<Article>();

                using (StreamReader r = new StreamReader(GetFilePath()))
                {
                    string json = r.ReadToEnd();
                    authors = JsonConvert.DeserializeObject<List<Author>>(json, new IsoDateTimeConverter { DateTimeFormat = "dd/mm/yyyy" });
                }

                foreach (Author a in authors)
                {
                    articles.AddRange(a.Articles);
                }
                _context.Authors.AddRange(authors);
                _context.Articles.AddRange(articles);
                _context.SaveChanges();
            }
        }

        // GET: api/articles

        /// <summary>
        /// Get a list of all articles
        /// </summary>


        /// <remarks>
        /// Sample request:
        ///
        ///     GET /articles
        ///     {
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetAuthors()
        {
            return await _context.Articles.ToListAsync();
        }


        //GET: api/articles/published-after/2018
       
        /// <summary>
        ///Filter articles by year
        /// </summary>


        /// <remarks>
        /// Sample request:
        ///
        ///     GET /articles/published-after/2018
        ///     {
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpGet("published-after/{year}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlesAfter(int year)
        {
            return await _context.Articles.Where(x=>x.DatePublished.Year>year).ToListAsync();
        }



        //GET: api/articles/level/beginner

        /// <summary>
        ///Filter articles by level
        /// </summary>


        /// <remarks>
        /// Sample request:
        ///
        ///     GET /articles/level/beginner
        ///     {
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpGet("level/{level}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlesByLevel(string level)
        {
            return await _context.Articles.Where(x => x.Level.Equals(Capitalize(level))).ToListAsync();
        }

        private string GetFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Authors.json");
        }


        private string Capitalize(string before)
        {

            string after = char.ToUpper(before.First()) + before.Substring(1).ToLower();
            return after;
        }

    }
}
