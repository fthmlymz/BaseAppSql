using Shared;

namespace Web.Api.Test.Shared
{
    public class PaginatedResultTest
    {
        [Test]
        public void Create_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var data = new List<int> { 1, 2, 3 };
            var count = 3;
            var pageNumber = 1;
            var pageSize = 10;

            // Act
            var result = PaginatedResult<int>.Create(data, count, pageNumber, pageSize);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(data, result.Data);
            Assert.AreEqual(pageNumber, result.CurrentPage);
            Assert.AreEqual((count + pageSize - 1) / pageSize, result.TotalPages);
            Assert.AreEqual(count, result.TotalCount);
            Assert.IsFalse(result.HasPreviousPage);
            Assert.IsFalse(result.HasNextPage);
        }
    }
}
