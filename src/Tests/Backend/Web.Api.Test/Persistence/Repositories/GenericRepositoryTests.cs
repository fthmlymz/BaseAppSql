using Application.Interfaces.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Context;
using Persistence.Repositories;
using System.Linq.Expressions;

namespace Web.Api.Test.Persistence.Repositories
{
    [TestFixture]
    public class GenericRepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private Mock<DbSet<GenericRepositoryTestEntity>> _dbSetMock;
        private IGenericRepository<GenericRepositoryTestEntity> _repository;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _dbContextMock = new Mock<ApplicationDbContext>();

            // Create DbSet mock
            _dbSetMock = new Mock<DbSet<GenericRepositoryTestEntity>>();

            // Setup DbSet mock
            var entities = new List<GenericRepositoryTestEntity>
            {
                new GenericRepositoryTestEntity { Id = 1, Name = "Entity 1" },
                new GenericRepositoryTestEntity { Id = 2, Name = "Entity 2" },
                new GenericRepositoryTestEntity { Id = 3, Name = "Entity 3" }
            }.AsQueryable();

            _dbSetMock.As<IAsyncEnumerable<GenericRepositoryTestEntity>>()
                .Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<GenericRepositoryTestEntity>(entities.GetEnumerator()));

            _dbSetMock.As<IQueryable<GenericRepositoryTestEntity>>().Setup(x => x.Provider).Returns(entities.Provider);
            _dbSetMock.As<IQueryable<GenericRepositoryTestEntity>>().Setup(x => x.Expression).Returns(entities.Expression);
            _dbSetMock.As<IQueryable<GenericRepositoryTestEntity>>().Setup(x => x.ElementType).Returns(entities.ElementType);
            _dbSetMock.As<IQueryable<GenericRepositoryTestEntity>>().Setup(x => x.GetEnumerator()).Returns(entities.GetEnumerator());

            // Setup _dbContextMock
            _dbContextMock.Setup(x => x.Set<GenericRepositoryTestEntity>()).Returns(_dbSetMock.Object);

            // Create repository using mocked context
            _repository = new GenericRepository<GenericRepositoryTestEntity>(_dbContextMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public async Task GetByIdAsync_Returns_Entity_With_Null_Id()
        {
            // Arrange
            var entityId = 2;

            // Act
            var result = await _repository.GetByIdAsync(entityId);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ReturnsEntity()
        {
            // Arrange
            int entityId = 1;
            var dbContextMock = new Mock<ApplicationDbContext>();
            var entityMock = new Mock<GenericRepositoryTestEntity>();
            var dbSetMock = new Mock<DbSet<GenericRepositoryTestEntity>>();
            dbSetMock.Setup(m => m.FindAsync(entityId)).ReturnsAsync(entityMock.Object);
            dbContextMock.Setup(m => m.Set<GenericRepositoryTestEntity>()).Returns(dbSetMock.Object);

            var repository = new GenericRepository<GenericRepositoryTestEntity>(dbContextMock.Object);

            // Act
            var result = await repository.GetByIdAsync(entityId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task AddAsync_Adds_New_Entity()
        {
            // Arrange
            var newEntity = new GenericRepositoryTestEntity { Id = 4, Name = "Entity4" };

            // Act
            var result = await _repository.AddAsync(newEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newEntity, result);
        }

        [Test]
        public async Task UpdateAsync_Throws_Exception_When_Entity_Is_Null()
        {
            // Arrange
            GenericRepositoryTestEntity entity = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _repository.UpdateAsync(entity);
            });
        }

        [Test]
        public async Task DeleteAsync_ValidEntity_EntityDeleted()
        {
            // Arrange
            var entity = new GenericRepositoryTestEntity();

            // Act
            await _repository.DeleteAsync(entity);

            // Assert
            _dbSetMock.Verify(x => x.RemoveRange(entity), Times.Once);
        }

        [Test]
        public async Task AnyAsync_With_Null_Expression_Returns_False()
        {
            // Arrange
            Expression<Func<GenericRepositoryTestEntity, bool>> expression = null;

            // Act
            var result = await _repository.AnyAsync(expression);

            // Assert
            Assert.IsFalse(result);
        }
    }
    public class GenericRepositoryTestEntity : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }
        public T Current => _enumerator.Current;
        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }
        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return new ValueTask();
        }
    }
}
