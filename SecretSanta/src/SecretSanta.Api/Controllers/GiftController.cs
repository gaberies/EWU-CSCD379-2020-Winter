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
    public class GiftController : ControllerBase
    {
        private IGiftService GiftService { get; }

        public GiftController(IGiftService giftService)
        {
            GiftService = giftService ?? throw new System.ArgumentNullException(nameof(giftService));
        }

        // GET: https://localhost/api/User
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Gift>>> Get()
        {
            if (await GiftService.FetchAllAsync() is List<Gift> gifts && gifts != null && gifts.Count != 0)
            {
                return Ok(gifts);
            }
            return NotFound();
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Gift>> Get(int id)
        {
            if (await GiftService.FetchByIdAsync(id) is Gift gift && gift != null)
            {
                return Ok(gift);
            }
            return NotFound();
        }

        // POST: api/Author
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Gift>> Post(Gift? value)
        {
            if (value != null)
            {
                await GiftService.InsertAsync(value);
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
        public async Task<ActionResult<Gift>> Put(int id, Gift value)
        {
            if (value != null && GiftService.FetchByIdAsync(id) is Task<Gift> giftResult)
            {
                if (giftResult.Result == null)
                {
                    return BadRequest();
                }
                if (await GiftService.UpdateAsync(id, value) is Gift gift && gift != null)
                {
                    return Ok(gift);
                }
                return StatusCode(500);
            }
            return BadRequest(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Gift>> Delete(int id)
        {
            if (GiftService.FetchByIdAsync(id) is Task<Gift> gift && gift != null)
            {
                if (await GiftService.DeleteAsync(id))
                {
                    return Ok(gift.Result);
                }
            }
            return NotFound();
        }
    }
}