using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Companies.Commands.UpdateCompany
{
    /// <summary>
    /// Handles the update company command asynchronously.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <typeparam name="TCompany">The type of the company.</typeparam>
    internal class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Result<Company>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCompanyCommandHandler> _logger;
        private readonly IEasyCacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCompanyCommandHandler{TRequest, TResponse, TCompany}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work of type IUnitOfWork<TCompany>.</param>
        /// <param name="logger">The logger of type ILogger<UpdateCompanyCommandHandler<TRequest, TResponse, TCompany>>.</param>
        /// <param name="cacheService">The cache service of type IEasyCacheService.</param>
        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCompanyCommandHandler> logger, IEasyCacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Handles the update company command asynchronously.
        /// </summary>
        /// <param name="request">The update company command request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated company as a Result.</returns>
        public async Task<Result<Company>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Repository<Company>().GetByIdAsync(request.Id);
            if (company == null)
            {
                _logger.LogInformation($"Company not found {request.Id} - {request.Name}");
                throw new NotFoundExceptionCustom($"{request.Name} isimli şirket bulunamadı.");
            }

            // Update company properties
            foreach (var propertyInfo in request.GetType().GetProperties())
            {
                var propertyValue = propertyInfo.GetValue(request);
                if (propertyValue != null && !string.IsNullOrWhiteSpace(propertyValue.ToString()))
                {
                    var propertyName = propertyInfo.Name;
                    var companyProperty = company.GetType().GetProperty(propertyName);
                    companyProperty.SetValue(company, propertyValue);
                }
            }

            await _unitOfWork.Repository<Company>().UpdateAsync(company);
            company.AddDomainEvent(new CompanyUpdatedEvent(company));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"{request.Id} - {request.Name} isimli şirket güncellendi.");

            return await Result<Company>.SuccessAsync(company);
        }
    }
}
