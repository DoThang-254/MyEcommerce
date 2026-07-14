using MediatR;
using ProductService.Domain.Interfaces;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProductService.Application.Features.Products.Commands.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommand, string>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UploadProductImageCommandHandler> _logger;

        public UploadProductImageCommandHandler(
            IFileStorageService fileStorageService,
            IProductRepository productRepo,
            IUnitOfWork unitOfWork,
            ILogger<UploadProductImageCommandHandler> logger)
        {
            _fileStorageService = fileStorageService;
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<string> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("1. Starting find product...");
            var product = await _productRepo.GetByIdAsync(request.ProductId, cancellationToken);

            if (product == null)
                throw new Exception($"Product with id {request.ProductId} not found.");

            var oldImageUrl = product.ImageUrl;

            _logger.LogInformation("2. Opening stream from file...");
            using var stream = request.File.OpenReadStream();
            var folderName = $"products/{request.ProductId}";

            _logger.LogInformation("3. Starting to call Cloudinary for upload...");
            var newImageUrl = await _fileStorageService.UploadFileAsync(
                stream,
                request.File.FileName,
                folderName,
                cancellationToken);

            _logger.LogInformation("4. Got URL from Cloudinary, preparing to save to DB...");
            product.UpdateImage(newImageUrl);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("5. Save completed.");
            if (!string.IsNullOrWhiteSpace(oldImageUrl))
            {
                try
                {
                    _logger.LogInformation("5.1. Starting to delete old image from Cloudinary...");
                    await _fileStorageService.DeleteFileAsync(oldImageUrl, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while deleting old image.");
                }
            }

            _logger.LogInformation("6. Finished Handle!");
            return newImageUrl;
        }
    }
}