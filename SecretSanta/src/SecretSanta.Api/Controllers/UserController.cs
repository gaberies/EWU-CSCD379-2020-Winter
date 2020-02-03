using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IAuthorService AuthorService { get; }

        public AuthorController(IAuthorService authorService)
        {
            AuthorService = authorService ?? throw new System.ArgumentNullException(nameof(authorService));
        }

        // GET: https://localhost/api/Author
        [HttpGet]
        public async Task<IEnumerable<Author>> Get()
        {
            List<Author> authors = await AuthorService.FetchAllAsync();
            return authors;
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Author>> Get(int id)
        {
            if (await AuthorService.FetchByIdAsync(id) is Author author)
            {
                return Ok(author);
            }
            return NotFound();
        }

        // POST: api/Author
        [HttpPost]
        public async Task<Author> Post(Author value)
        {
            return await AuthorService.InsertAsync(value);
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Author>> Put(int id, Author value)
        {
            if (await AuthorService.UpdateAsync(id, value) is Author author)
            {
                return author;
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}