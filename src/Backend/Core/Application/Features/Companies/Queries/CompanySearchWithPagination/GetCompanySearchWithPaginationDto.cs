namespace Application.Features.Companies.Queries.CompanySearchWithPagination
{
    /// <summary>
    /// Represents the DTO for the Company search with pagination.
    /// </summary>
    /// <param name="Name">The name of the company (nullable string).</param>
    /// <param name="Description">The description of the company (nullable string).</param>
    /// <param name="TenantId">The tenantId of the company (int).</param>
    /// <param name="Guid">The guid of the company (nullable string).</param>
    /// <param name="CreatedBy">The creator of the company (nullable string).</param>
    /// <param name="CreatedDate">The creation date of the company (nullable DateTime).</param>
    /// <param name="UpdatedBy">The last updater of the company (nullable string).</param>
    /// <param name="UpdatedDate">The last update date of the company (nullable DateTime).</param>
    public record GetCompanySearchWithPaginationDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int TenantId { get; set; }
        public Guid Guid { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
