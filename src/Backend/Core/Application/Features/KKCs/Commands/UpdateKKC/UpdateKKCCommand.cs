using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Features.KKCs.Commands.UpdateKKC
{
    public sealed record UpdateKKCCommand : IRequest<Result<KKC>>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int? DeviceId { get; set; }
        public string? DeviceIp { get; set; }
        public int? DevicePort { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Interval { get; set; }
        public byte? Status { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedUserId { get; set; }
    }
    //public sealed record UpdateKKCCommand(int Id, int TenantId, int? DeviceId, string? DeviceIp, int? DevicePort, string? Name, string? Description, int Interval, byte? Status, string? UpdatedBy, string? UpdateUserId) : IRequest<Result<KKC>>;
}
