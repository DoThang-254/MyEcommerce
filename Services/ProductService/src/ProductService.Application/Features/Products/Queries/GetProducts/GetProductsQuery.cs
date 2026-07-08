using Shared.Application.Features.Messaging;

namespace ProductService.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery : IQueryPagedResult<ProductListDto>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public Guid? CategoryId { get; set; } = null;

    public string? SearchTerm { get; set; } = null;
}
