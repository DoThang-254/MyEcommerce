using Shared.Application.Features.Messaging;

namespace ProductService.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
