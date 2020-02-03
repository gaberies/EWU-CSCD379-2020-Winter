using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Business.Services;
using SecretSanta.Data;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService GroupService { get; }

        public GroupController(IGroupService userService)
        {
            GroupService = userService ?? throw new System.ArgumentNullException(nameof(userService));
        }

        // GET: https://localhost/api/User
        [HttpGet]
        public async Task<IEnumerable<Group>> Get()
        {
            List<Group> users = await GroupService.FetchAllAsync();
            return users;
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> Get(int id)
        {
            if (await GroupService.FetchByIdAsync(id) is Group group)
            {
                return Ok(group);
            }
            return NotFound();
        }

        // POST: api/Author
        [HttpPost]
        public async Task<Group> Post(Group value)
        {
            return await GroupService.InsertAsync(value);
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Group>> Put(int id, Group value)
        {
            if (await GroupService.UpdateAsync(id, value) is Group group)
            {
                return group;
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> Delete(int id)
        {
            if (await GroupService.DeleteAsync(id) is Boolean result)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}