using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        [TestMethod]
        public async Task CreateGift_ShouldSaveIntoDatabase()
        {
            int giftId = -1;
            int giftId2 = -1;
            int userId = -1;
            int userId2 = -1;
            // Arrange
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var user = new User
                {
                    FirstName = "Tester",
                    LastName = "Franklin"
                };
                applicationDbContext.Users.Add(user);

                var user2 = new User
                {
                    FirstName = "Tester2",
                    LastName = "Franklin2"
                };
                applicationDbContext.Users.Add(user2);

                var gift = new Gift
                {
                    Title = "Gift",
                    Description = "Des",
                    Url = "inigo@montoya.me",
                    User = user

                };
                applicationDbContext.Gifts.Add(gift);

                var gift2 = new Gift
                {
                    Title = "Gift2",
                    Description = "Dec2",
                    Url = "inigo@montoya.me.and.you",
                    User = user
                };

                applicationDbContext.Gifts.Add(gift2);

                await applicationDbContext.SaveChangesAsync();

                userId = user.Id;
                userId2 = user2.Id;
                giftId = gift.Id;
                giftId2 = gift2.Id;
            };

            // act
            // assert
            using (var applicationdbcontext = new ApplicationDbContext(Options))
            {
                var user = await applicationdbcontext.Users.Where(a => a.Id == userId).SingleOrDefaultAsync();
                var user2 = await applicationdbcontext.Users.Where(a => a.Id == userId2).SingleOrDefaultAsync();
                var gift = await applicationdbcontext.Gifts.Where(a => a.Id == giftId).SingleOrDefaultAsync();
                var gift2 = await applicationdbcontext.Gifts.Where(a => a.Id == giftId2).SingleOrDefaultAsync();

                Assert.IsNotNull(gift);
                Assert.AreEqual<string>("Gift", gift.Title);
                Assert.AreEqual<string>("Des", gift.Description);
                Assert.AreEqual<string>("inigo@montoya.me", gift.Url);
                Assert.AreEqual<string>("Tester", user.FirstName);
                Assert.AreEqual<string>("Franklin", user.LastName);

                Assert.IsNotNull(gift2);
                Assert.AreEqual<string>("Gift2", gift2.Title);
                Assert.AreEqual<string>("Dec2", gift2.Description);
                Assert.AreEqual<string>("inigo@montoya.me.and.you", gift2.Url);
                Assert.AreEqual<string>("Tester2", user2.FirstName);
                Assert.AreEqual<string>("Franklin2", user2.LastName);
            }
        }


        [TestMethod]
        public async Task CreateGift_ShouldSetFingerPrintDataOnInitialSave()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            int giftId = -1;
            // Arrange
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = new Gift
                {
                    Title = "Inigo",
                    Description = "Montoya",
                    Url = "inigo@montoya.me",
                    User = new User()
                };
                applicationDbContext.Gifts.Add(gift);

                var gift2 = new Gift
                {
                    Title = "Inigo",
                    Description = "Montoya",
                    Url = "inigo@montoya.me",
                    User = new User()
                };
                applicationDbContext.Gifts.Add(gift2);

                await applicationDbContext.SaveChangesAsync();

                giftId = gift.Id;
            }

            // Act
            // Assert
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = await applicationDbContext.Gifts.Where(a => a.Id == giftId).SingleOrDefaultAsync();

                Assert.IsNotNull(gift);
                Assert.AreEqual("imontoya", gift.CreatedBy);
                Assert.AreEqual("imontoya", gift.ModifiedBy);
            }
        }

        [TestMethod]
        public async Task CreateGift_ShouldSetFingerPrintDataOnUpdate()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            int giftId = -1;
            // Arrange
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = new Gift
                {
                    Title = "Inigo",
                    Description = "Montoya",
                    Url = "inigo@montoya.me",
                    User = new User()
                };
                applicationDbContext.Gifts.Add(gift);

                var gift2 = new Gift
                {
                    Title = "Inigo",
                    Description = "Montoya",
                    Url = "inigo@montoya.me",
                    User = new User()
                };
                applicationDbContext.Gifts.Add(gift2);

                await applicationDbContext.SaveChangesAsync();

                giftId = gift.Id;
            }

            // Act
            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "pbuttercup"));
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = await applicationDbContext.Gifts.Where(a => a.Id == giftId).SingleOrDefaultAsync();
                gift.Title = "Princess";
                gift.Description = "Buttercup";

                await applicationDbContext.SaveChangesAsync();
            }
            // Assert
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = await applicationDbContext.Gifts.Where(a => a.Id == giftId).SingleOrDefaultAsync();

                Assert.IsNotNull(gift);
                Assert.AreEqual("imontoya", gift.CreatedBy);
                Assert.AreEqual("pbuttercup", gift.ModifiedBy);
            }
        }
    }
}