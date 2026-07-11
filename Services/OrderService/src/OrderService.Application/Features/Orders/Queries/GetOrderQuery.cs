using OrderService.Domain.Enums;
using Shared.Application.Features.Messaging;

namespace OrderService.Application.Features.Orders.Queries
{
    public record GetOrderQuery : IQueryPagedResult<OrderDto>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Các bộ lọc (Filters) dành riêng cho Order
        public Guid? UserId { get; set; }
        public OrderStatus? Status { get; set; } 
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? SearchTerm { get; set; } // Tìm theo mã đơn hoặc tên khách hàng
    }
}
