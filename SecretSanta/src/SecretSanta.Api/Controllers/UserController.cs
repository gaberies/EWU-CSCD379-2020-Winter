using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService UserService { get; }

        public UserController(IUserService userService)
        {
            UserService = userService ?? throw new System.ArgumentNullException(nameof(userService));
        }

        // GET: https://localhost/api/User
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<User>>> Get()
        {
            if(await UserService.FetchAllAsync() is List<User> users && users != null && users.Count != 0)
            {
                return Ok(users);
            }
            return NotFound();
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Get(int id)
        {
            if (await UserService.FetchByIdAsync(id) is User user && user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        // POST: api/Author
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> Post(User? value)
        {
            if(value != null)
            {
                await UserService.InsertAsync(value);
                return Ok(value);
            }
            return BadRequest(value);
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<User>> Put(int id, User value)
        {
            if(value != null && UserService.FetchByIdAsync(id) is Task<User> userResult)
            {
                if(userResult.Result == null)
                {
                    return BadRequest();
                }
                if (await UserService.UpdateAsync(id, value) is User user && user != null)
                {
                    return Ok(user);
                }
                return StatusCode(500);
            }
            return BadRequest(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Delete(int id)
        {
            if(UserService.FetchByIdAsync(id) is Task<User> user && user != null)
            {
                if (await UserService.DeleteAsync(id))
                {
                    return Ok(user.Result);
                }
            }
            return NotFound();
        }
    }
}