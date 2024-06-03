using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Features.Companies.Commands.UpdateCompany
{
    /// <summary>
    /// Represents a command to update a company.
    /// </summary>
    /// <param name="Id">The ID of the company to update.</param>
    /// <param name="TenantId">The ID of the tenant associated with the company.</param>
    /// <param name="Name">The name of the company.</param>
    /// <param name="Description">The description of the company.</param>
    /// <param name="UpdatedBy">The user who updated the company.</param>
    /// <param name="UpdatedUserId">The ID of the user who updated the company.</param>
    /// <returns>A result containing the updated company.</returns>
    public sealed record UpdateCompanyCommand(int Id, int TenantId, string? Name, string? Description, string UpdatedBy, string UpdatedUserId): IRequest<Result<Company>>;
}
