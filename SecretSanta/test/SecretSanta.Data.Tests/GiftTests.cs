using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {

        [TestMethod]
        public async Task Gift_CanBeSavedToDatabase()
        {
            // Arrange
            var user = new User("Inigo", "Montoya");

            var gift = new Gift(
                   "Ring Doorbell",
                   "The doorbell that saw too much",
                   "www.ring.com",
                    user
                    );

            gift.CreatedBy = "<name>";
            gift.CreatedOn = DateTime.Now;
            gift.ModifiedBy = "<name>";
            gift.ModifiedOn = DateTime.Now;

            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(gift);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            // Act
            // Assert
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.ToListAsync();

                Assert.AreEqual<int>(1, gifts.Count);
                Assert.AreEqual<string>("Ring Doorbell", gifts[0].Title);
                Assert.AreEqual<string>("The doorbell that saw too much", gifts[0].Description);
                Assert.AreEqual<string>("www.ring.com", gifts[0].Url);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(nameof(Gift.Title), null)]
        [DataRow(nameof(Gift.Description), null)]
        [DataRow(nameof(Gift.Url), null)]
        public void Properties_NullTypeArgument_ThrowArgumentException(string propertyName, string value)
        {
            SetPropertyOnGift(propertyName, value);
        }

        private static void SetPropertyOnGift(
    string propertyName, string? value)
        {
            Gift gift = new Gift(
                 "<title>", "<description>", "<url>", new User("<fistName>", "<lastName>"));

            //Retrieve the property information based on the type
            System.Reflection.PropertyInfo propertyInfo
                = gift.GetType().GetProperty(propertyName)!;

            try
            {
                //Set the value of the property
                propertyInfo.SetValue(gift, value, null);
            }
            catch (System.Reflection.TargetInvocationException exception)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException!).Throw();
            }
        }
    }
}
