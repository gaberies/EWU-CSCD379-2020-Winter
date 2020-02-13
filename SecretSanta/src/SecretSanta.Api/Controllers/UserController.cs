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
    public class UserController : BaseApiController<Business.Dto.User, Business.Dto.UserInput>
    {
        public IHttpClientFactory ClientFactory { get; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public UserController(IUserService userService)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(userService) { }

        public UserController(IUserService userService, IHttpClientFactory clientFactory)
            : base(userService)
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
            //UserClient client = new UserClient(httpClient);

            //ICollection<User> users = await client.GetAllAsync();
            return null!;  //View(users);
        }
    }
}
