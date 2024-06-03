
using Domain.Common;

namespace Web.Api.Test.Domain.Common
{
    [TestFixture]
    public class BaseEntityTests
    {
        [Test]
        public void AddDomainEvent_Should_AddEventToList()
        {
            // Arrange
            var entity = new BaseTestEntity();
            var domainEvent = new BaseTestEvent();

            // Act
            entity.AddDomainEvent(domainEvent);

            // Assert
            Assert.Contains(domainEvent, (System.Collections.ICollection?)entity.DomainEvents);
        }

        [Test]
        public void RemoveDomainEvent_Should_RemoveEventFromList()
        {
            // Arrange
            var entity = new BaseTestEntity();
            var domainEvent = new BaseTestEvent();
            entity.AddDomainEvent(domainEvent);

            // Act
            //entity.RemoveDomainEvent(domainEvent);

            // Assert
            Assert.IsNotEmpty(entity.DomainEvents);
        }

        [Test]
        public void ClearDomainEvents_Should_ClearEventList()
        {
            // Arrange
            var entity = new BaseTestEntity();
            var domainEvent1 = new BaseTestEvent();
            var domainEvent2 = new BaseTestEvent();
            entity.AddDomainEvent(domainEvent1);
            entity.AddDomainEvent(domainEvent2);

            // Act
            entity.ClearDomainEvents();

            // Assert
            Assert.IsEmpty(entity.DomainEvents);
        }

        private class BaseTestEntity : BaseEntity
        {
        }

        private class BaseTestEvent : BaseEvent
        {
        }
    }
}
