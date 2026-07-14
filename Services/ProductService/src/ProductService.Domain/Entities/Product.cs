using ProductService.Domain.Enums;
using ProductService.Domain.Events;
using Shared.Domain.Entities;
using Shared.Domain.ValueObjects;

namespace ProductService.Domain.Entities;

public class Product : AggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Money Price { get; private set; }
    public int StockQuantity { get; private set; }
    public string? ImageUrl { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public ProductStatus Status { get; private set; }


    private Product() { } // EF Core constructor

    public static Product Create(string name, string? description, decimal price, int stockQuantity, Guid categoryId, string? imageUrl = null, string? userId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = new Money(price),
            StockQuantity = stockQuantity,
            CategoryId = categoryId,
            ImageUrl = imageUrl,
            Status = ProductStatus.Active,
            CreatedBy = userId
        };

        product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.Name));

        return product;
    }

    public void Update(string name, string? description, decimal price, int stockQuantity, string? imageUrl)
    {
        Name = name;
        Description = description;
        Price = new Money(price);
        StockQuantity = stockQuantity;
    }

    public void UpdateImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty.", nameof(imageUrl));

        ImageUrl = imageUrl;
    }

    public void Deactivate()
    {
        Status = ProductStatus.Inactive;
    }
}
