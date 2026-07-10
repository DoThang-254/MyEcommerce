using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Interfaces;
using Shared.Application.Common.Models;
using Shared.Application.Features.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Features.Orders.Queries
{
    public class GetOrderQueryHandler : IQueryHandlerPagedResult<GetOrderQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOrderQueryHandler> _logger;

        public GetOrderQueryHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<GetOrderQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PagedResult<OrderDto>>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Đang lấy danh sách đơn hàng...");

                // 1. Áp dụng các điều kiện lọc từ Specification
                var spec = new OrdersByFilterSpec(request);

                // 2. Gọi DB để lấy dữ liệu và đếm tổng số dòng
                var orders = await _orderRepository.ListAsync(spec, cancellationToken);
                var totalRecords = await _orderRepository.CountAsync(spec, cancellationToken);

                // 3. Nếu không có dữ liệu
                if (orders == null || !orders.Any())
                {
                    _logger.LogWarning("Không tìm thấy đơn hàng nào khớp với điều kiện.");
                    return Result<PagedResult<OrderDto>>.Success(
                        new PagedResult<OrderDto>(new List<OrderDto>(), 0, request.PageIndex, request.PageSize));
                }

                // 4. Map sang DTO và đóng gói vào PagedResult
                var dtos = _mapper.Map<IReadOnlyList<OrderDto>>(orders);
                var pagedResult = new PagedResult<OrderDto>(
                    items: dtos,
                    totalCount: totalRecords,
                    pageNumber: request.PageIndex,
                    pageSize: request.PageSize
                );

                return Result<PagedResult<OrderDto>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đơn hàng.");
                return Result<PagedResult<OrderDto>>.Failure("Đã xảy ra lỗi hệ thống khi lấy dữ liệu.");
            }
        }
    }
}
