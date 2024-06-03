namespace Application.Features.Companies.Queries.CompanyPaginatedAllList
{
    /// <summary>
    /// Represents a DTO for a company with paginated results.
    /// </summary>
    /// <param name="Id">The ID of the company (int).</param>
    /// <param name="Name">The name of the company (nullable string).</param>
    /// <param name="Description">The description of the company (nullable string).</param>
    /// <param name="TenantId">The tenantId of the company (int).</param>
    /// <param name="Guid">The GUID of the company (Guid).</param>
    /// <param name="CreatedBy">The creator of the company (nullable string).</param>
    /// <param name="CreatedDate">The creation date of the company (nullable DateTime).</param>
    /// <param name="UpdatedBy">The last updater of the company (nullable string).</param>
    /// <param name="UpdatedDate">The last update date of the company (nullable DateTime).</param>
    public record GetCompanyPaginatedAllListDto
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
