using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;
using System.Threading.Tasks;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftServiceTests : TestBase
    {
        [TestMethod]
        public async Task CreateGift_ShouldSaveIntoDatabase()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(Options);

            IGiftService service = new GiftService(dbContext, Mapper);

            Gift gift = SampleData.CreateLifeGift();
            User user = gift.User;

            await service.InsertAsync(gift);

            // Act

            // Assert
            Assert.IsNotNull(gift.Id);
            Assert.IsNotNull(user.Id);
            Assert.AreSame(gift.User, user);
            Assert.AreEqual(user.Id, gift.User.Id);
        }

        [TestMethod]
        public async Task FetchByIdGift_ShouldIncludeAuthor()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(Options);

            IGiftService service = new GiftService(dbContext, Mapper);

            Gift gift = SampleData.CreateLifeGift();
            User user = gift.User;

            await service.InsertAsync(gift);

            // Act

            // Assert
            using var dbContext2 = new ApplicationDbContext(Options);
            service = new GiftService(dbContext, Mapper);
            gift = await service.FetchByIdAsync(gift.Id!);

            Assert.IsNotNull(gift.User);
        }

        [TestMethod]
        public async Task InsertGift_InigoAndPrincess_Success()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContextInsert, Mapper);

            Gift gift = SampleData.CreateLifeGift();

            // Act
            await service.InsertAsync(gift);

            // Assert
            Assert.IsNotNull(gift.Id);
        }

        [TestMethod]
        public async Task UpdateGift_ShouldSaveIntoDatabase()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContextInsert, Mapper);

            Gift giftLife = SampleData.CreateLifeGift();
            Gift giftLove = SampleData.CreateLoveGift();

            await service.InsertAsync(giftLife);
            await service.InsertAsync(giftLove);

            // Act
            using var dbContextFetch = new ApplicationDbContext(Options);
            Gift giftOfLife = await dbContextFetch.Gifts.SingleAsync(item => item.Id == giftLife.Id);

            const string deathGift = "Gift of Death";
            giftOfLife.Title = deathGift;

            await service.UpdateAsync(giftLove.Id, giftOfLife);

            // Assert
            using var dbContextAssert = new ApplicationDbContext(Options);
            giftOfLife = await dbContextAssert.Gifts.SingleAsync(item => item.Id == giftLife.Id);
            var giftOfLove = await dbContextAssert.Gifts.SingleAsync(item => item.Id == 2);

            Assert.AreEqual<string>(giftOfLife.Title, giftLife.Title);
            Assert.AreEqual<string>(giftOfLife.Description, giftLife.Description);
            Assert.AreEqual<string>(giftOfLife.Url, giftLife.Url);

            Assert.AreEqual<string>(giftOfLove.Title, deathGift);
            Assert.AreEqual<string>(giftOfLove.Description, giftLove.Description);
            Assert.AreEqual<string>(giftOfLove.Url, giftLove.Url);
        }
    }
}
