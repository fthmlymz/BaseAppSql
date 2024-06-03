using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Companies.Commands.CreateCompany
{
    public sealed class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Result<CreatedCompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCompanyCommandHandler> _logger;

        public CreateCompanyCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCompanyCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Handles the CreateCompanyCommand and returns the result of the operation.
        /// </summary>
        /// <param name="request">The CreateCompanyCommand to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a Result with a CreatedCompanyDto, or an error message.</returns>
        public async Task<Result<CreatedCompanyDto>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = request.Adapt<Company>();
            await _unitOfWork.Repository<Company>().AddAsync(company);

            company.AddDomainEvent(new CompanyCreatedEvent(company));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var saveChangesTask = _unitOfWork.SaveChangesAsync(cancellationToken);

            #region Logging
            await (saveChangesTask.IsCanceled || saveChangesTask.IsFaulted ? Task.CompletedTask : saveChangesTask);


            if (saveChangesTask.IsCanceled)
            {
                _logger.LogInformation("Save changes operation was cancelled.");
                throw new BadRequestExceptionCustom($"{request.Name} isimli kayıt eklenemedi !");
            }
            else if (saveChangesTask.IsCompleted)
            {
                _logger.LogInformation($"Company {company.Id} is successfully created.");
            }
            else if (saveChangesTask.IsFaulted)
            {
                _logger.LogError($"An error occurred while saving changes. {saveChangesTask.Exception}");
            }
            #endregion

            var createdcompanyDto = company.Adapt<CreatedCompanyDto>();

            _logger.LogInformation($"{createdcompanyDto.Id} - {createdcompanyDto.Name} Company created successfully");

            return await Result<CreatedCompanyDto>.SuccessAsync(createdcompanyDto, "Company created");
        }
    }
}
