using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;

namespace ProductService.Infrastructure.Data.Configurations;

public static class ProductSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var categoryId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var productId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479");

        // Seed Category (required by Product)
        modelBuilder.Entity<Category>().HasData(new
        {
            Id = categoryId,
            Name = "Default",
            Description = "Default category for seeded products",
            CreatedDate = DateTime.Parse("2026-04-01"),
            CreatedBy = "Seed"
        });

        // Seed Product
        modelBuilder.Entity<Product>().HasData(new
        {
            Id = productId,
            Name = "Bản tin công nghệ FU-News",
            CategoryId = categoryId,
            CreatedDate = DateTime.Parse("2026-04-01"),
            CreatedBy = "Thắng",
            Status = ProductStatus.Active,
            StockQuantity = 100
        });

        // Seed Price (Owned Type)
        modelBuilder.Entity<Product>().OwnsOne(p => p.Price).HasData(new
        {
            ProductId = productId,
            Amount = 0.00m,
            Currency = "VND"
        });
    }
}
