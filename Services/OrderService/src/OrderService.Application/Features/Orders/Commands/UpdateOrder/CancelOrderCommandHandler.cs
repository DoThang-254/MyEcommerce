using OrderService.Domain.Interfaces;
using Shared.Application.Common.Models;
using Shared.Application.Features.Handlers;
using Shared.Domain.Interfaces;

namespace OrderService.Application.Features.Orders.Commands.UpdateOrder
{
    public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Lấy đơn hàng từ DB lên
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                return Result<bool>.Failure("Không tìm thấy đơn hàng.");
            }

            try
            {
                // 2. GỌI HÀNH VI NGHIỆP VỤ TỪ DOMAIN 
                // Nếu đơn hàng đang vận chuyển, hàm Cancel() sẽ throw Exception và bị bắt ở khối catch
                order.Cancel();

                // (Tùy chọn) Có thể gán CancelReason ở đây nếu bạn mở public set hoặc thêm parameter vào hàm Cancel()
            }
            catch (InvalidOperationException ex)
            {
                // Bắt lỗi nghiệp vụ (Domain Exception) và trả về cho Client
                return Result<bool>.Failure(ex.Message);
            }

            // 3. Cập nhật xuống Database
            _orderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
