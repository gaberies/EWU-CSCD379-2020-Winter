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
    public class GroupController : ControllerBase
    {
        private IGroupService GroupService { get; }

        public GroupController(IGroupService groupService)
        {
            GroupService = groupService ?? throw new System.ArgumentNullException(nameof(groupService));
        }

        // GET: https://localhost/api/User
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Group>>> Get()
        {
            if (await GroupService.FetchAllAsync() is List<Group> groups && groups != null && groups.Count != 0)
            {
                return Ok(groups);
            }
            return NotFound();
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> Get(int id)
        {
            if (await GroupService.FetchByIdAsync(id) is Group group && group != null)
            {
                return Ok(group);
            }
            return NotFound();
        }

        // POST: api/Author
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Group>> Post(Group? value)
        {
            if (value != null)
            {
                await GroupService.InsertAsync(value);
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
        public async Task<ActionResult<Group>> Put(int id, Group value)
        {
            if (value != null && GroupService.FetchByIdAsync(id) is Task<Group> groupResult)
            {
                if (groupResult.Result == null)
                {
                    return BadRequest();
                }
                if (await GroupService.UpdateAsync(id, value) is Group group && group != null)
                {
                    return Ok(group);
                }
                return StatusCode(500);
            }
            return BadRequest(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> Delete(int id)
        {
            if (GroupService.FetchByIdAsync(id) is Task<Group> group && group != null)
            {
                if (await GroupService.DeleteAsync(id))
                {
                    return Ok(group.Result);
                }
            }
            return NotFound();
        }
    }
}