using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Companies.Commands.DeleteCompany
{

    /// <summary>
    /// Represents an event when a company is deleted.
    /// </summary>
    /// <typeparam name="Company">The type of the company.</typeparam>
    public class CompanyDeletedEvent : BaseEvent, INotification
    {
        public Company Company { get; set; }
        public CompanyDeletedEvent(Company company)
        {
            Company = company;
        }
    }
}
