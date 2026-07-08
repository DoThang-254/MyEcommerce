using Shared.Application.Features.Messaging;

namespace ProductService.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<bool>;
