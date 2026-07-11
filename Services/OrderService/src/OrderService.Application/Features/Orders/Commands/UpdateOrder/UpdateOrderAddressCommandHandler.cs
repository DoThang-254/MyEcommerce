using OrderService.Domain.Interfaces;
using Shared.Application.Common.Models;
using Shared.Application.Features.Handlers;
using Shared.Domain.Interfaces;
using Shared.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderAddressCommandHandler : ICommandHandler<UpdateOrderAddressCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderAddressCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(UpdateOrderAddressCommand request, CancellationToken cancellationToken)
        {
            // 1. Lấy đơn hàng từ Database
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                return Result<bool>.Failure("Không tìm thấy đơn hàng.");
            }

            try
            {
                // 2. Khởi tạo Value Object Address mới từ dữ liệu Client gửi lên
                var newAddress = new Address(
                    request.Street,
                    request.City,
                    request.State,
                    request.Country,
                    request.ZipCode
                );

                // 3. GỌI HÀNH VI NGHIỆP VỤ TỪ DOMAIN (Class Order)
                // Nếu đơn hàng đang giao (Shipping) -> Domain sẽ ném ra InvalidOperationException
                order.UpdateShippingAddress(newAddress);
            }
            catch (ArgumentException ex) // Bắt lỗi validate thiếu thông tin của Address (Street trống...)
            {
                return Result<bool>.Failure($"Dữ liệu địa chỉ không hợp lệ: {ex.Message}");
            }
            catch (InvalidOperationException ex) // Bắt lỗi nghiệp vụ (Đang giao hàng...)
            {
                return Result<bool>.Failure(ex.Message);
            }

            // 4. Lưu thay đổi xuống Database
            // Lưu ý: Không cần gọi _orderRepository.Update(order) nếu bạn dựa vào Change Tracker của EF Core
            // Hàm SaveChangesAsync sẽ tự động phát hiện cột ShippingAddress bị thay đổi và sinh câu lệnh UPDATE
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
