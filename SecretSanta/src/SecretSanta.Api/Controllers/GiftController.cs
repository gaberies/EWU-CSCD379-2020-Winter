using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private IGiftService GiftService { get; }

        public GiftController(IGiftService userService)
        {
            GiftService = userService ?? throw new System.ArgumentNullException(nameof(userService));
        }

        // GET: https://localhost/api/User
        [HttpGet]
        public async Task<IEnumerable<Gift>> Get()
        {
            List<Gift> users = await GiftService.FetchAllAsync();
            return users;
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Gift>> Get(int id)
        {
            if (await GiftService.FetchByIdAsync(id) is Gift gift)
            {
                return Ok(gift);
            }
            return NotFound();
        }

        // POST: api/Author
        [HttpPost]
        public async Task<Gift> Post(Gift value)
        {
            return await GiftService.InsertAsync(value);
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Gift>> Put(int id, Gift value)
        {
            if (await GiftService.UpdateAsync(id, value) is Gift gift)
            {
                return gift;
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Gift>> Delete(int id)
        {
            if (await GiftService.DeleteAsync(id) is Boolean result)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}