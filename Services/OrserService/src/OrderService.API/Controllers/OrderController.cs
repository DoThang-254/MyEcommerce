using Microsoft.AspNetCore.Mvc;
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
    }
}
