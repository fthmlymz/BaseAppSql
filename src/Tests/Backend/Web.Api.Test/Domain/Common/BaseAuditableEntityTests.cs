using Domain.Common;

namespace Web.Api.Test.Domain.Common
{
    [TestFixture]
    public class BaseAuditableEntityTests
    {
        [Test]
        public void BaseAuditableEntity_Properties_ShouldBeSet()
        {
            // Arrange
            var entity = new TestAuditableEntity();

            // Act
            entity.CreatedBy = "John";
            entity.CreatedUserId = "123";
            entity.CreatedDate = DateTime.Now;

            entity.UpdatedBy = "Jane";
            entity.UpdatedUserId = "456";
            entity.UpdatedDate = DateTime.Now;

            // Assert
            Assert.AreEqual("John", entity.CreatedBy);
            Assert.AreEqual("123", entity.CreatedUserId);
            Assert.IsNotNull(entity.CreatedDate);

            Assert.AreEqual("Jane", entity.UpdatedBy);
            Assert.AreEqual("456", entity.UpdatedUserId);
            Assert.IsNotNull(entity.UpdatedDate);
        }

        private class TestAuditableEntity : BaseAuditableEntity
        {
        }
    }
}
