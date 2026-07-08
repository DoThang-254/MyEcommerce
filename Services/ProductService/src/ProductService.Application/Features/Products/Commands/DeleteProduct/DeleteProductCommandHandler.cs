using MediatR;
using ProductService.Domain.Exceptions;
using ProductService.Domain.Interfaces;
using Shared.Application.Common.Models;
using Shared.Application.Features.Handlers;
using Shared.Domain.Interfaces;

namespace ProductService.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ProductNotFoundException($"Product with ID {request.Id} not found.");

        _productRepository.Delete(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
