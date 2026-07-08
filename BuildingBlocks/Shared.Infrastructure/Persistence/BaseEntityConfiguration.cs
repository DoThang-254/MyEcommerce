using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Entities;

namespace Shared.Infrastructure.Persistence;

public abstract class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TId>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // 1. Cấu hình Khóa chính
        builder.HasKey(x => x.Id);

        // 2. Cấu hình các trường Audit
        builder.Property(x => x.CreatedDate)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(255); // Độ dài tối đa cho User ID hoặc Username

        builder.Property(x => x.LastModifiedDate)
            .IsRequired(false);

        builder.Property(x => x.LastModifiedBy)
            .HasMaxLength(255);
    }
}
