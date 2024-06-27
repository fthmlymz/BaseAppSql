using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.KKCs.Commands.DeleteKKC
{
    public class KKCDeletedEvent : BaseEvent, INotification
    {
        public KKC Kcc { get; set; }

        public KKCDeletedEvent(KKC kcc)
        {
            Kcc = kcc;
        }
    }
}
