using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.KKCs.Commands.CreateKKC
{
    public sealed class CreateKKCCommandHandler : IRequestHandler<CreateKKCCommand, Result<CreatedKKCDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateKKCCommandHandler> _logger;

        public CreateKKCCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateKKCCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<CreatedKKCDto>> Handle(CreateKKCCommand request, CancellationToken cancellationToken)
        {
            #region Check if KKC already exists
            var check = await _unitOfWork.Repository<KKC>().AnyAsync(x => x.DeviceId == request.DeviceId);
            if (check)
            {
                _logger.LogInformation($"KKC Already Exists: {request.DeviceId} - {request.Name}");
                throw new BadRequestExceptionCustom($"{request.Name} isimli cihaz id daha önce eklenmiş.");
            }
            #endregion

            #region Create KKC
            var kkc = request.Adapt<KKC>();
            await _unitOfWork.Repository<KKC>().AddAsync(kkc);

            // Event
            kkc.AddDomainEvent(new KKCCreatedEvent(kkc));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"KKC Created: {kkc.DeviceId} - {kkc.Name}");
            #endregion

            var createdKKCDto = kkc.Adapt<CreatedKKCDto>();

            return await Result<CreatedKKCDto>.SuccessAsync(createdKKCDto);
        }
    }
}