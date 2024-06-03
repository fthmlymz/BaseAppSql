using Domain.Common.Interfaces;

namespace Web.Api.Test.Domain.Common.Interfaces
{
    [TestFixture]
    public class IEntityTests
    {
        [Test]
        public void IEntity_Properties_ShouldBeSettableAndGettable()
        {
            // Arrange
            IEntity entity = new MockIEntity();

            // Act
            entity.Id = 1;
            entity.TenantId = 2;
            entity.Guid = Guid.NewGuid();

            // Assert
            Assert.AreEqual(1, entity.Id);
            Assert.AreEqual(2, entity.TenantId);
            Assert.IsNotNull(entity.Guid);
        }

        private class MockIEntity: IEntity
        {
            public int Id { get; set; }
            public int TenantId { get; set; }
            public Guid Guid { get; set; }
        }
    }
}
