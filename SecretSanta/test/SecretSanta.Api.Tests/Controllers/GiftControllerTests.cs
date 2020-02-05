using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Data;
using SecretSanta.Data.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllerTests
    {
        [TestMethod]
        public void Create_GiftController_Success()
        {
            //Arrange
            var service = new MockGiftService();

            //Act
            _ = new GiftController(service);

            //Assert
            Assert.IsTrue(service != null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Fails()
        {
            //Arrange

            //Act
            _ = new GiftController(null!);

            //Assert
        }

        [TestMethod]
        public async Task GetById_WithExistingGift_Success()
        {
            // Arrange
            var service = new MockGiftService();
            Gift gift = SampleData.CreateLifeGift();
            gift = await service.InsertAsync(gift);

            var controller = new GiftController(service);

            // Act
            ActionResult<Gift> rv = await controller.Get(gift.Id);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetById_BadId_NotFound()
        {
            // Arrange
            var service = new MockGiftService();

            var controller = new GiftController(service);

            // Act
            ActionResult<Gift> rv = await controller.Get(42);

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task GetAll_WithExistingGifts_Success()
        {
            // Arrange
            var service = new MockGiftService();
            Gift gift = SampleData.CreateLifeGift();
            Gift gift2 = SampleData.CreateLoveGift();
            await service.InsertAsync(gift);
            await service.InsertAsync(gift2);

            var controller = new GiftController(service);

            // Act
            ActionResult<List<Gift>> rv = await controller.Get();

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetAll_NoGifts_NotFound()
        {
            // Arrange
            var service = new MockGiftService();

            var controller = new GiftController(service);

            // Act
            ActionResult<List<Gift>> rv = await controller.Get();

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task Post_NewGift_Success()
        {
            // Arrange
            var service = new MockGiftService();
            Gift gift = SampleData.CreateLoveGift();

            var controller = new GiftController(service);

            // Act
            ActionResult<Gift> rv = await controller.Post(gift);

            // AssertRt
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Post_NullGift_BadRequestObject()
        {
            // Arrange
            var service = new MockGiftService();

            var controller = new GiftController(service);

            // Act
            ActionResult<Gift> rv = await controller.Post(null!);

            // Assert
            Assert.IsTrue(rv.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task Put_UpdateGift_Success()
        {
            // Arrange
            var service = new MockGiftService();
            Gift gift = SampleData.CreateLoveGift();
            Gift changeGift = SampleData.CreateLifeGift();
            gift = await service.InsertAsync(gift);

            var controller = new GiftController(service);

            // Act
            ActionResult<Gift> rv = await controller.Put(gift.Id, changeGift);

            // AssertRt
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Put_BadId_BadRequest()
        {
            // Arrange
            var service = new MockGiftService();
            Gift gift = SampleData.CreateLifeGift();
            Gift changeGift = SampleData.CreateLoveGift();
            await service.InsertAsync(gift);

            var controller = new GiftController(service);

            // Act
            ActionResult<Gift> rv = await controller.Put(42, changeGift);

            // Assert
            Assert.IsTrue(rv.Result is BadRequestResult);
        }

        [TestMethod]
        public async Task Put_NullGift_BadRequest()
        {
            // Arrange
            var service = new MockGiftService();
            Gift gift = SampleData.CreateLifeGift();
            gift = await service.InsertAsync(gift);

            var controller = new GiftController(service);

            // Act
            ActionResult<Gift> rv = await controller.Put(gift.Id, null!);

            // Assert
            Assert.IsTrue(rv.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task Delete_DeleteGift_Success()
        {
            // Arrange
            var service = new MockGiftService();
            Gift gift = SampleData.CreateLifeGift();
            gift = await service.InsertAsync(gift);

            var controller = new GiftController(service);

            // Act
            ActionResult<Gift> rv = await controller.Delete(gift.Id);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Delete_BadId_NotFound()
        {
            // Arrange
            var service = new MockGiftService();
            Gift gift = SampleData.CreateLifeGift();
            await service.InsertAsync(gift);

            var controller = new GiftController(service);

            // Act
            ActionResult<Gift> rv = await controller.Delete(42);

            // Assert
            Assert.IsTrue(rv.Result is NotFoundResult);
        }

    }

    internal class MockGiftService : IGiftService
    {
        private Dictionary<int, Gift> MockItems { get; } = new Dictionary<int, Gift>();

        public Task<bool> DeleteAsync(int id)
        {
            if (MockItems.TryGetValue(id, out Gift? user) && user != null)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<List<Gift>> FetchAllAsync()
        {
            List<Gift> giftList = new List<Gift>();
            if (MockItems.Count != 0)
            {
                foreach (Gift gift in MockItems.Values)
                {
                    giftList.Add(gift);
                }
                return Task.FromResult(giftList);
            }
            return Task.FromResult<List<Gift>>(null!);
        }

        public Task<Gift> FetchByIdAsync(int id)
        {
            if (MockItems.TryGetValue(id, out Gift? gift))
            {
                return gift != null ? Task.FromResult(gift) : Task.FromResult<Gift>(null!);
            }
            return Task.FromResult<Gift>(null!);
        }

        public Task<Gift> InsertAsync(Gift entity)
        {
            if (entity != null)
            {
                int id = MockItems.Count + 1;
                MockItems[id] = new TestGift(entity, id);
                return Task.FromResult(MockItems[id]);
            }
            return Task.FromResult(entity!);
        }

        public Task<Gift?> UpdateAsync(int id, Gift entity)
        {
            MockItems[id] = entity;
            return Task.FromResult<Gift?>(MockItems[id]);
        }

        private class TestGift : Gift
        {
            public TestGift(Gift gift, int id)
                : base(
                      (gift ?? throw new ArgumentNullException(nameof(gift)))
                      .Title ?? throw new ArgumentNullException(nameof(gift)),
                      gift.Url ?? throw new ArgumentNullException(nameof(gift)),
                      gift.Description ?? throw new ArgumentNullException(nameof(gift)),
                      SampleData.CreateInigoMontoya()
                      )
            {
                Id = id;
            }
        }
    }
}