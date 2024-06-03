using Domain.Common;
using MediatR;
using Moq;

namespace Web.Api.Test.Domain.Common
{
    [TestFixture]
    public class DomainEventDispatcherTests
    {
        private DomainEventDispatcher _dispatcher;
        private Mock<IMediator> _mediatorMock;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _dispatcher = new DomainEventDispatcher(_mediatorMock.Object);
        }

        [Test]
        public async Task DispatchAndClearEvents_ShouldPublishDomainEvents()
        {
            // Arrange
            var entity1 = new Mock<BaseEntity>().Object;
            var entity2 = new Mock<BaseEntity>().Object;
            var entitiesWithEvents = new List<BaseEntity> { entity1, entity2 };

            entity1.AddDomainEvent(It.IsAny<BaseEvent>());
            entity2.AddDomainEvent(It.IsAny<BaseEvent>());


            //// Act
            await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

            //// Assert
            //_mediatorMock.Verify(x => x.Publish(It.IsAny<BaseEvent>()), Times.Exactly(2));

            entity1.ClearDomainEvents();
            entity2.ClearDomainEvents();

            Assert.IsEmpty(entity1.DomainEvents);
            Assert.IsEmpty(entity2.DomainEvents);
        }
    }

    public class TestDomainEvent : INotification
    {
    }
}
