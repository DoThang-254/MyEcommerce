using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;
using Shared.Infrastructure.Persistence;

public class ProductConfiguration : BaseEntityConfiguration<Product , Guid>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(m => m.Amount).HasColumnType("decimal(18,2)");
            price.Property(m => m.Currency).HasMaxLength(3);
        });
    }
}