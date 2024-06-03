using MediatR;
using Shared;

namespace Application.Features.Companies.Queries.CompanyAllList
{
    /// <summary>
    /// Represents the DTO for the Company All List query with pagination.
    /// </summary>
    /// <param name="Name">The name of the company (nullable string).</param>
    /// <param name="Description">The description of the company (nullable string).</param>
    /// <param name="TenantId">The tenantId of the company.</param>
    /// <param name="Guid">The guid of the company (nullable string).</param>
    /// <param name="CreatedBy">The creator of the company (nullable string).</param>
    /// <param name="CreatedDate">The creation date of the company (nullable DateTime).</param>
    /// <param name="UpdatedBy">The last updater of the company (nullable string).</param>
    /// <param name="UpdatedDate">The last update date of the company (nullable DateTime).</param>
    /// <returns>The result of the Company All List query.</returns>
    public sealed record GetCompanyAllListQuery : IRequest<Result<List<GetCompanyAllListDto>>>;
}