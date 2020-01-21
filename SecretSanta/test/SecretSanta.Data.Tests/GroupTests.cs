using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace SecretSanta.Data.Tests
    
{
    [TestClass]
    public class GroupTests : TestBase
        
    {
        [TestMethod]
        public async Task CreateGroup_ShouldSaveIntoDatabase()
        {
            int groupId = -1;
            // Arrange
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var group = new Group
                {
                    Name = "Group"
                };
                applicationDbContext.Groups.Add(group);

                await applicationDbContext.SaveChangesAsync();

                groupId = group.Id;
            };

            // act
            // assert
            using (var applicationdbcontext = new ApplicationDbContext(Options))
            {
                var group = await applicationdbcontext.Groups.Where(a => a.Id == groupId).SingleOrDefaultAsync();

                Assert.IsNotNull(group);
                Assert.AreEqual<string>("Group", group.Name);
            }
        }


        [TestMethod]
        public async Task CreateGroup_ShouldSetFingerPrintDataOnInitialSave()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            int groupId = -1;
            // Arrange
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var group = new Group
                {
                    Name = "This Group Is Awesome"
                };
                applicationDbContext.Groups.Add(group);


                await applicationDbContext.SaveChangesAsync();

                groupId = group.Id;
            }

            // Act
            // Assert
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var group = await applicationDbContext.Groups.Where(a => a.Id == groupId).SingleOrDefaultAsync();

                Assert.IsNotNull(group);
                Assert.AreEqual("imontoya", group.CreatedBy);
                Assert.AreEqual("imontoya", group.ModifiedBy);
            }
        }

        [TestMethod]
        public async Task CreateGroup_ShouldSetFingerPrintDataOnUpdate()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            int groupId = -1;
            // Arrange
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var group = new Group
                {
                    Name = "This Group Is Awesome"
                };
                applicationDbContext.Groups.Add(group);

                await applicationDbContext.SaveChangesAsync();

                groupId = group.Id;
            }

            // Act
            // change the user that is updating the record
            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "pbuttercup"));
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                // Since we are pulling back the record from the database and making changes to it, we don't need to re-add it to the collection
                // thus no Authors.Add call, that is only needed when new records are inserted
                var group = await applicationDbContext.Groups.Where(a => a.Id == groupId).SingleOrDefaultAsync();
                group.Name = "Princess";

                await applicationDbContext.SaveChangesAsync();
            }
            // Assert
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var group = await applicationDbContext.Groups.Where(a => a.Id == groupId).SingleOrDefaultAsync();

                Assert.IsNotNull(group);
                Assert.AreEqual("imontoya", group.CreatedBy);
                Assert.AreEqual("pbuttercup", group.ModifiedBy);
            }
        }
    }
}
