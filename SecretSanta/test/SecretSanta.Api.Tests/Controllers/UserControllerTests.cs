using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    class UserControllerTests
    {
        [TestMethod]
        public void Create_UserController_Success()
        {
            //Arrange
            var service = new UserService();

            //Act
            _ = new UserController(service);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Fails()
        {
            //Arrange

            //Act
            _ = new UserController(null!);

            //Assert
        }

        [TestMethod]
        public async Task GetById_WithExistingUser_Success()
        {
            // Arrange
            var service = new UserService();
            User author = SampleData.CreateInigoMontoya();
            author = await service.InsertAsync(author);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Get(author.Id!.Value);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

    }

    public class AuthorService : IAuthorService
    {
        private Dictionary<int, Author> Items { get; } = new Dictionary<int, Author>();

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Author>> FetchAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Author?> FetchByIdAsync(int id)
        {
            if (Items.TryGetValue(id, out Author? author))
            {
                Task<Author?> t1 = Task.FromResult<Author?>(author);
                return t1;
            }
            Task<Author?> t2 = Task.FromResult<Author?>(null);
            return t2;
        }

        public Task<Author> InsertAsync(Author entity)
        {
            int id = Items.Count + 1;
            Items[id] = new TestAuthor(entity, id);
            return Task.FromResult(Items[id]);
        }

        public Task<Author[]> InsertAsync(params Author[] entity)
        {
            throw new NotImplementedException();
        }

        public Task<Author?> UpdateAsync(int id, Author entity)
        {
            throw new NotImplementedException();
        }

        private class TestUser : User
        {
            public TestUser(User user, int id)
                : base((user ?? throw new ArgumentNullException(nameof(user))).FirstName,
                      user.LastName)
            {
                Id = id;
            }
        }
    }
}
