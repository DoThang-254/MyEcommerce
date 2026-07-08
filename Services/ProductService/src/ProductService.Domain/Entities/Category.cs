using Shared.Domain.Entities;

namespace ProductService.Domain.Entities;

public class Category : BaseEntity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Category() { }

    public static Category Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
        };
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
