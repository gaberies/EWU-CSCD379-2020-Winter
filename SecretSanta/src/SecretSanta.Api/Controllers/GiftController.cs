using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : BaseApiController<Business.Dto.Gift, Business.Dto.GiftInput>
    {

        public IHttpClientFactory ClientFactory { get; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public GiftController(IGiftService giftService)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(giftService) { }

        public GiftController(IGiftService giftService, IHttpClientFactory clientFactory)
            : base (giftService)
        {
            if(clientFactory is null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }

            ClientFactory = clientFactory;
        }

        public async Task<ActionResult> IndexAsync()
        {
            HttpClient httpClient = ClientFactory.CreateClient("SantaApi");
            GiftClient client = new GiftClient(httpClient);

            ICollection<Gift> users = await client.GetAllAsync();
            return View(users);
        }
    }
}