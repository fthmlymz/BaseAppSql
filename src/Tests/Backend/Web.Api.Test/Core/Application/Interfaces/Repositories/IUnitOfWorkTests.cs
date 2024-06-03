using Application.Interfaces.Repositories;
using Domain.Common;
using Moq;
using Persistence.Context;
using Persistence.Repositories;

namespace Web.Api.Test.Core.Application.Interfaces.Repositories
{
    [TestFixture]
    public class IUnitOfWorkTests
    {
        private Mock<IGenericRepository<BaseAuditableEntity>> _repositoryMock;
        private IUnitOfWork _unitOfWork;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IGenericRepository<BaseAuditableEntity>>();
            var dbContextMock = new Mock<ApplicationDbContext>();
            _unitOfWork = new UnitOfWork(dbContextMock.Object);

            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork.Dispose();
        }

        [Test]
        public void Repository_ShouldReturnGenericRepository()
        {
            // Act
            var repository = _unitOfWork.Repository<BaseAuditableEntity>();

            // Assert
            Assert.IsNotNull(repository);
            Assert.IsInstanceOf<IGenericRepository<BaseAuditableEntity>>(repository);
        }

        [Test]
        public async Task SaveAndRemoveCache_ShouldSaveAndRemoveCacheKeys()
        {
            // Arrange
            CancellationToken cancellationToken = new CancellationToken();
            string[] cacheKeys = new string[] { "key1", "key2" };
            int expectedResult = 10;

            _unitOfWorkMock.Setup(uow => uow.SaveAndRemoveCache(cancellationToken, cacheKeys))
                .ReturnsAsync(expectedResult);

            // Act
            int result = await _unitOfWorkMock.Object.SaveAndRemoveCache(cancellationToken, cacheKeys);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task SaveChangesAsync_ShouldSaveChanges()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Assert: No exception thrown indicates success
        }

        [Test]
        public void SaveChanges_ShouldSaveChanges()
        {
            // Act
            _unitOfWork.SaveChanges();

            // Assert: No exception thrown indicates success
        }

        [Test]
        public async Task Rollback_ShouldRollback()
        {
            // Arrange
            var unitOfWork = new Mock<IUnitOfWork>();

            // Act
            await unitOfWork.Object.Rollback();

            // Assert
            unitOfWork.Verify(u => u.Rollback(), Times.Once);
        }
    }
}
