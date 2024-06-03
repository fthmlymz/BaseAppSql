using Application.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Test.Core.Application.Extensions
{
    [TestFixture]
    public class QueryableExtensionsTests
    {
        private List<QueryableExtensionsTestEntity> _testEntities;
        private DbContextOptions<QueryableExtension> _options;

        [SetUp]
        public void Setup()
        {
            _testEntities = new List<QueryableExtensionsTestEntity>
            {
                new QueryableExtensionsTestEntity { Id = 1, Name = "Entity 1" },
                new QueryableExtensionsTestEntity { Id = 2, Name = "Entity 2" },
                new QueryableExtensionsTestEntity { Id = 3, Name = "Entity 3" },
                new QueryableExtensionsTestEntity { Id = 4, Name = "Entity 4" },
                new QueryableExtensionsTestEntity { Id = 5, Name = "Entity 5" }
            };

            _options = new DbContextOptionsBuilder<QueryableExtension>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        [Test]
        public async Task ToPaginatedListAsync_ShouldReturnCorrectPaginatedResult()
        {
            // Arrange
            using var dbContext = new QueryableExtension(_options);
            dbContext.TestEntities.AddRange(_testEntities);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await dbContext.TestEntities.ToPaginatedListAsync(1, 2, CancellationToken.None);

            // Assert
            Assert.AreEqual(2, result.Data.Count);
            //Assert.AreEqual(5, result.TotalCount);
            Assert.AreEqual(1, result.CurrentPage);
            Assert.AreEqual(2, result.PageSize);
        }

        [Test]
        public async Task ToPaginatedListAsync_ShouldReturnEmptyListWhenNoRecords()
        {
            // Arrange
            using var dbContext = new QueryableExtension(_options);

            // Act
            var result = await dbContext.TestEntities.ToPaginatedListAsync(1, 2, CancellationToken.None);

            //// Assert
          //  Assert.IsEmpty(result.Data);
            //Assert.AreEqual(0, result.TotalCount);
            Assert.AreEqual(1, result.CurrentPage);
            Assert.AreEqual(2, result.PageSize);
        }

        private class QueryableExtensionsTestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class QueryableExtension : DbContext
        {
            public QueryableExtension(DbContextOptions<QueryableExtension> options) : base(options)
            {
            }

            public DbSet<QueryableExtensionsTestEntity> TestEntities { get; set; }
        }
    }
}
