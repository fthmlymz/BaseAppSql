using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Companies.Commands.UpdateCompany
{
    /// <summary>
    /// Handles the CompanyUpdatedEvent and performs necessary actions.
    /// </summary>
    /// <typeparam name="Company">The type of the company.</typeparam>
    public class CompanyUpdatedEventHandler : INotificationHandler<CompanyUpdatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CompanyUpdatedEventHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyUpdatedEventHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work of type IUnitOfWork<Company>.</param>
        /// <param name="logger">The logger of type ILogger<CompanyUpdatedEventHandler>.</param>
        public CompanyUpdatedEventHandler(IUnitOfWork unitOfWork, ILogger<CompanyUpdatedEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Handles the CompanyUpdatedEvent and performs necessary actions.
        /// </summary>
        /// <param name="notification">The CompanyUpdatedEvent to handle of type CompanyUpdatedEvent.</param>
        /// <param name="cancellationToken">The cancellation token of type CancellationToken.</param>
        /// <returns>A Task that represents the asynchronous operation of type Task.</returns>
        public async Task Handle(CompanyUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var updatedCompany = notification.Company;

            //Bu alanda hangi tabloya ne bilgi eklemek istedigimizi belirliyoruz.
        }
    }
}
