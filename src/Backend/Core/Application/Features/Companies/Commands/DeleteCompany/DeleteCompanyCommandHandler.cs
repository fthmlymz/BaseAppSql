using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Companies.Commands.DeleteCompany
{
    internal class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCompanyCommandHandler> _logger;
        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCompanyCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Handles a DeleteCompanyCommand by deleting the corresponding Company entity and returning true if successful.
        /// Throws a NotFoundExceptionCustom if the Company entity is not found.
        /// </summary>
        /// <param name="request">The DeleteCompanyCommand to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Repository<Company>().GetByIdAsync(request.Id);
            if (company == null)
            {
                _logger.LogWarning($"Company not found {request.Id}");
                throw new NotFoundExceptionCustom($"{request.Id} numaralı şirket bulunamadı.");
            }

            await _unitOfWork.Repository<Company>().DeleteAsync(company);
            company.AddDomainEvent(new CompanyDeletedEvent(company));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Company with ID {request.Id} deleted successfully");

            return true;
        }
    }
}
