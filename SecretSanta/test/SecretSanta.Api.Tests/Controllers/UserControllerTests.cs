using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Data;
using SecretSanta.Data.Tests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void Create_UserController_Success()
        {
            //Arrange
            var service = new MockUserService();

            //Act
            _ = new UserController(service);

            //Assert
            Assert.IsTrue(service != null);
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
            var service = new MockUserService();
            User user = SampleData.CreateInigoMontoya();
            user = await service.InsertAsync(user);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Get(user.Id);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetById_BadId_NotFound()
        {
            // Arrange
            var service = new MockUserService();

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Get(42);

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task GetAll_WithExistingUsers_Success()
        {
            // Arrange
            var service = new MockUserService();
            User user = SampleData.CreateInigoMontoya();
            User user2 = SampleData.CreatePrincessButtercup();
            await service.InsertAsync(user);
            await service.InsertAsync(user2);

            var controller = new UserController(service);

            // Act
            ActionResult<List<User>> rv = await controller.Get();

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetAll_WithNoUsers_NotFound()
        {
            // Arrange
            var service = new MockUserService();

            var controller = new UserController(service);

            // Act
            ActionResult<List<User>> rv = await controller.Get();

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task Post_NewUser_Success()
        {
            // Arrange
            var service = new MockUserService();
            User user = SampleData.CreateInigoMontoya();

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Post(user);

            // AssertRt
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Post_NullUser_BadRequestObject()
        {
            // Arrange
            var service = new MockUserService();
            
            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Post(null!);

            // Assert
            Assert.IsTrue(rv.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task Put_UpdateUser_Success()
        {
            // Arrange
            var service = new MockUserService();
            User user = SampleData.CreateInigoMontoya();
            User changeUser = SampleData.CreatePrincessButtercup();
            user = await service.InsertAsync(user);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Put(user.Id, changeUser);

            // AssertRt
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Put_BadId_BadRequest()
        {
            // Arrange
            var service = new MockUserService();
            User user = SampleData.CreateInigoMontoya();
            User changeUser = SampleData.CreatePrincessButtercup();
            await service.InsertAsync(user);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Put(42, changeUser);

            // Assert
            Assert.IsTrue(rv.Result is BadRequestResult);
        }

        [TestMethod]
        public async Task Put_NullUser_BadRequest()
        {
            // Arrange
            var service = new MockUserService();
            User user = SampleData.CreateInigoMontoya();
            user = await service.InsertAsync(user);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Put(user.Id, null!);

            // Assert
            Assert.IsTrue(rv.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task Delete_DeleteUser_Success()
        {
            // Arrange
            var service = new MockUserService();
            User user = SampleData.CreateInigoMontoya();
            user = await service.InsertAsync(user);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Delete(user.Id);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Delete_BadId_NotFound()
        {
            // Arrange
            var service = new MockUserService();
            User user = SampleData.CreateInigoMontoya();
            await service.InsertAsync(user);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Delete(42);

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

    }

    internal class MockUserService : IUserService
    {
        private Dictionary<int, User> MockItems { get; } = new Dictionary<int, User>();

        public Task<bool> DeleteAsync(int id)
        {
            if(MockItems.TryGetValue(id, out User? user) && user != null)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<List<User>> FetchAllAsync()
        {
            List<User> userList = new List<User>();
            if(MockItems.Count != 0)
            {
                foreach (User user in MockItems.Values)
                {
                    userList.Add(user);
                }
                return Task.FromResult(userList);
            }
            return Task.FromResult<List<User>>(null!);
        }

        public Task<User> FetchByIdAsync(int id)
        {
            if (MockItems.TryGetValue(id, out User? user))
            {
                return user != null ?  Task.FromResult(user) : Task.FromResult<User>(null!);
            }
            return Task.FromResult<User>(null!);
        }

        public Task<User> InsertAsync(User entity)
        {
            if (entity != null)
            {
                int id = MockItems.Count + 1;
                MockItems[id] = new TestUser(entity, id);
                return Task.FromResult(MockItems[id]);
            }
            return Task.FromResult(entity!);
        }

        public Task<User?> UpdateAsync(int id, User entity)
        {
            MockItems[id] = entity;
            return Task.FromResult<User?>(MockItems[id]);
        }

        private class TestUser : User
        {
            public TestUser(User user, int id)
                : base((user ?? throw new ArgumentNullException(nameof(user)))
                      .FirstName ?? throw new ArgumentNullException(nameof(user)),
                      user.LastName ?? throw new ArgumentNullException(nameof(user)))
            {
                Id = id;
            }
        }
    }
}