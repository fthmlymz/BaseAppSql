using Domain.Common;
using Domain.Entities;

namespace Application.Features.KKCs.Commands.CreateKKC
{
    public class KKCCreatedEvent : BaseEvent
    {
        public KKC Kkc { get; set; }

        public KKCCreatedEvent(KKC kkc)
        {
            Kkc = kkc;
        }
    }
}
