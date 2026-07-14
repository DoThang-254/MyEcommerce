using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProductService.Application.Features.Products.Commands.UploadProductImage
{
    public record UploadProductImageCommand(IFormFile File, Guid ProductId) : IRequest<string>;
}
