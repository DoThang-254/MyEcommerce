using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            // Khóa chính (vì không còn dùng BaseEntity)
            builder.HasKey(oi => oi.OrderItemId);

            builder.Property(oi => oi.OrderId)
                .IsRequired();

            builder.Property(oi => oi.ProductId)
                .IsRequired();

            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(oi => oi.ProductImage)
                .HasMaxLength(500);

            // Cấu hình Value Object: Money (UnitPrice)
            builder.OwnsOne(oi => oi.UnitPrice, price =>
            {
                price.Property(m => m.Amount)
                     .HasColumnName("UnitPrice")
                     .HasColumnType("decimal(18,2)")
                     .IsRequired();
                
                price.Property(m => m.Currency)
                     .HasColumnName("UnitPriceCurrency")
                     .HasMaxLength(3)
                     .IsRequired();
            });

            // Cấu hình Value Object: Money (TotalPrice)
            builder.OwnsOne(oi => oi.TotalPrice, price =>
            {
                price.Property(m => m.Amount)
                     .HasColumnName("TotalPrice")
                     .HasColumnType("decimal(18,2)")
                     .IsRequired();
                
                price.Property(m => m.Currency)
                     .HasColumnName("TotalPriceCurrency")
                     .HasMaxLength(3)
                     .IsRequired();
            });

            // Quantity
            builder.Property(oi => oi.Quantity)
                .IsRequired();
        }
    }
}