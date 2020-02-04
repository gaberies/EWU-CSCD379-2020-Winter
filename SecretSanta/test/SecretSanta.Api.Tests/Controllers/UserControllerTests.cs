using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.Data.Tests;
using SecretSanta.Data;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void Create_UserController_Success()
        {
            //Arrange
            var service = new UserService();

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
            var service = new UserService();
            User user = SampleData.CreateInigoMontoya();
            user = await service.InsertAsync(user);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Get(user.Id);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetById_WithExistingUser_NotFound()
        {
            // Arrange
            var service = new UserService();
            User user = SampleData.CreateInigoMontoya();
            user = await service.InsertAsync(user);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Get(user.Id + 52);

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task FetchAll_WithExistingUsers_Success()
        {
            // Arrange
            var service = new UserService();
            User user = SampleData.CreateInigoMontoya();
            User user2 = SampleData.CreatePrincessButtercup();
            await service.InsertAsync(user);
            await service.InsertAsync(user2);

            var controller = new UserController(service);

            // Act
            List<User> returnedValue = (List<User>)await controller.Get();

            // Assert
            Assert.AreEqual(2, returnedValue.Count);
        }

        [TestMethod]
        public async Task FetchAll_WithExistingUsers_NotFound()
        {
            // Arrange
            var service = new UserService();
            User user = SampleData.CreateInigoMontoya();
            user = await service.InsertAsync(user);

            var controller = new UserController(service);

            // Act
            ActionResult<User> rv = await controller.Get(user.Id + 52);

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

    }

    internal class UserService : IUserService
    {
        private Dictionary<int, User> MockItems { get; } = new Dictionary<int, User>();

        public Task<bool> DeleteAsync(int id)
        {
            if (MockItems.TryGetValue(id, out _))
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<List<User>> FetchAllAsync()
        { 
            if(MockItems.Count != 0)
            {
                List<User> userList = new List<User>();
                foreach(User user in MockItems.Values)
                {
                    userList.Add(user);
                }
                Task<List<User>> t1 = Task.FromResult(userList);
                return t1;
            }
            return Task.FromResult<List<User>>(null);
        }

        public Task<User?> FetchByIdAsync(int id)
        {
            if (MockItems.TryGetValue(id, out User? user))
            {
                Task<User?> t1 = Task.FromResult<User?>(user);
                return t1;
            }
            Task<User?> t2 = Task.FromResult<User?>(null);
            return t2;
        }

        public Task<User> InsertAsync(User entity)
        {
            int id = MockItems.Count + 1;
            MockItems[id] = new TestUser(entity, id);
            return Task.FromResult(MockItems[id]);
        }

        public Task<User[]> InsertAsync(params User[] entity)
        {
            throw new NotImplementedException();
        }

        public Task<User?> UpdateAsync(int id, User entity)
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