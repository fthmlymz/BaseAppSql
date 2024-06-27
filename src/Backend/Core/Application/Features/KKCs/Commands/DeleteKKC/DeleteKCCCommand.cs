using MediatR;

namespace Application.Features.KKCs.Commands.DeleteKKC
{
    public sealed record DeleteKCCCommand(int Id) : IRequest<bool>;
}
