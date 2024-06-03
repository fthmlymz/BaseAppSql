using Application.Interfaces;
using Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEasyCacheService _easyCacheService;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(IEasyCacheService easyCacheService, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _easyCacheService = easyCacheService;
            _logger = logger;
        }

        /// <summary>
        /// Handles the request asynchronously and performs caching if the request implements the ICacheable interface.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="next">The next request handler delegate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheable cacheableQuery)
            {
                TResponse response;

                if (cacheableQuery.BypassCache) return await next();

                async Task<TResponse> GetResponseAndAddToCache()
                {
                    response = await next();

                    await _easyCacheService.SetAsync(cacheableQuery.CacheKey, response);
                    return response;
                }
                var cachedResponse = await _easyCacheService.GetAsync(cacheableQuery.CacheKey, typeof(TResponse));
                if (cachedResponse != null)
                {
                    response = (TResponse)cachedResponse;
                    _logger.LogInformation($"Fetched from Cache -> '{cacheableQuery.CacheKey}'.");
                }
                else
                {
                    response = await GetResponseAndAddToCache();
                    _logger.LogInformation($"Added to Cache -> '{cacheableQuery.CacheKey}'.");
                }
                return response;
            }
            else
            {
                return await next();
            }
        }
    }
}
