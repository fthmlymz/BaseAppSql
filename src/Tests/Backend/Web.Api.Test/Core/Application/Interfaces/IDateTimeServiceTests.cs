using Application.Interfaces;
using Infrastructure.Services;

namespace Web.Api.Test.Core.Application.Interfaces
{
    [TestFixture]
    public class IDateTimeServiceTests
    {
        private IDateTimeService _dateTimeService;

        [SetUp]
        public void Setup()
        {
            _dateTimeService = new DateTimeService(); // DateTimeService sınıfınızı burada örnekleyin
        }


        [Test]
        public void NowUtc_ShouldReturnCurrentUtcDateTime()
        {
            // Arrange
            var expectedDateTime = DateTime.UtcNow;

            // Act
            var actualDateTime = _dateTimeService.NowUtc;

            // Assert
            Assert.AreEqual(expectedDateTime.Date, actualDateTime.Date);
            Assert.AreEqual(expectedDateTime.Hour, actualDateTime.Hour);
            Assert.AreEqual(expectedDateTime.Minute, actualDateTime.Minute);
            Assert.AreEqual(expectedDateTime.Second, actualDateTime.Second);
            Assert.That(expectedDateTime.Millisecond, Is.EqualTo(actualDateTime.Millisecond).Within(1));
        }
    }
}
