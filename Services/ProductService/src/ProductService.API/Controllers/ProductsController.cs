using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Products.Commands.CreateProduct;
using ProductService.Application.Features.Products.Commands.DeleteProduct;
using ProductService.Application.Features.Products.Commands.UpdateProduct;
using ProductService.Application.Features.Products.Commands.UploadProductImage;
using ProductService.Application.Features.Products.Queries.GetProductById;
using ProductService.Application.Features.Products.Queries.GetProducts;
using Shared.API.Controllers;
using Shared.Application.Common.Interfaces;

namespace ProductService.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IFileStorageService _fileStorageService;

        public ProductsController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetProductsQuery query,
                            CancellationToken cancellationToken)
        {
            // Mediator sẽ gửi query chứa đầy đủ PageIndex, PageSize, CategoryId...
            var result = await Mediator.Send(query, cancellationToken);

            // Trả về kết quả dựa trên Result pattern bạn đang dùng
            return result.IsSuccess ? Ok(result) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Get a product by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetProductByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid id, IFormFile file , CancellationToken cancellationToken)
        {
            // Khởi tạo command
            var command = new UploadProductImageCommand(file, id);

            // Gửi command cho MediatR xử lý
            var imageUrl = await Mediator.Send(command, cancellationToken);

            return Ok(new { Url = imageUrl });
        }


        /// <summary>
        /// Update an existing product
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest("Route ID and body ID must match.");

            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return Ok(result);
        }
    }
}
