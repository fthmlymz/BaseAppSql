using Domain.Common;
using Domain.Entities;

namespace Application.Features.KKCs.Commands.UpdateKKC
{
    public class KKCUpdatedEvent : BaseEvent
    {
        public KKC Kkc { get; set; }
        public KKCUpdatedEvent(KKC kkc)
        {
            Kkc = kkc;
        }
    }
}
