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
    public class GroupControleerTests
    {
        [TestMethod]
        public void Create_GroupController_Success()
        {
            //Arrange
            var service = new MockGroupService();

            //Act
            _ = new GroupController(service);

            //Assert
            Assert.IsTrue(service != null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Fails()
        {
            //Arrange

            //Act
            _ = new GroupController(null!);

            //Assert
        }

        [TestMethod]
        public async Task GetById_WithExistingGroup_Success()
        {
            // Arrange
            var service = new MockGroupService();
            Group group = SampleData.CreateLoveTeam();
            group = await service.InsertAsync(group);

            var controller = new GroupController(service);

            // Act
            ActionResult<Group> rv = await controller.Get(group.Id);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetById_BadId_NotFound()
        {
            // Arrange
            var service = new MockGroupService();

            var controller = new GroupController(service);

            // Act
            ActionResult<Group> rv = await controller.Get(42);

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task GetAll_WithExistingGroups_Success()
        {
            // Arrange
            var service = new MockGroupService();
            Group group = SampleData.CreateLifeTeam();
            Group group2 = SampleData.CreateLoveTeam();
            await service.InsertAsync(group);
            await service.InsertAsync(group2);

            var controller = new GroupController(service);

            // Act
            ActionResult<List<Group>> rv = await controller.Get();

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetAll_WithNoGroups_NotFound()
        {
            // Arrange
            var service = new MockGroupService();

            var controller = new GroupController(service);

            // Act
            ActionResult<List<Group>> rv = await controller.Get();

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task Post_NewGroup_Success()
        {
            // Arrange
            var service = new MockGroupService();
            Group group = SampleData.CreateLoveTeam();

            var controller = new GroupController(service);

            // Act
            ActionResult<Group> rv = await controller.Post(group);

            // AssertRt
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Post_NullGroup_BadRequestObject()
        {
            // Arrange
            var service = new MockGroupService();

            var controller = new GroupController(service);

            // Act
            ActionResult<Group> rv = await controller.Post(null!);

            // Assert
            Assert.IsTrue(rv.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task Put_ChangeGroup_Success()
        {
            // Arrange
            var service = new MockGroupService();
            Group group = SampleData.CreateLifeTeam();
            Group changeGroup = SampleData.CreateLoveTeam();
            group = await service.InsertAsync(group);

            var controller = new GroupController(service);

            // Act
            ActionResult<Group> rv = await controller.Put(group.Id, changeGroup);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Put_BadId_BadRequest()
        {
            // Arrange
            var service = new MockGroupService();
            Group group = SampleData.CreateLifeTeam();
            Group changeGroup = SampleData.CreateLifeTeam();
            await service.InsertAsync(group);

            var controller = new GroupController(service);

            // Act
            ActionResult<Group> rv = await controller.Put(42, changeGroup);

            // Assert
            Assert.IsTrue(rv.Result is BadRequestResult);
        }

        [TestMethod]
        public async Task Put_NullGroup_BadRequest()
        {
            // Arrange
            var service = new MockGroupService();
            Group group = SampleData.CreateLifeTeam();
            group = await service.InsertAsync(group);

            var controller = new GroupController(service);

            // Act
            ActionResult<Group> rv = await controller.Put(group.Id, null!);

            // Assert
            Assert.IsTrue(rv.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task Delete_DeleteGroup_Success()
        {
            // Arrange
            var service = new MockGroupService();
            Group group = SampleData.CreateLifeTeam();
            group = await service.InsertAsync(group);

            var controller = new GroupController(service);

            // Act
            ActionResult<Group> rv = await controller.Delete(group.Id);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Delete_BadId_NotFound()
        {
            // Arrange
            var service = new MockGroupService();
            Group group = SampleData.CreateLifeTeam();
            await service.InsertAsync(group);

            var controller = new GroupController(service);

            // Act
            ActionResult<Group> rv = await controller.Delete(42);

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

    }

    internal class MockGroupService : IGroupService
    {
        private Dictionary<int, Group> MockItems { get; } = new Dictionary<int, Group>();

        public Task<bool> DeleteAsync(int id)
        {
            if (MockItems.TryGetValue(id, out Group? group) && group != null)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<List<Group>> FetchAllAsync()
        {
            List<Group> groupList = new List<Group>();
            if (MockItems.Count != 0)
            {
                foreach (Group group in MockItems.Values)
                {
                    groupList.Add(group);
                }
                return Task.FromResult(groupList);
            }
            return Task.FromResult<List<Group>>(null!);
        }

        public Task<Group> FetchByIdAsync(int id)
        {
            if (MockItems.TryGetValue(id, out Group? group))
            {
                return group != null ? Task.FromResult(group) : Task.FromResult<Group>(null!);
            }
            return Task.FromResult<Group>(null!);
        }

        public Task<Group> InsertAsync(Group entity)
        {
            if (entity != null)
            {
                int id = MockItems.Count + 1;
                MockItems[id] = new TestGroup(entity, id);
                return Task.FromResult(MockItems[id]);
            }
            return Task.FromResult(entity!);
        }

        public Task<Group?> UpdateAsync(int id, Group entity)
        {
            MockItems[id] = entity;
            return Task.FromResult<Group?>(MockItems[id]);
        }

        private class TestGroup : Group
        {
            public TestGroup(Group group, int id)
                : base((group ?? throw new ArgumentNullException(nameof(group)))
                      .Title ?? throw new ArgumentNullException(nameof(group)))
            {
                Id = id;
            }
        }
    }
}