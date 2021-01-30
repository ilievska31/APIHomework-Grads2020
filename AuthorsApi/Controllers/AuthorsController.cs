using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthorsApi.Models;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Converters;

namespace AuthorsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApiContext _context;

        public AuthorsController(ApiContext context)
        {
            _context = context;

            if (!_context.Authors.Any())
            {
                List<Author> authors = new List<Author>();
                List<Article> articles = new List<Article>();

                using (StreamReader r = new StreamReader(GetFilePath()))
                {
                    string json = r.ReadToEnd();
                    authors = JsonConvert.DeserializeObject<List<Author>>(json, new IsoDateTimeConverter { DateTimeFormat = "dd/mm/yyyy"});
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

        // GET: api/authors

        /// <summary>
        /// Gets a list of all authors
        /// </summary>


        /// <remarks>
        /// Sample request:
        ///
        ///     GET /authors
        ///     {
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _context.Authors.Include(x=>x.Articles).ToListAsync();
        }

        // GET: api/authors/1

        /// <summary>
        /// Gets an author by id
        /// </summary>
        

        /// <remarks>
        ///     Sample request:
        ///
        ///     GET /authors/1
        ///     {
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.Include(x => x.Articles).FirstOrDefaultAsync(x => x.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        //GET: api/authors/1/articles
        /// <summary>
        /// Search author articles by Id, Title, Level and/or Published Date
        /// </summary>


        /// <remarks>
        /// Sample request:
        ///
        ///     GET /authors/1/articles?title=object
        ///     {
        ///          
        ///     }
        ///
        /// </remarks>

        [HttpGet("{id}/articles")]
        public async Task<ActionResult<List<Article>>> GetAuthorArticlesBy(int id, string title = "", string level = "", string datePublished = "")
        {
            var author = await _context.Authors.Include(x => x.Articles).FirstOrDefaultAsync(x => x.Id == id);
            if (String.IsNullOrWhiteSpace(title) && String.IsNullOrWhiteSpace(level) && String.IsNullOrWhiteSpace(datePublished))
            {
                var articles = author.Articles;
                if (articles == null)
                    return NotFound();

                return articles;
            }
            else if (String.IsNullOrWhiteSpace(title) && String.IsNullOrWhiteSpace(level) && !String.IsNullOrWhiteSpace(datePublished))
            {
                var articles = author.Articles.Where(x => DateTime.Parse(datePublished).Equals(x.DatePublished.Date));
                if (articles == null)
                    return NotFound();

                return articles.ToList();
            }
            else if (String.IsNullOrWhiteSpace(title) && !String.IsNullOrWhiteSpace(level) && String.IsNullOrWhiteSpace(datePublished))
            {

                var articles = author.Articles.Where(x => x.Level.Equals(Capitalize(level)));
                if (articles == null)
                    return NotFound();

                return articles.ToList();
            }
            if (String.IsNullOrWhiteSpace(title) && String.IsNullOrWhiteSpace(level) && String.IsNullOrWhiteSpace(datePublished))
            {
                var articles = author.Articles;
                if (articles == null)
                    return NotFound();

                return articles;
            }
            else if (String.IsNullOrWhiteSpace(title) && String.IsNullOrWhiteSpace(level) && !String.IsNullOrWhiteSpace(datePublished))
            {
                var articles = author.Articles.Where(x => DateTime.Parse(datePublished).Equals(x.DatePublished.Date));
                if (articles == null)
                    return NotFound();

                return articles.ToList();
            }
            else if (String.IsNullOrWhiteSpace(title) && !String.IsNullOrWhiteSpace(level) && String.IsNullOrWhiteSpace(datePublished))
            {

                var articles = author.Articles.Where(x => x.Level.Equals(Capitalize(level)));
                if (articles == null)
                    return NotFound();

                return articles.ToList();
            }
            else if (String.IsNullOrWhiteSpace(title) && !String.IsNullOrWhiteSpace(level) && !String.IsNullOrWhiteSpace(datePublished))
            {

                var articles = author.Articles.Where(x => x.Level.Equals(Capitalize(level)) && DateTime.Parse(datePublished).Equals(x.DatePublished.Date));
                if (articles == null)
                    return NotFound();

                return articles.ToList();
            }
            else if (!String.IsNullOrWhiteSpace(title) && String.IsNullOrWhiteSpace(level) && String.IsNullOrWhiteSpace(datePublished))
            {
                var articles = author.Articles.Where(x => x.Title.Contains(Capitalize(title)));
                if (articles == null)
                    return NotFound();

                return articles.ToList();
            }
            else if (!String.IsNullOrWhiteSpace(title) && String.IsNullOrWhiteSpace(level) && !String.IsNullOrWhiteSpace(datePublished))
            {
                var articles = author.Articles.Where(x => x.Title.Contains(Capitalize(title)) && DateTime.Parse(datePublished).Equals(x.DatePublished.Date));
                if (articles == null)
                    return NotFound();

                return articles.ToList();
            }
            else if (!String.IsNullOrWhiteSpace(title) && !String.IsNullOrWhiteSpace(level) && String.IsNullOrWhiteSpace(datePublished))
            {
                var articles = author.Articles.Where(x => x.Level.Equals(Capitalize(level)) && x.Title.Contains(Capitalize(title)));
                if (articles == null)
                    return NotFound();

                return articles.ToList();
            }
            else if (!String.IsNullOrWhiteSpace(title) && !String.IsNullOrWhiteSpace(level) && !String.IsNullOrWhiteSpace(datePublished))
            {
                var articles = author.Articles.Where(x => x.Level.Equals(Capitalize(level)) && x.Title.Contains(Capitalize(title)) && DateTime.Parse(datePublished).Equals(x.DatePublished.Date));
                if (articles == null)
                    return NotFound();

                return articles.ToList();
            }
            return BadRequest();
        }




        // PUT: api/authors/1


        /// <summary>
        /// Lets admin user update Author by Id
        /// </summary>


        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /authors/1
        ///     {
        ///     
        ///         "id": "1",
        ///         "name": "Johnna Do",
        ///         "username": "jdoe",
        ///         "email": "john.doe@gmail.com"
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, [FromBody] Author author, [FromHeader(Name = "Authorization")] string authorization)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            if (!String.IsNullOrWhiteSpace(authorization)) 
            { 
                if (authorization.Equals("admin"))
                {
                    _context.Entry(author).State = EntityState.Modified;
                    try
                    {

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AuthorExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return Ok();
                }
                else return Unauthorized();
            }
            else return StatusCode(500);

            
        }


        //PUT: api/authors/1/articles/edit/ddc82058-9a67-4926-9314-00faff10ec71z

        /// <summary>
        /// Lets author update article title and/or level by Id 
        /// </summary>

        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /authors/1/articles/edit/ddc82058-9a67-4926-9314-00faff10ec71z
        ///     {
        ///         "Id": "ddc82058-9a67-4926-9314-00faff10ec71",
        ///          "Title": "Structured Programming -  Fundamentals",
        ///          "Level": "Beginner"
        ///     }
        ///     
        ///     
        ///
        /// </remarks>

        [HttpPut("{id}/articles/edit/{articleId}")]
        public async Task<IActionResult> PutAuthorArticle(int id, [FromBody] ArticleDTO article, string articleId, [FromHeader(Name ="Username")] string username)
        {
            if (!articleId.Equals(article.Id))
            {
                return BadRequest();
            }
            var author = await _context.Authors.AsNoTracking().Include(x => x.Articles).FirstOrDefaultAsync(x => x.Id == id);

            if (!String.IsNullOrWhiteSpace(username))
            {
                if (username.Equals(author.Username))
                {
                    var fromContext = author.Articles.FirstOrDefault(x => x.Id == articleId);
                    Article a = new Article();
                    a.Id = articleId;
                    a.Level = article.Level;
                    a.Title = article.Title;
                    a.DatePublished = fromContext.DatePublished;


                    _context.Entry(a).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_context.Articles.FirstOrDefault(x => x.Id == article.Id) == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            else return StatusCode(500);


         


        }

        // POST: api/authors


        /// <summary>
        /// Lets admin user create new author
        /// </summary>


        /// <remarks>
        /// Sample request:
        ///
        ///     POST /authors/
        ///     {
        ///     
        ///         "id": "4",
        ///         "name": "Sarah Jay",
        ///         "username": "sjay",
        ///         "email": "s.jay@gmail.com"
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor([FromBody]AuthorDTO author, [FromHeader(Name = "Authorization")] string admin)
        {
            if (!String.IsNullOrWhiteSpace(admin))
            {
                if (admin.Equals("admin"))
                {
                    Author a = new Author();
                    a.Name = author.Name;
                    a.Email = author.Email;
                    a.Username = author.Username;
                    a.Articles = author.Articles;

                    _context.Authors.Add(a);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetAuthor", new { id = a.Id }, a);
                }
                else return Unauthorized();
            }
            else return StatusCode(500);

        }

       

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }





        //POST api/authors/1/articles/new
        /// <summary>
        /// Lets author create article 
        /// </summary>

        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /authors/1/articles/new
        ///     {
        ///       
        ///          "Title": "Structured Programming -  Fundamentals",
        ///          "Level": "Beginner",
        ///          "DatePublished": "12/12/2019"
        ///     }
        ///     
        ///     
        ///
        /// </remarks>

        [HttpPost("{id}/articles/new")]
        public async Task<IActionResult> PostAuthorArticle(int id, [FromBody] ArticleDTO article, [FromHeader(Name = "Username")] string username)
        {
            var author = await _context.Authors.Include(x => x.Articles).FirstOrDefaultAsync(x => x.Id == id);

            if (!String.IsNullOrWhiteSpace(username))
            {
                if (username.Equals(author.Username))
                {

                    Article a = new Article();
                    a.Id = Guid.NewGuid().ToString();
                    a.Level = article.Level;
                    a.Title = article.Title;
                    a.DatePublished = article.DatePublished;

                    //    _context.Articles.Add(a);
                    author.Articles.Add(a);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_context.Articles.FirstOrDefault(x => x.Id == article.Id) == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return CreatedAtAction("GetAuthor", new { id = author.Id }, author); 
                }
                else
                {
                    return Unauthorized();
                }
            }
            else return StatusCode(500);

            
        }


        // DELETE: api/authors/1


        /// <summary>
        /// Lets admin user delete an author by id
        /// </summary>

        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /authors/1
        ///     {
        ///     
        ///     
        ///     }
        ///
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> DeleteAuthor(int id, [FromHeader(Name = "Authorization")] string admin)
        {
            if (!String.IsNullOrWhiteSpace(admin))
            {
                if (admin.Equals("admin"))
                {

                    var author = await _context.Authors.FindAsync(id);
                    if (author == null)
                    {
                        return NotFound();
                    }

                    _context.Authors.Remove(author);
                    await _context.SaveChangesAsync();

                    return author;
                }

                else return Unauthorized();
            }
            else return StatusCode(500);
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
