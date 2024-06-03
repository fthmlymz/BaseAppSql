using Infrastructure.Settings;

namespace Web.Api.Test.Infrastructure.Settings
{
    [TestFixture]
    public class CacheSettingsTests
    {
        [Test]
        public void CacheSettings_PreferRedis_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var cacheSettings = new CacheSettings();
            var preferRedis = true;

            // Act
            cacheSettings.PreferRedis = preferRedis;

            // Assert
            Assert.AreEqual(preferRedis, cacheSettings.PreferRedis);
        }

        [Test]
        public void CacheSettings_RedisURL_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var cacheSettings = new CacheSettings();
            var redisURL = "redis://localhost";

            // Act
            cacheSettings.RedisURL = redisURL;

            // Assert
            Assert.AreEqual(redisURL, cacheSettings.RedisURL);
        }

        [Test]
        public void CacheSettings_RedisPort_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var cacheSettings = new CacheSettings();
            var redisPort = 6379;

            // Act
            cacheSettings.RedisPort = redisPort;

            // Assert
            Assert.AreEqual(redisPort, cacheSettings.RedisPort);
        }

        [Test]
        public void CacheSettings_Database_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var cacheSettings = new CacheSettings();
            var database = 1;

            // Act
            cacheSettings.Database = database;

            // Assert
            Assert.AreEqual(database, cacheSettings.Database);
        }

        [Test]
        public void CacheSettings_ConnectionTimeout_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var cacheSettings = new CacheSettings();
            var connectionTimeout = 5000;

            // Act
            cacheSettings.ConnectionTimeout = connectionTimeout;

            // Assert
            Assert.AreEqual(connectionTimeout, cacheSettings.ConnectionTimeout);
        }
    }
}
