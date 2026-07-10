using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;
using Shared.Domain.ValueObjects;
using Shared.Infrastructure.Persistence;
using System;

namespace OrderService.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : BaseEntityConfiguration<Order, Guid>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.ToTable("Orders");

            builder.Property(o => o.UserId)
               .IsRequired();

            builder.Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            // 1. Cấu hình Value Object Đơn Trị (Single-value Object) bằng HasConversion
            builder.Property(o => o.CustomerEmail)
                .HasConversion(
                    email => email.Value, // Convert từ Object -> Chuỗi lưu vào DB
                    value => EmailAddress.Create(value)) // Convert từ DB -> Object
                .HasColumnName("CustomerEmail")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(o => o.CustomerPhone)
                .HasConversion(
                    phone => phone.Value,
                    value => PhoneNumber.Create(value))
                .HasColumnName("CustomerPhone")
                .IsRequired()
                .HasMaxLength(20);

            // 2. Cấu hình Value Object Đa Trị (Multi-value Object) bằng OwnsOne

            // --- Address ---
            builder.OwnsOne(o => o.ShippingAddress, address =>
            {
                address.Property(a => a.Street).HasColumnName("ShippingStreet").HasMaxLength(200).IsRequired();
                address.Property(a => a.City).HasColumnName("ShippingCity").HasMaxLength(100).IsRequired();
                address.Property(a => a.State).HasColumnName("ShippingState").HasMaxLength(100).IsRequired();
                address.Property(a => a.Country).HasColumnName("ShippingCountry").HasMaxLength(100).IsRequired();
                address.Property(a => a.ZipCode).HasColumnName("ShippingZipCode").HasMaxLength(20).IsRequired();
            });

            // --- DateRange ---
            builder.OwnsOne(o => o.DeliveryWindow, window =>
            {
                window.Property(d => d.StartDate).HasColumnName("DeliveryStartDate");
                window.Property(d => d.EndDate).HasColumnName("DeliveryEndDate");
            });

            // --- Money (Gộp các cột Amount và Currency lại cho gọn DB) ---
            // --- Cấu hình lại toàn bộ các trường Money (Bỏ các dòng .Ignore đi) ---

            builder.OwnsOne(o => o.SubTotal, m => {
                m.Property(p => p.Amount).HasColumnName("SubTotal").HasColumnType("decimal(18,2)").IsRequired();
                m.Property(p => p.Currency).HasColumnName("SubTotalCurrency").HasMaxLength(3).IsRequired();
            });

            builder.OwnsOne(o => o.ShippingFee, m => {
                m.Property(p => p.Amount).HasColumnName("ShippingFee").HasColumnType("decimal(18,2)").IsRequired();
                m.Property(p => p.Currency).HasColumnName("ShippingFeeCurrency").HasMaxLength(3).IsRequired();
            });

            builder.OwnsOne(o => o.DiscountAmount, m => {
                m.Property(p => p.Amount).HasColumnName("DiscountAmount").HasColumnType("decimal(18,2)").IsRequired();
                m.Property(p => p.Currency).HasColumnName("DiscountAmountCurrency").HasMaxLength(3).IsRequired();
            });

            builder.OwnsOne(o => o.TaxAmount, m => {
                m.Property(p => p.Amount).HasColumnName("TaxAmount").HasColumnType("decimal(18,2)").IsRequired();
                m.Property(p => p.Currency).HasColumnName("TaxAmountCurrency").HasMaxLength(3).IsRequired();
            });

            builder.OwnsOne(o => o.TotalAmount, m => {
                m.Property(p => p.Amount).HasColumnName("TotalAmount").HasColumnType("decimal(18,2)").IsRequired();
                m.Property(p => p.Currency).HasColumnName("TotalAmountCurrency").HasMaxLength(3).IsRequired();
            });

            // Các thuộc tính cơ bản khác
            builder.Property(o => o.Note).HasMaxLength(1000);
            builder.Property(o => o.Status).HasConversion<int>().IsRequired();
            builder.Property(o => o.PaymentMethod).IsRequired().HasMaxLength(50);
            builder.Property(o => o.IsPaid).IsRequired();
            builder.Property(o => o.PaidAt);
            builder.Property(o => o.TrackingNumber).HasMaxLength(100);
            builder.Property(o => o.CancelReason).HasMaxLength(500);

            // 3. Cấu hình Encapsulation (Đóng gói Collection)
            // Báo cho EF Core biết hãy map data thẳng vào biến private `_orderItems`
            builder.Metadata
                .FindNavigation(nameof(Order.OrderItems))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}