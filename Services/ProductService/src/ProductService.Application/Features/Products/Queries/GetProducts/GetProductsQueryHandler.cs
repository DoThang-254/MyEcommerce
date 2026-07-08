using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Interfaces;
using Shared.Application.Common.Models;
using Shared.Application.Features.Handlers;

namespace ProductService.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IQueryHandlerPagedResult<GetProductsQuery, ProductListDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsQueryHandler> _logger;

    public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductsQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PagedResult<ProductListDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching products from database...");

            var spec = new ProductsByCategorySpec(request);

            var products = await _productRepository.ListAsync(spec, cancellationToken);
            var totalRecords = await _productRepository.CountAsync(spec, cancellationToken);

            if (products == null || !products.Any())
            {
                _logger.LogWarning("No products found.");
                return Result<PagedResult<ProductListDto>>.Success(new PagedResult<ProductListDto>(Enumerable.Empty<ProductListDto>().ToList(), 0, request.PageIndex, request.PageSize));
            }

            var dtos = _mapper.Map<IReadOnlyList<ProductListDto>>(products);
            var pagedResult = new PagedResult<ProductListDto>(
                        items: dtos,
                        totalCount: totalRecords,
                        pageNumber: request.PageIndex,
                        pageSize: request.PageSize
                    );

            return Result<PagedResult<ProductListDto>>.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching products.");
            return Result<PagedResult<ProductListDto>>.Failure("An error occurred while processing your request.");
        }


    }
}
