using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Companies.Queries.CompanyPaginatedAllList
{
    /// <summary>
    /// Handles the <see cref="GetCompanyPaginatedAllListQuery"/> and returns a <see cref="PaginatedResult{GetCompanyPaginatedAllListDto}"/>.
    /// </summary>
    /// <param name="request">The <see cref="GetCompanyPaginatedAllListQuery"/> to handle.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to cancel the operation.</param>
    /// <returns>A <see cref="Task{PaginatedResult{GetCompanyPaginatedAllListDto}}"/> representing the asynchronous operation.</returns>
    internal class GetCompanyPaginatedAllListQueryHandler : IRequestHandler<GetCompanyPaginatedAllListQuery, PaginatedResult<GetCompanyPaginatedAllListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEasyCacheService _cacheService;
        private readonly ILogger<GetCompanyPaginatedAllListQueryHandler> _logger;
        public GetCompanyPaginatedAllListQueryHandler(IUnitOfWork unitOfWork, IEasyCacheService cacheService, ILogger<GetCompanyPaginatedAllListQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _logger = logger;
        }

        /// <summary>
        /// Handles the GetCompanyPaginatedAllListQuery request.
        /// </summary>
        /// <param name="request">The GetCompanyPaginatedAllListQuery request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The paginated result of GetCompanyPaginatedAllListDto.</returns>
        public async Task<PaginatedResult<GetCompanyPaginatedAllListDto>> Handle(GetCompanyPaginatedAllListQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "AllCompanies";
            var cacheResult = await _cacheService.GetAsync<PaginatedResult<GetCompanyPaginatedAllListDto>>(cacheKey);
            if (cacheResult != null)
            {
                _logger.LogInformation($"Company found in cache: {request.PageNumber} and {request.PageSize}");
                return cacheResult;
            }

            var query = _unitOfWork.Repository<Company>().Entities
                .OrderBy(x => nameof(x.Name))
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var totalCount = await _unitOfWork.Repository<Company>().Entities.CountAsync(cancellationToken);

            var dtos = await query.Select(c => new GetCompanyPaginatedAllListDto
            {
                Id = c.Id,
                Name = c.Name,
                Guid = c.Guid,
                Description = c.Description,
                TenantId = c.TenantId,
                CreatedBy = c.CreatedBy,
                CreatedDate = c.CreatedDate,
                UpdatedBy = c.UpdatedBy,
                UpdatedDate = c.UpdatedDate
            }).ToListAsync(cancellationToken);

            _logger.LogInformation("Company found in database");

            return new PaginatedResult<GetCompanyPaginatedAllListDto>(
                succeeded: true,
                data: dtos,
                count: totalCount,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);
        }
    }
}
