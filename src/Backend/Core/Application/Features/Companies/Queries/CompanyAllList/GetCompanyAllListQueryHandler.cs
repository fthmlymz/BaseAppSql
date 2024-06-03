using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Companies.Queries.CompanyAllList
{
    /// <summary>
    /// Represents the handler for the GetCompanyAllListQuery.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request. It should be GetCompanyAllListQuery.</typeparam>
    /// <typeparam name="TResponse">The type of the response. It should be Result<List<GetCompanyAllListDto>>.</typeparam>
    internal class GetCompanyAllListQueryHandler : IRequestHandler<GetCompanyAllListQuery, Result<List<GetCompanyAllListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetCompanyAllListQueryHandler> _logger;
        private readonly IEasyCacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCompanyAllListQueryHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        public GetCompanyAllListQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCompanyAllListQueryHandler> logger, IEasyCacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Handles the GetCompanyAllListQuery request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the GetCompanyAllListQuery request.</returns>
        public async Task<Result<List<GetCompanyAllListDto>>> Handle(GetCompanyAllListQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "CompanyAllList";
            var cachedResult = await _cacheService.GetAsync<List<GetCompanyAllListDto>>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation("GetCompanyAllList cached");
                return Result<List<GetCompanyAllListDto>>.Success(cachedResult);
            }
            _logger.LogInformation("CompanyAllList not found in cache. Fetching from database...");

            var companies = await _unitOfWork.Repository<Company>().Entities.ToListAsync(cancellationToken);
            _logger.LogInformation($"Fetched {companies.Count} companies from database");

            var dtos = companies.Select(company => new GetCompanyAllListDto
            {
                Id = company.Id,
                Name = company.Name,
                Guid = company.Guid,
                Description = company.Description,
                TenantId = company.TenantId,
                CreatedBy = company.CreatedBy,
                CreatedDate = company.CreatedDate,
                UpdatedBy = company.UpdatedBy,
                UpdatedDate = company.UpdatedDate
            }).ToList();

            _logger.LogInformation($"Mapped {dtos.Count} companies to DTOs");

            await _cacheService.SetAsync(cacheKey, dtos);
            _logger.LogInformation("CompanyAllList set cached");

            return Result<List<GetCompanyAllListDto>>.Success(dtos, "CompanyAllList fetched successfully");
        }
    }
}
