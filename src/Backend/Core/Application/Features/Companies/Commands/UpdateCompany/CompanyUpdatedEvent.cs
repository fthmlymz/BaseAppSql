using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Companies.Commands.UpdateCompany
{
    /// <summary>
    /// Represents an event when a company is updated.
    /// </summary>
    /// <param name="Company">The company object.</param>
    public class CompanyUpdatedEvent : BaseEvent, INotification
    {
        public Company Company { get; }
        public CompanyUpdatedEvent(Company company)
        {
            Company = company;
        }
    }
}
