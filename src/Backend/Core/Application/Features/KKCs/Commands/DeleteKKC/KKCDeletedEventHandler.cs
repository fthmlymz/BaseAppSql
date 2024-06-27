using Application.Interfaces.Repositories;
using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.KKCs.Commands.DeleteKKC
{
    public class KKCDeletedEventHandler : INotificationHandler<KKCDeletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<KKCDeletedEventHandler> _logger;
        private readonly ICapPublisher _capPublisher;

        public KKCDeletedEventHandler(IUnitOfWork unitOfWork, ILogger<KKCDeletedEventHandler> logger, ICapPublisher capPublisher)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _capPublisher = capPublisher;
        }

        public async Task Handle(KKCDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedKCC = notification.Kcc;

            #region If you want to throw an event after recording
            if (_capPublisher != null)
            {
                await _capPublisher.PublishAsync("kkc.deleted", notification);
            }
            #endregion
        }
    }
}
