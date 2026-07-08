using MediatR;
using Shared.Application.Features.Messaging;

namespace ProductService.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId,
    string? ImageUrl 
) : ICommand<Guid>;
