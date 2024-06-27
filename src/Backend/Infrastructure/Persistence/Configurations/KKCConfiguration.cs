using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class KKCConfiguration : IEntityTypeConfiguration<KKC>
    {
        public void Configure(EntityTypeBuilder<KKC> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(i => new { i.DeviceId, i.DeviceIp, i.DevicePort }).IsUnique();

            builder.Property(i => i.DeviceIp).HasMaxLength(32).IsRequired();
            builder.Property(i => i.DevicePort).IsRequired();
            builder.Property(i => i.Name).HasMaxLength(255).IsRequired();
            builder.Property(i => i.Description).HasMaxLength(255);
            builder.Property(i => i.Status).HasDefaultValue(0);
            builder.Property(x => x.UpdatedBy).HasMaxLength(100);
            builder.Property(x => x.UpdatedUserId).HasMaxLength(100);
            builder.Property(x => x.Guid).HasDefaultValueSql("NEWSEQUENTIALID()");
            builder.ToTable(nameof(KKC));
        }
    }
}
