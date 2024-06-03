using Application.Behaviors;
using Application.Interfaces;
using Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Web.Api.Test.Core.Application.Behaviors
{
    [TestFixture]
    public class CachingBehaviorTests
    {
        private Mock<IEasyCacheService> _cacheServiceMock;
        private Mock<ILogger<CachingBehavior<CachingBehaviorRequest, CachingBehaviorResponse>>> _loggerMock;
        private CachingBehavior<CachingBehaviorRequest, CachingBehaviorResponse> _behavior;

        [SetUp]
        public void SetUp()
        {
            _cacheServiceMock = new Mock<IEasyCacheService>();
            _loggerMock = new Mock<ILogger<CachingBehavior<CachingBehaviorRequest, CachingBehaviorResponse>>>();
            _behavior = new CachingBehavior<CachingBehaviorRequest, CachingBehaviorResponse>(_cacheServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_CacheableQueryWithBypassCacheTrue_ShouldNotCache()
        {
            // Arrange
            var request = new CachingBehaviorRequest { BypassCache = true };
            var next = new RequestHandlerDelegate<CachingBehaviorResponse>(() => Task.FromResult(new CachingBehaviorResponse()));

            // Act
            var response = await _behavior.Handle(request, next, CancellationToken.None);

            // Assert
            _cacheServiceMock.Verify(s => s.GetAsync(It.IsAny<string>(), typeof(CachingBehaviorResponse)), Times.Never);
            _cacheServiceMock.Verify(s => s.SetAsync(It.IsAny<string>(), It.IsAny<CachingBehaviorResponse>(), It.IsAny<TimeSpan?>()), Times.Never);

            _loggerMock.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), null, (Func<object, Exception, string>)It.IsAny<object>()), Times.Never);
        }

        [Test]
        public async Task Handle_CacheableQueryWithBypassCacheFalseAndCachedResponseExists_ShouldReturnCachedResponse()
        {
            // Arrange
            var request = new CachingBehaviorRequest { BypassCache = false, CacheKey = "key" };
            var cachedResponse = new CachingBehaviorResponse();
            var next = new RequestHandlerDelegate<CachingBehaviorResponse>(() => Task.FromResult(new CachingBehaviorResponse()));
            _cacheServiceMock.Setup(s => s.GetAsync("key", typeof(CachingBehaviorResponse))).ReturnsAsync(cachedResponse);

            // Act
            var response = await _behavior.Handle(request, next, CancellationToken.None);

            // Assert
            Assert.AreEqual(cachedResponse, response);
            _cacheServiceMock.Verify(s => s.GetAsync("key", typeof(CachingBehaviorResponse)), Times.Once);
            _cacheServiceMock.Verify(s => s.SetAsync(It.IsAny<string>(), It.IsAny<CachingBehaviorResponse>(), It.IsAny<TimeSpan?>()), Times.Never);
            //_loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);

        }

        [Test]
        public async Task Handle_CacheableQueryWithBypassCacheFalseAndNoCachedResponse_ShouldAddResponseToCache()
        {
            // Arrange
            var request = new CachingBehaviorRequest { BypassCache = false, CacheKey = "key" };
            var nextResponse = new CachingBehaviorResponse();
            var next = new RequestHandlerDelegate<CachingBehaviorResponse>(() => Task.FromResult(nextResponse));
            _cacheServiceMock.Setup(s => s.GetAsync("key", typeof(CachingBehaviorResponse))).Returns(Task.FromResult<object>(null));

            // Act
            var response = await _behavior.Handle(request, next, CancellationToken.None);

            // Assert
            Assert.AreEqual(nextResponse, response);
            _cacheServiceMock.Verify(s => s.GetAsync("key", typeof(CachingBehaviorResponse)), Times.Once);
            _cacheServiceMock.Verify(s => s.SetAsync("key", nextResponse, It.IsAny<TimeSpan?>()), Times.Once);

            //_loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }
    }

    public class CachingBehaviorRequest : IRequest<CachingBehaviorResponse>, ICacheable
    {
        public bool BypassCache { get; set; }
        public string CacheKey { get; set; }
    }

    public class CachingBehaviorResponse { }
}
