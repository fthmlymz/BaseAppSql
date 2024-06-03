using Domain.Common;
using Moq;
using Persistence.Context;
using Persistence.Repositories;

namespace Web.Api.Test.Persistence.Repositories
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void SetUp()
        {
            //_dbContextMock = new Mock<ApplicationDbContext>();
            _dbContextMock = _dbContextMock ?? new Mock<ApplicationDbContext>();
            _unitOfWork = new UnitOfWork(_dbContextMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork.Dispose();
        }

        [Test]
        public void Repository_WhenCalledForTheFirstTime_CreatesNewRepositoryInstance()
        {
            // Arrange
            var repositoryType = typeof(GenericRepository<>);
            var entityType = typeof(UnitOfWorkTestEntity);
            var expectedRepositoryType = repositoryType.MakeGenericType(entityType);

            // Act
            var repository = _unitOfWork.Repository<UnitOfWorkTestEntity>();

            // Assert
            Assert.IsInstanceOf(repositoryType.MakeGenericType(entityType), repository);
            Assert.IsInstanceOf(expectedRepositoryType, repository);
        }

        [Test]
        public void Repository_WhenCalledMultipleTimesForTheSameType_ReturnsSameRepositoryInstance()
        {
            // Arrange
            var entityType = typeof(UnitOfWorkTestEntity);

            // Act
            var repository1 = _unitOfWork.Repository<UnitOfWorkTestEntity>();
            var repository2 = _unitOfWork.Repository<UnitOfWorkTestEntity>();

            // Assert
            Assert.AreSame(repository1, repository2);
            Assert.IsInstanceOf<GenericRepository<UnitOfWorkTestEntity>>(repository1);
            Assert.IsInstanceOf<GenericRepository<UnitOfWorkTestEntity>>(repository2);
            Assert.IsTrue(repository1.GetType().Equals(repository2.GetType()));
        }

        [Test]
        public void SaveChanges_InvokesSaveChangesOnDbContext()
        {
            // Act
            _unitOfWork.SaveChanges();

            // Assert
            _dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public async Task SaveChangesAsync_InvokesSaveChangesAsyncOnDbContext()
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            // Act
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Assert
            _dbContextMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Test]
        public void Dispose_DisposesDbContextAndClearsRepositories()
        {
            // Act
            _unitOfWork.Dispose();

            // Assert
            _dbContextMock.Verify(x => x.Dispose(), Times.Once);
            Assert.IsNull(_unitOfWork.GetType().GetField("_repositories", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_unitOfWork));
        }

        public class UnitOfWorkTestEntity : BaseAuditableEntity
        {
            public string Name { get; set; }
        }
    }
}





