namespace Application.Features.Companies.Commands.CreateCompany
{
    public record CreatedCompanyDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int TenantId { get; set; }
        public Guid Guid { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set;}
        public string? UpdatedBy { get; set; }
        public string? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
