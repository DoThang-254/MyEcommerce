using MediatR;
using ProductService.Domain.Exceptions;
using ProductService.Domain.Interfaces;
using Shared.Application.Common.Models;
using Shared.Application.Features.Handlers;
using Shared.Domain.Interfaces;

namespace ProductService.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, bool>  
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ProductNotFoundException($"Product with ID {request.Id} not found.");

        product.Update(request.Name, request.Description, request.Price, request.StockQuantity, request.ImageUrl);

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
