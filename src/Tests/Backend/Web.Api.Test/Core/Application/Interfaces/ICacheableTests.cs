using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Test.Core.Application.Interfaces
{
    [TestFixture]
    public class ICacheableTests
    {
        [Test]
        public void BypassCache_ShouldHaveGetter()
        {
            // Arrange
            ICacheable cacheable = new MockCacheable();

            // Act
            bool bypassCache = cacheable.BypassCache;

            // Assert
            Assert.That(bypassCache, Is.EqualTo(true));
        }

        [Test]
        public void CacheKey_ShouldHaveGetter()
        {
            // Arrange
            ICacheable cacheable = new MockCacheable();

            // Act
            string cacheKey = cacheable.CacheKey;

            // Assert
            Assert.That(cacheKey, Is.EqualTo("test_cache_key"));
        }

        private class MockCacheable : ICacheable
        {
            public bool BypassCache => true;
            public string CacheKey => "test_cache_key";
        }
    }
}
