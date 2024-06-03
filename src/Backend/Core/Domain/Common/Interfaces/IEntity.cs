namespace Domain.Common.Interfaces
{
    public interface IEntity
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Guid Guid { get; set; }
    }
}