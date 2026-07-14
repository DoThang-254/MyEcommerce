using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.Infrastructure.Persistence
{
    public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ServiceName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.UserId).HasMaxLength(100);
            builder.Property(x => x.CorrelationId).HasMaxLength(100);
            builder.Property(x => x.IpAddress).HasMaxLength(64);
            builder.Property(x => x.EntityName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.EntityId).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Action).HasMaxLength(20).IsRequired();

            builder.HasIndex(x => new { x.EntityName, x.EntityId });
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.OccurredAtUtc);
        }
    }
}
