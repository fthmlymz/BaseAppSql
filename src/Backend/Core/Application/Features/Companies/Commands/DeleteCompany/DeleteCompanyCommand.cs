using MediatR;

namespace Application.Features.Companies.Commands.DeleteCompany
{
    /// <summary>
    /// Represents a command to delete a company.
    /// </summary>
    /// <param name="Id">The ID of the company to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    public sealed record DeleteCompanyCommand(int Id) : IRequest<bool>;
}
