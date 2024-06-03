using Infrastructure.Services;

namespace Web.Api.Test.Infrastructure.Services
{
    [TestFixture]
    public class DateTimeServiceTests
    {
        [Test]
        public void NowUtc_Should_Return_Current_Utc_DateTime()
        {
            // Arrange
            var dateTimeService = new DateTimeService();

            // Act
            var nowUtc = dateTimeService.NowUtc;

            // Assert
            Assert.That(nowUtc, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMilliseconds(1)));
        }
    }
}