#region Rollback not working
/*[Test]
public async Task Rollback_ShouldRevertChanges_AfterAddingEntity()
{
    // Arrange
    var mockDbContext = new Mock<ApplicationDbContext>();
    var mockDbSet = new Mock<DbSet<UnitOfWorkTestEntity>>();

    var unitOfWork = new UnitOfWork(mockDbContext.Object);
    var testEntity = new UnitOfWorkTestEntity { Id = 1, Name = "Test" };

    // Mock DbSet'inin AddAsync metodunu yalnızca bir kez çağırmayı bekleyin
    //mockDbSet.Setup(x => x.AddAsync(It.IsAny<UnitOfWorkTestEntity>(), It.IsAny<CancellationToken>()))
    //         .Returns(ValueTask.CompletedTask)
    //         .Verifiable();
    mockDbSet.Setup(x => x.AddAsync(It.IsAny<UnitOfWorkTestEntity>(), It.IsAny<CancellationToken>()))
             .Returns(new ValueTask<EntityEntry<UnitOfWorkTestEntity>>(mockDbContext.Object.Entry(testEntity)))
             .Verifiable();

    mockDbContext.Setup(x => x.Set<UnitOfWorkTestEntity>()).Returns(mockDbSet.Object);

    // Act
    await unitOfWork.Repository<UnitOfWorkTestEntity>().AddAsync(testEntity);
    await unitOfWork.Rollback();

    // Assert
    mockDbSet.Verify(x => x.AddAsync(testEntity, It.IsAny<CancellationToken>()), Times.Once);
    mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
}

/*[Test]
public async Task Rollback_ShouldRevertChanges_AfterAddingEntity()
{
    // Arrange
    var mockDbSet = new Mock<DbSet<UnitOfWorkTestEntity>>();

    var mockDbContext = new Mock<ApplicationDbContext>();
    mockDbContext.Setup(x => x.Set<UnitOfWorkTestEntity>()).Returns(mockDbSet.Object);

    var unitOfWork = new UnitOfWork(mockDbContext.Object);
    var testEntity = new UnitOfWorkTestEntity { Id = 1, Name = "Test" };

    // Mock DbSet'inin AddAsync metodunu yalnızca bir kez çağırmayı bekleyin
    mockDbSet.Setup(x => x.AddAsync(It.IsAny<UnitOfWorkTestEntity>(), It.IsAny<CancellationToken>()))
             .Returns(new ValueTask<EntityEntry<UnitOfWorkTestEntity>>(mockDbContext.Object.Entry(testEntity)))
             .Verifiable();

    // Act
   // await unitOfWork.Repository<UnitOfWorkTestEntity>().AddAsync(testEntity);
    await unitOfWork.Rollback();

    // Assert
    //mockDbSet.Verify(x => x.AddAsync(testEntity, It.IsAny<CancellationToken>()), Times.Once);
    //mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
}
[Test]
public async Task Rollback_ShouldRevertChanges_AfterAddingEntity()
{
    // Arrange
    var mockDbSet = new Mock<DbSet<UnitOfWorkTestEntity>>();
    var mockDbContext = new Mock<ApplicationDbContext>();

    // DbSet mock nesnesinin Set metodu çağrıldığında mockDbSet nesnesini döndürmesini sağlayın
    mockDbContext.Setup(x => x.Set<UnitOfWorkTestEntity>()).Returns(mockDbSet.Object);

    // UnitOfWork sınıfını mockDbContext nesnesiyle başlatın
    var unitOfWork = new UnitOfWork(mockDbContext.Object);

    // Act
    await unitOfWork.Rollback();

    // Assert
    // Geri alma işlemi yapıldıktan sonra beklenen davranışları test edin
}


//[Test]
//public async Task Rollback_ShouldReloadAllEntriesInChangeTracker()
//{
//    var test = new Mock<UnitOfWork>();

//    await test.Object.Rollback();

//    test.Verify(u => u.Rollback(), Times.Once);
//}

//[Test]
//public void Rollback_ShouldReloadAllEntriesInChangeTracker()
//{
//    // Arrange
//    var entity1 = new UnitOfWorkTestEntity { Id = 1, Name = "Entity 1" };
//    var entity2 = new UnitOfWorkTestEntity { Id = 2, Name = "Entity 2" };
//    var entity3 = new UnitOfWorkTestEntity { Id = 3, Name = "Entity 3" };
//    var changeTrackerEntries = new List<object> { entity1, entity2, entity3 };
//    _dbContextMock.Setup(m => m.ChangeTracker.Entries()).Returns(changeTrackerEntries.Select(e => Mock.Of<EntityEntry>(entry => entry.Entity == e)).ToList());

//    // Act
//    _unitOfWork.Rollback();

//    //// Assert
//    //_dbContextMock.Verify(m => m.ChangeTracker.Entries(), Times.Once);
//    //Assert.AreEqual(entity1.Id, 1);
//    //Assert.AreEqual(entity1.Name, "Entity 1");
//    //Assert.AreEqual(entity2.Id, 2);
//    //Assert.AreEqual(entity2.Name, "Entity 2");
//    //Assert.AreEqual(entity3.Id, 3);
//    //Assert.AreEqual(entity3.Name, "Entity 3");
//}

//Hiçbir değişiklik yapılmamış senaryo
[Test]
public void Rollback_ShouldDoNothing_IfNoChangesMade()
{
    // Arrange
    var mockDbContext = new Mock<ApplicationDbContext>();
    var unitOfWork = new UnitOfWork(mockDbContext.Object);

    // Act
    unitOfWork.Rollback();

    // Assert
    mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
}

//veri eklendikten sonra
[Test]
public async Task Rollback_ShouldRevertChanges_AfterAddingEntity()
{
    // Arrange
    var mockDbContext = new Mock<ApplicationDbContext>();
    var unitOfWork = new UnitOfWork(mockDbContext.Object);
    var testEntity = new UnitOfWorkTestEntity { Id = 1, Name = "Test" };

    // Act
    unitOfWork.Repository<UnitOfWorkTestEntity>().AddAsync(testEntity);
   // await unitOfWork.Rollback();

    // Assert
 //   mockDbContext.Verify(x => x.Set<UnitOfWorkTestEntity>().Add(testEntity), Times.Once);
 //   mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
}

//Veri güncellendikten sonra
 [Test]
 public async Task Rollback_ShouldRevertChanges_AfterUpdatingEntity()
 {
     // Arrange
     var mockDbContext = new Mock<ApplicationDbContext>();
     var unitOfWork = new UnitOfWork(mockDbContext.Object);
     var existingEntity = new UnitOfWorkTestEntity { Id = 1, Name = "Old Name" };
     mockDbContext.Setup(x => x.Set<UnitOfWorkTestEntity>()).Returns(DbSetMock.Create<UnitOfWorkTestEntity>(existingEntity));

     existingEntity.Name = "New Name";

     // Act
     unitOfWork.Repository<UnitOfWorkTestEntity>().UpdateAsync(existingEntity);
     await unitOfWork.Rollback();

     // Assert
     mockDbContext.Verify(x => x.Set<UnitOfWorkTestEntity>().Update(existingEntity), Times.Once);
     Assert.AreEqual("Old Name", existingEntity.Name); // Değişiklik geri alındı mı?
     mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
 }
 //veri silindikten sonra
 [Test]
 public async Task Rollback_ShouldRevertChanges_AfterDeletingEntity()
 {
     // Arrange
     var mockDbContext = new Mock<ApplicationDbContext>();
     var unitOfWork = new UnitOfWork(mockDbContext.Object);
     var existingEntity = new UnitOfWorkTestEntity { Id = 1, Name = "Test" };
     mockDbContext.Setup(x => x.Set<UnitOfWorkTestEntity>()).Returns(.Create<UnitOfWorkTestEntity>(existingEntity));

     // Act
     unitOfWork.Repository<UnitOfWorkTestEntity>().Delete(existingEntity);
     await unitOfWork.Rollback();

     // Assert
     mockDbContext.Verify(x => x.Set<UnitOfWorkTestEntity>().Remove(existingEntity), Times.Once);
     mockDbContext.Verify(x => x.Set<UnitOfWorkTestEntity>()).Contains(existingEntity); // Silme işlemi geri alındı mı?
     mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
 }*/
#endregion
