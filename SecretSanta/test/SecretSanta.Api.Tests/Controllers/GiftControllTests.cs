using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllTests : BaseApiControllerTests<Business.Dto.Gift, Business.Dto.GiftInput, GiftInMemoryService>
    {
        protected override BaseApiController<Business.Dto.Gift, Business.Dto.GiftInput> CreateController(GiftInMemoryService service)
            => new GiftController(service);

        protected override Business.Dto.Gift CreateEntity()
        {
            Business.Dto.User user = new Business.Dto.User
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };
            return new Business.Dto.Gift
            {
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString(),
                User = user
            };
        }
    }

    public class GiftInMemoryService : InMemoryEntityService<Business.Dto.Gift, Business.Dto.GiftInput>, IGiftService
    {

    }
}
