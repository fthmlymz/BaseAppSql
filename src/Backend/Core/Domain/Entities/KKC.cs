using Domain.Common;

namespace Domain.Entities
{
    public sealed class KKC: BaseAuditableEntity
    {
        public int DeviceId { get; set; }
        public string DeviceIp { get; set; }
        public int DevicePort { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Interval { get; set; }
        public byte Status { get; set; }
    }
}
