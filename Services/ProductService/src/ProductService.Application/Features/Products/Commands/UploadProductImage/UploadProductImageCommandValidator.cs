using FluentValidation;
using Shared.Application.Common.Validators;

namespace ProductService.Application.Features.Products.Commands.UploadProductImage
{
        public class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommand>
        {
            public UploadProductImageCommandValidator()
            {
                RuleFor(x => x.ProductId)
                    .NotEmpty().WithMessage("ProductId is required.");

                RuleFor(x => x.File)
                    .MustBeValidImage(5 * 1024 * 1024); // Tối đa 5MB cho ảnh sản phẩm
            }
        }
}