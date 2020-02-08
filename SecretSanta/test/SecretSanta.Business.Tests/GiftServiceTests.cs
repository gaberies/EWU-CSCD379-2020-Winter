using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Business.Services;
using SecretSanta.Business.Dto;
using SecretSanta.Data;
using System;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftServiceTests : EntityServiceTests<Dto.Gift, Dto.GiftInput, Data.Gift>
    {
        [TestInitialize]
        public void TestSetup()
        {
            //Gifts need related users, so create one
            using var dbContext = new ApplicationDbContext(Options);
            dbContext.Users.Add(new Data.User("first", "last"));
            dbContext.SaveChanges();
        }

        protected override Data.Gift CreateEntity()
            => new Data.Gift(Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                new Data.User(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));

        protected override GiftInput CreateInputDto()
        {
            // We need to figure out why this is not getting mapped over. I thnk it has
            // to do with the mapper
            Dto.User user = new Dto.User
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };
            return new GiftInput
            {
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString(),
                User = user,
                UserId = 1 //From the user we inserted in the initialize
            };
        }

        protected override IEntityService<Dto.Gift, Dto.GiftInput> GetService(ApplicationDbContext dbContext, IMapper mapper)
            => new GiftService(dbContext, mapper);
    }
}