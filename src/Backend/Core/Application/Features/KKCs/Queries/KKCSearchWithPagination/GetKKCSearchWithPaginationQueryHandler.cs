using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using System.Linq.Expressions;

namespace Application.Features.KKCs.Queries.KKCSearchWithPagination
{
    internal class GetKKCSearchWithPaginationQueryHandler : IRequestHandler<GetKKCSearchWithPaginationQuery, PaginatedResult<GetKKCSearchWithPaginationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEasyCacheService _cacheService;
        private readonly ILogger<GetKKCSearchWithPaginationQueryHandler> _logger;

        public GetKKCSearchWithPaginationQueryHandler(IUnitOfWork unitOfWork, IEasyCacheService cacheService, ILogger<GetKKCSearchWithPaginationQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<PaginatedResult<GetKKCSearchWithPaginationDto>> Handle(GetKKCSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = GetCacheKey(request);
            var cacheResult = await _cacheService.GetAsync<PaginatedResult<GetKKCSearchWithPaginationDto>>(cacheKey);
            if (cacheResult != null)
            {
                _logger.LogInformation($"KKC found in cache: {request.Name}");
                return cacheResult;
            }

            var result = await SearchProducts(request);
            await _cacheService.SetAsync(cacheKey, result);

            _logger.LogInformation($"KKC not found in cache: {request.Name}");

            return result;
        }

        private string GetCacheKey(GetKKCSearchWithPaginationQuery request)
        {
            var cacheKey = "KKCSearch_";

            foreach (var property in typeof(GetKKCSearchWithPaginationQuery).GetProperties())
            {
                var value = property.GetValue(request);
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    cacheKey += $"{property.Name}_{value}_";
                }
            }

            cacheKey = cacheKey.TrimEnd('_');

            _logger.LogInformation($"KKC cache key generated: {cacheKey}");

            return cacheKey;
        }

        private async Task<PaginatedResult<GetKKCSearchWithPaginationDto>> SearchProducts(GetKKCSearchWithPaginationQuery request)
        {
            var searchPredicate = BuildSearchPredicate(request);

            var totalCount = await _unitOfWork.Repository<KKC>().Where(searchPredicate).CountAsync();

            var result = await _unitOfWork.Repository<KKC>()
                .Where(searchPredicate)
                .OrderBy(p => p.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetKKCSearchWithPaginationDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    DeviceIp = p.DeviceIp,
                    DeviceId = p.DeviceId,
                    DevicePort = p.DevicePort,
                    TenantId = p.TenantId,
                    Description = p.Description,
                    CreatedBy = p.CreatedBy,
                    CreatedDate = p.CreatedDate,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedDate = p.UpdatedDate,
                }).ToListAsync();

            return PaginatedResult<GetKKCSearchWithPaginationDto>.Create(result, totalCount, request.PageNumber, request.PageSize);
        }

        private Expression<Func<KKC, bool>> BuildSearchPredicate(GetKKCSearchWithPaginationQuery request)
        {
            var parameter = Expression.Parameter(typeof(KKC), "kkc");
            Expression predicate = Expression.Constant(true);

            // KKC entity'sinde tanımlı özellikleri alıyoruz
            var kkcProperties = typeof(KKC).GetProperties().Select(p => p.Name).ToHashSet();

            foreach (var property in typeof(GetKKCSearchWithPaginationQuery).GetProperties())
            {
                // Yalnızca KKC entity'sinde olan özellikleri işleme alıyoruz
                if (!kkcProperties.Contains(property.Name))
                {
                    continue;
                }

                var value = property.GetValue(request);
                if (value != null)
                {
                    var propertyType = property.PropertyType;
                    var kkcProperty = Expression.Property(parameter, property.Name);

                    if (propertyType == typeof(string))
                    {
                        predicate = Expression.AndAlso(
                            predicate,
                            Expression.Call(
                                kkcProperty,
                                nameof(string.Contains),
                                Type.EmptyTypes,
                                Expression.Constant(value, typeof(string))
                            )
                        );
                    }
                    else if (propertyType == typeof(int) || propertyType == typeof(int?))
                    {
                        predicate = Expression.AndAlso(
                            predicate,
                            Expression.Equal(
                                kkcProperty,
                                Expression.Constant(value, typeof(int))
                            )
                        );
                    }
                    else if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    {
                        predicate = Expression.AndAlso(
                            predicate,
                            Expression.Equal(
                                kkcProperty,
                                Expression.Constant(value, typeof(Guid))
                            )
                        );
                    }
                    else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                    {
                        predicate = Expression.AndAlso(
                            predicate,
                            Expression.Equal(
                                kkcProperty,
                                Expression.Constant(value, typeof(DateTime))
                            )
                        );
                    }
                }
            }

            return Expression.Lambda<Func<KKC, bool>>(predicate, parameter);
        }
    }
}
