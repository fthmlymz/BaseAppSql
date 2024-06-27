using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.KKCs.Commands.DeleteKKC
{
    internal class DeleteKKCCommandHandler : IRequestHandler<DeleteKCCCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteKKCCommandHandler> _logger;

        public DeleteKKCCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteKKCCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteKCCCommand request, CancellationToken cancellationToken)
        {
            var kcc = await _unitOfWork.Repository<KKC>().GetByIdAsync(request.Id);
            if (kcc == null)
            {
                _logger.LogWarning($"Kcc not found {request.Id}");
                throw new NotFoundExceptionCustom($"{request.Id} numaralı kcc kaydı bulunamadı.");
            }

            await _unitOfWork.Repository<KKC>().DeleteAsync(kcc);
            kcc.AddDomainEvent(new KKCDeletedEvent(kcc));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Kcc deleted {request.Id}");
            return true;
        }
    }
}
