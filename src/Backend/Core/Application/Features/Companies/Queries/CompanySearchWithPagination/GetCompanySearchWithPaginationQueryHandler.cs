using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using System.Linq.Expressions;

namespace Application.Features.Companies.Queries.CompanySearchWithPagination
{
    internal class GetCompanySearchWithPaginationQueryHandler : IRequestHandler<GetCompanySearchWithPaginationQuery, PaginatedResult<GetCompanySearchWithPaginationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEasyCacheService _cacheService;
        private readonly ILogger<GetCompanySearchWithPaginationQueryHandler> _logger;

        public GetCompanySearchWithPaginationQueryHandler(IUnitOfWork unitOfWork, IEasyCacheService cacheService, ILogger<GetCompanySearchWithPaginationQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _logger = logger;
        }

        /// <summary>
        /// Handles the GetCompanySearchWithPaginationQuery request and returns a PaginatedResult of GetCompanySearchWithPaginationDto.
        /// </summary>
        /// <param name="request">The GetCompanySearchWithPaginationQuery request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a PaginatedResult of GetCompanySearchWithPaginationDto.</returns>
        public async Task<PaginatedResult<GetCompanySearchWithPaginationDto>> Handle(GetCompanySearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Company handle method called with request: {request}");

            var cacheKey = GetCacheKey(request);
            _logger.LogInformation($"Company cache key generated: {cacheKey}");

            var cacheResult = await _cacheService.GetAsync<PaginatedResult<GetCompanySearchWithPaginationDto>>(cacheKey);
            _logger.LogInformation($"Company cache result retrieved: {cacheResult}");

            if (cacheResult != null)
            {
                _logger.LogInformation($"Company found in cache: {request.Name}");
                return cacheResult;
            }

            var result = await SearchCompanies(request);
            _logger.LogInformation($"Search companies method called with request: {request}");
            _logger.LogInformation($"Search companies result: {result}");

            await _cacheService.SetAsync(cacheKey, result);
            _logger.LogInformation($"Company cached: {cacheKey}");

            return result;
        }

        /// <summary>
        /// Generates a cache key for the given GetCompanySearchWithPaginationQuery request.
        /// The cache key is a string that includes the values of all non-null and non-empty properties of the request.
        /// </summary>
        /// <param name="request">The GetCompanySearchWithPaginationQuery request. It is of type GetCompanySearchWithPaginationQuery.</param>
        /// <returns>A string representing the cache key. It is of type string.</returns>
        private string GetCacheKey(GetCompanySearchWithPaginationQuery request)
        {
            var cacheKey = "CompanySearch_";
            foreach (var property in typeof(GetCompanySearchWithPaginationQuery).GetProperties())
            {
                var value = property.GetValue(request);
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    cacheKey += value.ToString() + "_";
                }
            }
            cacheKey = cacheKey.TrimEnd('_');
            return cacheKey;
        }

        /// <summary>
        /// Searches for companies based on the provided criteria and returns a paginated result.
        /// </summary>
        /// <param name="request">The search request. Includes the search criteria and pagination details.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a PaginatedResult of GetCompanySearchWithPaginationDto.</returns>
        private async Task<PaginatedResult<GetCompanySearchWithPaginationDto>> SearchCompanies(GetCompanySearchWithPaginationQuery request)
        {
            _logger.LogInformation($"SearchCompanies called with request: {request}");

            var searchPredicate = BuildSearchPredicate(request);

            var totalCount = await _unitOfWork.Repository<Company>()
                .Where(searchPredicate)
                .CountAsync();

            _logger.LogInformation($"Company total count of companies: {totalCount}");

            var result = await _unitOfWork.Repository<Company>()
                .Where(searchPredicate)
                .OrderBy(p => nameof(p.Name))
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetCompanySearchWithPaginationDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    TenantId = p.TenantId,
                    Guid = p.Guid,
                    CreatedBy = p.CreatedBy,
                    CreatedDate = p.CreatedDate,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedDate = p.UpdatedDate
                }).ToListAsync();

            _logger.LogInformation($"Company search results: {result}");

            return PaginatedResult<GetCompanySearchWithPaginationDto>.Create(result, totalCount, request.PageNumber, request.PageSize);
        }

        /// <summary>
        /// Builds a search predicate for the Company entity based on the properties of the provided GetCompanySearchWithPaginationQuery request.
        /// The search predicate checks if any of the string properties of the request are contained in the corresponding string properties of the Company entity.
        /// The search is case-insensitive.
        /// </summary>
        /// <param name="request">The GetCompanySearchWithPaginationQuery request. It is of type GetCompanySearchWithPaginationQuery.</param>
        /// <returns>A Expression<Func<Company, bool>> representing the search predicate. The predicate returns true if any of the search fields match, false otherwise.</returns>
        private Expression<Func<Company, bool>> BuildSearchPredicate(GetCompanySearchWithPaginationQuery request)
        {
            var parameter = Expression.Parameter(typeof(Company), "p");
            Expression body = Expression.Constant(false);
            bool isAnySearchFieldProvided = false;

            foreach (var property in typeof(GetCompanySearchWithPaginationQuery).GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(request);

                if (propertyValue != null && property.PropertyType == typeof(string))
                {
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var propertyExpression = Expression.Property(parameter, propertyName);
                    var valueExpression = Expression.Constant(propertyValue);
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var propertyToLowerExpression = Expression.Call(propertyExpression, toLowerMethod);
                    var valueToLowerExpression = Expression.Call(valueExpression, toLowerMethod);
                    var containsExpression = Expression.Call(propertyToLowerExpression, containsMethod, valueToLowerExpression);
                    body = Expression.OrElse(body, containsExpression);
                    isAnySearchFieldProvided = true;

                    _logger.LogInformation($"Property {propertyName} with value {propertyValue} is being processed.");
                }
            }
            if (!isAnySearchFieldProvided)
            {
                _logger.LogInformation("Company no search field provided, defaulting to true.");
                return Expression.Lambda<Func<Company, bool>>(Expression.Constant(true), parameter);
            }

            _logger.LogInformation("Company search predicate successfully built.");
            return Expression.Lambda<Func<Company, bool>>(body, parameter);
        }
    }
}
