using Domain.Common;

namespace Web.Api.Test.Domain.Common
{
    [TestFixture]
    public class BaseEventTests
    {
        [Test]
        public void BaseEvent_DateOccurred_ShouldBeSetToUtcNow()
        {
            // Arrange
            var baseEvent = new MockBaseEvent();

            // Act
            var dateOccurred = baseEvent.DateOccurred;

            // Assert
            Assert.AreEqual(DateTime.UtcNow.Date, dateOccurred.Date);
            Assert.AreEqual(DateTimeKind.Utc, dateOccurred.Kind);
        }
    }

    public class MockBaseEvent : BaseEvent { }
}
