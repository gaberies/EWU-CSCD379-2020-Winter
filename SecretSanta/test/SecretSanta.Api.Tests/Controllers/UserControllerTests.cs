using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : BaseApiControllerTests<Business.Dto.User, Business.Dto.UserInput, UserInMemoryService>
    {
        protected override BaseApiController<Business.Dto.User, Business.Dto.UserInput> CreateController(UserInMemoryService service)
            => new UserController(service);

        protected override Business.Dto.User CreateEntity()
        {
            return new Business.Dto.User
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };
        }
    }

    public class UserInMemoryService : InMemoryEntityService<Business.Dto.User, Business.Dto.UserInput>, IUserService
    {

    }
}
