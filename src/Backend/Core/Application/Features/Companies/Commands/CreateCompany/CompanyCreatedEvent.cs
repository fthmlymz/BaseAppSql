using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Companies.Commands.CreateCompany
{
    /// <summary>
    /// Represents the event when a company is created.
    /// </summary>
    /// <typeparam name="Company">The type of the company.</typeparam>
    public class CompanyCreatedEvent: BaseEvent, INotification
    {
        public Company Company { get; set; }

        public CompanyCreatedEvent(Company company)
        {
            Company = company;
        }
    }
}
