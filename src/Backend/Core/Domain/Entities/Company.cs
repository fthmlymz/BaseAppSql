using Domain.Common;

namespace Domain.Entities
{
    public sealed class Company : BaseAuditableEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
