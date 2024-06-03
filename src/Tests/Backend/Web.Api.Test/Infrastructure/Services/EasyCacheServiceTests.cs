using EasyCaching.Core;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Web.Api.Test.Infrastructure.Services
{
    [TestFixture]
    public class EasyCacheServiceTests
    {
        private Mock<IEasyCachingProvider> _mockCachingProvider;
        private Mock<IConfiguration> _mockConfiguration;
        private EasyCacheService _cacheService;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();

            _mockCachingProvider = new Mock<IEasyCachingProvider>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            {"RedisConnectionSettings:DefaultExpiration", "00:05:00"}
                })
                .Build();
            _cacheService = new EasyCacheService(_mockCachingProvider.Object, configuration);
        }

        [Test]
        public async Task GetAsync_WithValidKeyAndType_ReturnsCachedData()
        {
            // Arrange
            string key = "testKey";
            Type cachedDataType = typeof(string);
            object expectedData = "testData";
            var cancellationToken = new CancellationToken();

            _mockCachingProvider.Setup(x => x.GetAsync(key, cachedDataType, cancellationToken)).ReturnsAsync(expectedData);

            // Act
            var result = await _cacheService.GetAsync(key, cachedDataType);

            // Assert
            Assert.AreEqual(expectedData, result);
        }

        [Test]
        public async Task GetAsync_WithValidKeyAndGenericType_ReturnsCachedData()
        {
            // Arrange
            string key = "testKey";
            string expectedData = "testData";
            var cancellationToken = new CancellationToken();

            _mockCachingProvider.Setup(x => x.GetAsync<string>(key, cancellationToken)).ReturnsAsync(new CacheValue<string>(expectedData, true));

            // Act
            var result = await _cacheService.GetAsync<string>(key);

            // Assert
            Assert.AreEqual(expectedData, result);
        }

        [Test]
        public async Task RemoveAsync_WithValidKey_CallsRemoveAsyncOnCachingProvider()
        {
            // Arrange
            string key = "testKey";
            var cancellationToken = new CancellationToken();

            // Act
            await _cacheService.RemoveAsync(key);

            // Assert
            _mockCachingProvider.Verify(x => x.RemoveAsync(key, cancellationToken), Times.Once);
        }

        [Test]
        public async Task RemoveByPrefixAsync_WithValidPrefix_CallsRemoveByPrefixAsyncOnCachingProvider()
        {
            // Arrange
            string prefix = "testPrefix";
            var cancellationToken = new CancellationToken();

            // Act
            await _cacheService.RemoveByPrefixAsync(prefix);

            // Assert
            _mockCachingProvider.Verify(x => x.RemoveByPrefixAsync(prefix, cancellationToken), Times.Once);
        }

        [Test]
        public async Task SetAsync_WithExpiration_SetsDataWithExpiration()
        {
            // Arrange
            string key = "testKey";
            string value = "testValue";
            TimeSpan expiration = TimeSpan.FromMinutes(10);
            var cancellationToken = new CancellationToken();

            // Act
            await _cacheService.SetAsync(key, value, expiration);

            // Assert
            _mockCachingProvider.Verify(x => x.SetAsync(key, value, expiration, cancellationToken), Times.Once);
        }

        [Test]
        public async Task SetAsync_WithoutExpiration_SetsDataWithDefaultExpirationFromConfiguration()
        {
            // Arrange
            string key = "testKey";
            string value = "testValue";
            TimeSpan defaultExpiration = TimeSpan.FromMinutes(5);
            var cancellationToken = new CancellationToken();

            _mockConfiguration.Setup(x => x["RedisConnectionSettings:DefaultExpiration"]).Returns("00:05:00");

            // Act
            await _cacheService.SetAsync(key, value);

            // Assert
            _mockCachingProvider.Verify(x => x.SetAsync(key, value, defaultExpiration, cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetAnyAsync_WithValidKey_ReturnsCachedValue()
        {
            // Arrange
            string key = "testKey";
            bool expectedValue = true;
            var cancellationToken = new CancellationToken();

            _mockCachingProvider.Setup(x => x.GetAsync<bool?>(key, cancellationToken)).ReturnsAsync(new CacheValue<bool?>(expectedValue, true));

            // Act
            var result = await _cacheService.GetAnyAsync(key);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }
    }
}
