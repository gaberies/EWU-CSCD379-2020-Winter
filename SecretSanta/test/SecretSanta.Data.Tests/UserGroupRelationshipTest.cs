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

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class UserGroupRelationshipTestTests : TestBase
    {
        [TestMethod]
        public async Task CreateUserWithManyGroups()
        {
            // Arrange
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            var user = new User
            {
                FirstName = "a",
                LastName = "b",
            };
            var group1 = new Group
            {
                Name = "C#",
            };
            var group2 = new Group
            {
                Name = "Lecture",
            };

            // Act
            user.Relationship = new List<UserGroupRelationship>();
            user.Relationship.Add(new UserGroupRelationship { User = user, Group = group1 });
            user.Relationship.Add(new UserGroupRelationship { User = user, Group = group2 });

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            // Assert
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var retrievedUser = await dbContext.Users.Where(p => p.Id == user.Id)
                    .Include(p => p.Relationship).ThenInclude(pt => pt.Group).SingleOrDefaultAsync();


                Assert.IsNotNull(retrievedUser);
                Assert.AreEqual(2, retrievedUser.Relationship.Count);
                Assert.IsNotNull(retrievedUser.Relationship[0].Group);
                Assert.IsNotNull(retrievedUser.Relationship[1].Group);
            }
        }
    }
}