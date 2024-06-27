namespace Application.Features.KKCs.Queries.KKCSearchWithPagination
{
    public record GetKKCSearchWithPaginationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DeviceId { get; set; }
        public string DeviceIp { get; set; }
        public int DevicePort { get; set; }
        public int TenantId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
