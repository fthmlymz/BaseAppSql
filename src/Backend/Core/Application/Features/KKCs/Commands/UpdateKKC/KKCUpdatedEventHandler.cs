using Application.Interfaces.Repositories;
using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.KKCs.Commands.UpdateKKC
{
    public class KKCUpdatedEventHandler : INotificationHandler<KKCUpdatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<KKCUpdatedEventHandler> _logger;
        private readonly ICapPublisher _capPublisher;
        public KKCUpdatedEventHandler(IUnitOfWork unitOfWork, ILogger<KKCUpdatedEventHandler> logger, ICapPublisher capPublisher)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _capPublisher = capPublisher;
        }

        public async Task Handle(KKCUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var updatedKKC = notification.Kkc;

            #region If you want to throw an event after recording
            if (_capPublisher != null)
            {
                await _capPublisher.PublishAsync("kkc.updated", notification);
            }
            #endregion
        }
    }
}
