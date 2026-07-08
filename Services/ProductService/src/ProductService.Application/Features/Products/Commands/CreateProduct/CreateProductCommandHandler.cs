using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using Shared.Application.Common.Interfaces;
using Shared.Application.Common.Models;
using Shared.Application.Features.Handlers;
using Shared.Domain.Interfaces;

namespace ProductService.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.StockQuantity,
            request.CategoryId,
            request.ImageUrl,
            userId
            );

        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id, 201);
    }
}
