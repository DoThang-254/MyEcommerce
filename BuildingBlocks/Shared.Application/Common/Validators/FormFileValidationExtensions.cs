using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Shared.Application.Common.Validators
{
    public static class FormFileValidationExtensions
    {
        public static IRuleBuilderOptions<T, IFormFile> MustBeValidImage<T>(
            this IRuleBuilder<T, IFormFile> ruleBuilder,
            long maxFileSizeInBytes = 5 * 1024 * 1024)
        {
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp" };

            return ruleBuilder
                .NotNull().WithMessage("File is required.")
                .Must(f => f.Length > 0).WithMessage("File is empty.")
                .Must(f => f.Length <= maxFileSizeInBytes).WithMessage($"Maximum file size is {maxFileSizeInBytes / (1024 * 1024)}MB.")
                .Must(f => allowedMimeTypes.Contains(f.ContentType)).WithMessage("Unsupported file type.");
        }
    }
}
