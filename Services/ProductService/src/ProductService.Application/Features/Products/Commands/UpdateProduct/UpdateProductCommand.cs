using Shared.Application.Features.Messaging;

namespace ProductService.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl
) : ICommand<bool>;
