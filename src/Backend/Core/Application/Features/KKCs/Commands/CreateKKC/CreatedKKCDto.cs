namespace Application.Features.KKCs.Commands.CreateKKC
{
    public record CreatedKKCDto
    {
        public int? Id { get; set; }
        public int? DeviceId { get; set; }
        public string? DeviceIp { get; set; }
        public int? DevicePort { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte? Status { get; set; }
        public int? Interval { get; set; }
        public int? TenantId { get; set; }
    }
}
