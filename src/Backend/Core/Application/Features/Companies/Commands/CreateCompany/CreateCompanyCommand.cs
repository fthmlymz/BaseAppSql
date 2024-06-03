using MediatR;
using Shared;

namespace Application.Features.Companies.Commands.CreateCompany
{
    /// <summary>
    /// Represents the command to create a new company.
    /// </summary>
    /// <param name="Name">The name of the company.</param>
    /// <param name="Description">The description of the company.</param>
    /// <param name="TenantId">The ID of the tenant.</param>
    /// <param name="CreatedBy">The user who created the company.</param>
    /// <param name="CreatedUserId">The ID of the user who created the company.</param>
    /// <returns>A <see cref="Result{CreatedCompanyDto}"/> representing the result of the operation.</returns>
    public sealed record CreateCompanyCommand(string Name, string? Description, int TenantId, string? CreatedBy, string? CreatedUserId) : IRequest<Result<CreatedCompanyDto>>;
}
