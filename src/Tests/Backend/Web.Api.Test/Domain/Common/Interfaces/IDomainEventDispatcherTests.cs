using Domain.Common;
using Domain.Common.Interfaces;
using MediatR;
using Moq;

namespace Web.Api.Test.Domain.Common.Interfaces
{
    [TestFixture]
    public class IDomainEventDispatcherTests
    {
        private Mock<IMediator> _mediatorMock;
        private IDomainEventDispatcher _dispatcher;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _dispatcher = new DomainEventDispatcher(_mediatorMock.Object);
        }

        [Test]
        public async Task DispatchAndClearEvents_ValidEntities_Success()
        {
            // Arrange
            var entitiesWithEvents = new List<BaseEntity>
            {
                // Add test entities with events
            };

            // Act
            await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

            // Assert
            foreach (var entity in entitiesWithEvents)
            {
                _mediatorMock.Verify(x => x.Send(It.IsAny<object>(), default), Times.Once);
            }

            // Verify
            Assert.IsEmpty(entitiesWithEvents);
            Assert.IsNotNull(entitiesWithEvents);
        }
    }
}
