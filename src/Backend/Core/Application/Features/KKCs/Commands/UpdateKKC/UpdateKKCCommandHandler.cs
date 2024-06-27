using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.KKCs.Commands.UpdateKKC
{
    public class UpdateKKCCommandHandler : IRequestHandler<UpdateKKCCommand, Result<KKC>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateKKCCommandHandler> _logger;

        public UpdateKKCCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateKKCCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<KKC>> Handle(UpdateKKCCommand request, CancellationToken cancellationToken)
        {
            #region KKC Id Check
            var kkcResult = await _unitOfWork.Repository<KKC>().GetByIdAsync(request.Id);
            if (kkcResult == null)
            {
                _logger.LogWarning($"KKC with id {request.Id} not found.");
                throw new NotFoundExceptionCustom($"{request.Name} isimli kkc kaydı bulunamadı.");
            }
            #endregion

            #region Check if KKC already exists
            var checkDeviceId = await _unitOfWork.Repository<KKC>().AnyAsync(x => x.DeviceId == request.DeviceId);
            if (checkDeviceId)
            {
                _logger.LogInformation($"KKC Already Exists: {request.DeviceId} - {request.Name}");
                throw new BadRequestExceptionCustom($"{request.Name} isimli cihaz id daha önce eklenmiş.");
            }
            #endregion

            #region KKC Update
            foreach (var propertyInfo in request.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(request);
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    var propertyName = propertyInfo.Name;
                    var productProperty = kkcResult.GetType().GetProperty(propertyName);
                    productProperty.SetValue(kkcResult, value);
                }
            }

            await _unitOfWork.Repository<KKC>().UpdateAsync(kkcResult);
            
            // Event
            kkcResult.AddDomainEvent(new KKCUpdatedEvent(kkcResult));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            #endregion

            _logger.LogInformation($"KKC with id {request.Id} updated.");

            return await Result<KKC>.SuccessAsync(kkcResult);
        }
    }
}
