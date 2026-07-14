using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Orders.Commands.CreateOrder;
using OrderService.Application.Features.Orders.Commands.UpdateOrder;
using OrderService.Application.Features.Orders.Queries;
using Shared.API.Controllers;
using Shared.Application.Common.Models;

namespace OrderService.API.Controllers
{
    public class OrderController : BaseController
    {
        /// <summary>
        /// Lấy danh sách đơn hàng có phân trang và bộ lọc (Status, UserId, Date, Search)
        /// </summary>
        /// <param name="query">Bộ lọc và thông tin phân trang gửi từ URL Query String</param>
        /// <param name="cancellationToken">Token hủy request từ phía Client</param>
        /// <returns>Danh sách đơn hàng dạng PagedResult</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<OrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrders([FromQuery] GetOrderQuery query, CancellationToken cancellationToken)
        {
            // Gửi Query sang Handler thông qua Mediator (thuộc tính Mediator thường được định nghĩa sẵn trong BaseController)
            var result = await Mediator.Send(query, cancellationToken);

            // Kiểm tra kết quả trả về từ tầng Application
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }

        /// <summary>
        /// Tạo mới một đơn hàng (Checkout)
        /// </summary>
        /// <param name="command">Thông tin đơn hàng và danh sách sản phẩm từ Body</param>
        /// <param name="cancellationToken">Token hủy request</param>
        /// <returns>ID của đơn hàng vừa tạo (Guid)</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
        {
            // Gửi Command sang Handler xử lý thông qua MediatR
            var result = await Mediator.Send(command, cancellationToken);

            // Nếu tầng Application hoặc Domain báo lỗi (ví dụ: Sai định dạng Email, Phone, trống địa chỉ...)
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            // Theo chuẩn RESTful, tạo mới thành công nên trả về trạng thái 201 Created kèm ID vật thể
            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        [HttpPut("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelOrder(Guid id, [FromBody] string reason, CancellationToken cancellationToken)
        {
            var command = new CancelOrderCommand(id, reason);
            var result = await Mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage); // Hiển thị lỗi nghiệp vụ (vd: "Đang giao không được hủy")
            }

            return Ok(new { Message = "Hủy đơn hàng thành công" });
        }

        /// <summary>
        /// Cập nhật địa chỉ giao hàng của đơn hàng
        /// </summary>
        /// <param name="id">ID của đơn hàng (lấy từ URL)</param>
        /// <param name="command">Thông tin địa chỉ mới (lấy từ JSON Body)</param>
        [HttpPut("{id}/address")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderAddress(Guid id, [FromBody] UpdateOrderAddressCommand requestBody, CancellationToken cancellationToken)
        {
            // Kết hợp ID từ URL và dữ liệu Address từ Body để tạo thành Command hoàn chỉnh
            var command = new UpdateOrderAddressCommand(
                id,
                requestBody.Street,
                requestBody.City,
                requestBody.State,
                requestBody.Country,
                requestBody.ZipCode
            );

            var result = await Mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                // Trả về HTTP 400 kèm câu thông báo lỗi từ tầng Domain (VD: "Không thể đổi địa chỉ khi đơn hàng đang được giao.")
                return BadRequest(result.ErrorMessage);
            }

            return Ok(new { Message = "Cập nhật địa chỉ giao hàng thành công." });
        }
    }
}
