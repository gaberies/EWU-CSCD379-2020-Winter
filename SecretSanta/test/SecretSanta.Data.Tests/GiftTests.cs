using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Data;
using SecretSanta.Data.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        [TestMethod]
        public async Task CreateAuthor_ShouldSaveIntoDatabase()
        {
            int giftId = -1;
            // Arrange
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var Gift = new Gift
                {
                    Title = "Inigo",
                    Description = "Montoya",
                    Url = "inigo@montoya.me"

                };
                applicationDbContext.Gifts.Add(Gift);

                var Gift2 = new Gift
                {
                    Title = "Inigo",
                    Description = "Montoya",
                    Url = "inigo@montoya.me"

                };
                applicationDbContext.Gifts.Add(Gift2);

                await applicationDbContext.SaveChangesAsync();

                giftId = Gift.Id;
            }

            // act
            // assert
            using (var applicationdbcontext = new applicationDbContext(Options))
            {
                var author = await applicationDbContext.Authors.Where(a => a.Id == authorId).SingleOrDefaultAsync();

                Assert.IsNotNull(author);
                Assert.AreEqual("Inigo", author.FirstName);
            }
        }

        [TestMethod]
        public async Task CreateAuthor_ShouldSetFingerPrintDataOnInitialSave()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            int authorId = -1;
            // Arrange
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var author = new Author
                {
                    FirstName = "Inigo",
                    LastName = "Montoya",
                    Email = "inigo@montoya.me"
                };
                applicationDbContext.Authors.Add(author);

                var author2 = new Author
                {
                    FirstName = "Inigo",
                    LastName = "Montoya",
                    Email = "inigo@montoya.me"
                };
                applicationDbContext.Authors.Add(author2);

                await applicationDbContext.SaveChangesAsync();

                authorId = author.Id;
            }

            // Act
            // Assert
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var author = await applicationDbContext.Authors.Where(a => a.Id == authorId).SingleOrDefaultAsync();

                Assert.IsNotNull(author);
                Assert.AreEqual("imontoya", author.CreatedBy);
                Assert.AreEqual("imontoya", author.ModifiedBy);
            }
        }

        [TestMethod]
        public async Task CreateAuthor_ShouldSetFingerPrintDataOnUpdate()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            int authorId = -1;
            // Arrange
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var author = new Author
                {
                    FirstName = "Inigo",
                    LastName = "Montoya",
                    Email = "inigo@montoya.me"
                };
                applicationDbContext.Authors.Add(author);

                var author2 = new Author
                {
                    FirstName = "Inigo",
                    LastName = "Montoya",
                    Email = "inigo@montoya.me"
                };
                applicationDbContext.Authors.Add(author2);

                await applicationDbContext.SaveChangesAsync();

                authorId = author.Id;
            }

            // Act
            // change the user that is updating the record
            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "pbuttercup"));
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                // Since we are pulling back the record from the database and making changes to it, we don't need to re-add it to the collection
                // thus no Authors.Add call, that is only needed when new records are inserted
                var author = await applicationDbContext.Authors.Where(a => a.Id == authorId).SingleOrDefaultAsync();
                author.FirstName = "Princess";
                author.LastName = "Buttercup";

                await applicationDbContext.SaveChangesAsync();
            }
            // Assert
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var author = await applicationDbContext.Authors.Where(a => a.Id == authorId).SingleOrDefaultAsync();

                Assert.IsNotNull(author);
                Assert.AreEqual("imontoya", author.CreatedBy);
                Assert.AreEqual("pbuttercup", author.ModifiedBy);
            }
        }
    }
}