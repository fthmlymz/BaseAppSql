using Application.Interfaces.Repositories;
using Domain.Common.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace Web.Api.Test.Core.Application.Interfaces.Repositories
{
    [TestFixture]
    public class IGenericRepositoryTests
    {
        private Mock<IGenericRepository<IGenericRepositoryTestEntity>> _repositoryMock;
        private IGenericRepository<IGenericRepositoryTestEntity> _repository;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IGenericRepository<IGenericRepositoryTestEntity>>();
            _repository = _repositoryMock.Object;
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ReturnsEntity()
        {
            // Arrange
            int id = 1;
            IGenericRepositoryTestEntity expectedEntity = new IGenericRepositoryTestEntity { Id = id };
            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(expectedEntity);

            // Act
            IGenericRepositoryTestEntity result = await _repository.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(expectedEntity, result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            List<IGenericRepositoryTestEntity> expectedEntities = new List<IGenericRepositoryTestEntity>
            {
                new IGenericRepositoryTestEntity { Id = 1 },
                new IGenericRepositoryTestEntity { Id = 2 },
                new IGenericRepositoryTestEntity { Id = 3 }
            };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedEntities);

            // Act
            List<IGenericRepositoryTestEntity> result = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(expectedEntities, result);
        }

        [Test]
        public async Task AddAsync_WithValidEntity_ReturnsAddedEntity()
        {
            // Arrange
            IGenericRepositoryTestEntity entity = new IGenericRepositoryTestEntity();
            _repositoryMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);

            // Act
            IGenericRepositoryTestEntity result = await _repository.AddAsync(entity);

            // Assert
            Assert.AreEqual(entity, result);
        }

        [Test]
        public async Task UpdateAsync_WithValidEntity_ReturnsCompletedTask()
        {
            // Arrange
            IGenericRepositoryTestEntity entity = new IGenericRepositoryTestEntity();

            // Act
            Task result = _repository.UpdateAsync(entity);

            // Assert
            Assert.IsTrue(result.IsCompleted);
        }

        [Test]
        public async Task DeleteAsync_WithValidEntity_ReturnsCompletedTask()
        {
            // Arrange
            IGenericRepositoryTestEntity entity = new IGenericRepositoryTestEntity();

            // Act
            Task result = _repository.DeleteAsync(entity);

            // Assert
            Assert.IsTrue(result.IsCompleted);
        }

        [Test]
        public async Task Where_WithValidExpression_ReturnsFilteredEntities()
        {
            // Arrange
            Expression<Func<IGenericRepositoryTestEntity, bool>> expression = e => e.Id > 5;
            IQueryable<IGenericRepositoryTestEntity> expectedEntities = new List<IGenericRepositoryTestEntity>
            {
                new IGenericRepositoryTestEntity { Id = 6 },
                new IGenericRepositoryTestEntity { Id = 7 },
                new IGenericRepositoryTestEntity { Id = 8 }
            }.AsQueryable();
            _repositoryMock.Setup(r => r.Where(expression)).Returns(expectedEntities);

            // Act
            IQueryable<IGenericRepositoryTestEntity> result = _repository.Where(expression);

            // Assert
            Assert.AreEqual(expectedEntities, result);
        }

        [Test]
        public async Task AnyAsync_WithValidExpression_ReturnsTrue()
        {
            // Arrange
            Expression<Func<IGenericRepositoryTestEntity, bool>> expression = e => e.Id > 5;
            _repositoryMock.Setup(r => r.AnyAsync(expression)).ReturnsAsync(true);

            // Act
            bool result = await _repository.AnyAsync(expression);

            // Assert
            Assert.IsTrue(result);
        }
    }

    public class IGenericRepositoryTestEntity : IEntity
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Guid Guid { get; set; }
    }
}
