using Application.Interfaces.Services;
using Moq;

namespace Web.Api.Test.Core.Application.Interfaces.Services
{
    [TestFixture]
    public class EasyCacheServiceTests
    {
        private Mock<IEasyCacheService> _cacheServiceMock;

        [SetUp]
        public void Setup()
        {
            _cacheServiceMock = new Mock<IEasyCacheService>();
        }

        [Test]
        public async Task GetAsync_WithValidKeyAndType_ShouldReturnCachedData()
        {
            // Arrange
            var expectedData = new object();
            var key = "testKey";

            _cacheServiceMock.Setup(x => x.GetAsync(key, typeof(object))).ReturnsAsync(expectedData);

            // Act
            var result = await _cacheServiceMock.Object.GetAsync(key, typeof(object));

            // Assert
            Assert.AreEqual(expectedData, result);
        }

        [Test]
        public async Task GetAsync_WithValidKeyAndGenericType_ShouldReturnCachedData()
        {
            // Arrange
            var expectedData = "testValue";
            var key = "testKey";

            _cacheServiceMock.Setup(x => x.GetAsync<string>(key)).ReturnsAsync(expectedData);

            // Act
            var result = await _cacheServiceMock.Object.GetAsync<string>(key);

            // Assert
            Assert.AreEqual(expectedData, result);
        }

        [Test]
        public async Task GetAnyAsync_WithExistingKey_ShouldReturnTrue()
        {
            // Arrange
            var key = "existingKey";

            _cacheServiceMock.Setup(x => x.GetAnyAsync(key)).ReturnsAsync(true);

            // Act
            var result = await _cacheServiceMock.Object.GetAnyAsync(key);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAnyAsync_WithNonExistingKey_ShouldReturnFalse()
        {
            // Arrange
            var key = "nonExistingKey";

            _cacheServiceMock.Setup(x => x.GetAnyAsync(key)).ReturnsAsync(false);

            // Act
            var result = await _cacheServiceMock.Object.GetAnyAsync(key);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveAsync_WithValidKey_ShouldCallRemoveAsyncMethod()
        {
            // Arrange
            var key = "testKey";

            // Act
            await _cacheServiceMock.Object.RemoveAsync(key);

            // Assert
            _cacheServiceMock.Verify(x => x.RemoveAsync(key), Times.Once);
        }

        [Test]
        public async Task RemoveByPrefixAsync_WithValidPrefix_ShouldCallRemoveByPrefixAsyncMethod()
        {
            // Arrange
            var prefix = "testPrefix";

            // Act
            await _cacheServiceMock.Object.RemoveByPrefixAsync(prefix);

            // Assert
            _cacheServiceMock.Verify(x => x.RemoveByPrefixAsync(prefix), Times.Once);
        }
    }
}
