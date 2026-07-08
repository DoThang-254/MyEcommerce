using AutoMapper;
using ProductService.Domain.Exceptions;
using ProductService.Domain.Interfaces;
using Shared.Application.Common.Models;
using Shared.Application.Features.Handlers;

namespace ProductService.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ProductNotFoundException($"Product with ID {request.Id} not found.");

        var dto = _mapper.Map<ProductDto>(product);
        return Result<ProductDto>.Success(dto);
    }
}
