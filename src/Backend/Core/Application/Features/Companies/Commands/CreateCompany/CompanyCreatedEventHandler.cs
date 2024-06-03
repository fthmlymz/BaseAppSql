using MediatR;

namespace Application.Features.Companies.Commands.CreateCompany
{
    /// <summary>
    /// Handles the CompanyCreatedEvent notification.
    /// </summary>
    /// <param name="notification">The CompanyCreatedEvent notification to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public class CompanyCreatedEventHandler : INotificationHandler<CompanyCreatedEvent>
    {
        public async Task Handle(CompanyCreatedEvent notification, CancellationToken cancellationToken)
        {
            //Console.WriteLine($"Company {notification.Company.Name} is successfully created");
        }
    }
}
