using MediatR;
using Shared;

namespace Application.Features.KKCs.Commands.CreateKKC
{
    public sealed record CreateKKCCommand(int TenantId, int? DeviceId, string? DeviceIp, int? DevicePort, string? Name, string? Description, int Interval, byte? Status, string? CreatedBy, string? CreatedUserId): IRequest<Result<CreatedKKCDto>>;
}
