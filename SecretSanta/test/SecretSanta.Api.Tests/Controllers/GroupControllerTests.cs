using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTests : BaseApiControllerTests<Business.Dto.Group, Business.Dto.GroupInput, GroupInMemoryService>
    {
        protected override BaseApiController<Business.Dto.Group, Business.Dto.GroupInput> CreateController(GroupInMemoryService service)
            => new GroupController(service);

        protected override Business.Dto.Group CreateEntity()
        {
            return new Business.Dto.Group
            {
                Title = Guid.NewGuid().ToString()
            };
        }
    }


    public class GroupInMemoryService : InMemoryEntityService<Business.Dto.Group, Business.Dto.GroupInput>, IGroupService
    {

    }
}
