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
    public class GroupController : BaseApiController<Business.Dto.Group, Business.Dto.GroupInput>
    {
        public IHttpClientFactory ClientFactory { get; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public GroupController(IGroupService groupService)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(groupService) { }

        public GroupController(IGroupService groupService, IHttpClientFactory clientFactory) 
            : base(groupService)
        {
            if (clientFactory is null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }

            ClientFactory = clientFactory;
        }

        public async Task<ActionResult> IndexAsync()
        {
            HttpClient httpClient = ClientFactory.CreateClient("SantaApi");
            GroupClient client = new GroupClient(httpClient);

            ICollection<Group> users = await client.GetAllAsync();
            return View(users);
        }
    }
}
