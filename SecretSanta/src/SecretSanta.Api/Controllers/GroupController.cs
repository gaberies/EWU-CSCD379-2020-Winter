using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : BaseApiController<Business.Dto.Group, Business.Dto.GroupInput>
    {
        public GroupController(IGroupService groupService) 
            : base(groupService)
        { }
    }
}
