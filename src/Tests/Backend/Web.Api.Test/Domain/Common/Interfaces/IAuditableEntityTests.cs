using Domain.Common.Interfaces;

namespace Web.Api.Test.Domain.Common.Interfaces
{
    public class IAuditableEntityTests
    {
        [Test]
        public void IAuditableEntity_ImplementedCorrectly()
        {
            // Arrange

            // Act
            var dummyEntity = new IAuditableTestEntity();

            // Assert
            Assert.IsTrue(dummyEntity is IAuditableEntity);
        }

        class IAuditableTestEntity : IAuditableEntity
        {
            public string Id { get; set; }
            public string? CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string? UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public int TenantId { get; set; }
            public Guid Guid { get; set; }
            int IEntity.Id { get; set; }
        }
    }
}
