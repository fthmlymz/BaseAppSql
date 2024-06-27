using Application.Interfaces.Repositories;
using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.KKCs.Commands.CreateKKC
{
    public class KKCCreatedEventHandler : INotificationHandler<KKCCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<KKCCreatedEventHandler> _logger;
        private readonly ICapPublisher _capPublisher;

        public KKCCreatedEventHandler(IUnitOfWork unitOfWork, ILogger<KKCCreatedEventHandler> logger, ICapPublisher capPublisher)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _capPublisher = capPublisher;
        }

        public async Task Handle(KKCCreatedEvent notification, CancellationToken cancellationToken)
        {
            #region If you want to throw an event after recording
            if (_capPublisher != null)
            {
                await _capPublisher.PublishAsync("kkc.created", notification);
            }
            #endregion
        }
    }
}
