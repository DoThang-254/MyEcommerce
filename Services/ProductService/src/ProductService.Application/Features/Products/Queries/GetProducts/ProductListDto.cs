namespace ProductService.Application.Features.Products.Queries.GetProducts;

public record ProductListDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int StockQuantity { get; init; }
    public string? ImageUrl { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}
