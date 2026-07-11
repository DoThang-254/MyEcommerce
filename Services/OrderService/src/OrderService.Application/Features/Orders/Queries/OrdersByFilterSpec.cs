using Ardalis.Specification;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Features.Orders.Queries
{
    public class OrdersByFilterSpec : Specification<Order>
    {
        public OrdersByFilterSpec(GetOrderQuery request)
        {
            // 1. Lọc theo UserId (Ví dụ: User xem lịch sử mua hàng của chính mình)
            if (request.UserId.HasValue)
            {
                Query.Where(o => o.UserId == request.UserId.Value);
            }

            // 2. Lọc theo trạng thái đơn hàng
            if (request.Status.HasValue)
            {
                Query.Where(o => o.Status == request.Status.Value);
            }

            // 3. Lọc theo ngày tạo
            if (request.FromDate.HasValue)
            {
                Query.Where(o => o.CreatedDate >= request.FromDate.Value);
            }
            if (request.ToDate.HasValue)
            {
                Query.Where(o => o.CreatedDate <= request.ToDate.Value);
            }

            // 4. Lọc theo SearchTerm (Tên khách hàng , Mã vận đơn)
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                Query.Where(o => o.CustomerName.Contains(request.SearchTerm) ||
                                 (o.TrackingNumber != null && o.TrackingNumber.Contains(request.SearchTerm)));
            }

            // Sắp xếp: Đơn hàng mới nhất lên đầu
            Query.OrderByDescending(o => o.CreatedDate);

            // Phân trang
            int validPageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            int validPageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            if (validPageSize > 100) validPageSize = 100;

            Query.Skip((validPageIndex - 1) * validPageSize)
                 .Take(validPageSize);
        }
    }
}
