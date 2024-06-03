using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Companies.Commands.DeleteCompany
{
    /// <summary>
    /// Handles the CompanyDeletedEvent and performs necessary actions.
    /// </summary>
    /// <typeparam name="Company">The type of the company.</typeparam>
    public class CompanyDeletedEventHandler : INotificationHandler<CompanyDeletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CompanyDeletedEventHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyDeletedEventHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        public CompanyDeletedEventHandler(IUnitOfWork unitOfWork, ILogger<CompanyDeletedEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Handles the CompanyDeletedEvent and performs necessary actions.
        /// </summary>
        /// <param name="notification">The CompanyDeletedEvent to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task Handle(CompanyDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedCompany = notification.Company;

            //Bu alanda hangi tabloya ne bilgi eklemek istedigimizi belirliyoruz.
        }
    }
}
