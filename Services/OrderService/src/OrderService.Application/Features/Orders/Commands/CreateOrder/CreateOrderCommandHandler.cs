using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;
using Shared.Application.Common.Interfaces;
using Shared.Application.Common.Models;
using Shared.Application.Features.Handlers;
using Shared.Domain.Interfaces;
using Shared.Domain.ValueObjects;

namespace OrderService.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _orderRepository = orderRepository ;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Khởi tạo các Value Object từ Command
            var email = EmailAddress.Create(request.CustomerEmail);
            var phone = PhoneNumber.Create(request.CustomerPhone);
            var address = new Address(
                request.ShippingAddress.Street,
                request.ShippingAddress.City,
                request.ShippingAddress.State,
                request.ShippingAddress.Country,
                request.ShippingAddress.ZipCode
            );
            // Giả định phí ship tính toán từ hệ thống hoặc mặc định ban đầu
            var defaultShippingFee = Money.Of(30000, "VND");

            var userIdString = _currentUserService.UserId;
            if (userIdString == Guid.Empty)
            {
                return Result<Guid>.Failure("User is not authenticated or token is invalid."); // Tuỳ thuộc vào cấu trúc Result của bạn
            }

            // 2. Tạo Order tổng trước bằng Constructor public của bạn (Trạng thái Pending)
            var order = new Order(
                //userId: request.CustomerId,
                userId: _currentUserService.UserId, // Lấy từ ICurrentUserService
                customerName: request.CustomerName,
                email: email,
                phone: phone,
                address: address,
                shippingFee: defaultShippingFee
            );

            // 3. Duyệt danh sách các mặt hàng gửi lên
            foreach (var itemDto in request.OrderItems)
            {
                var unitPrice = Money.Of(itemDto.Price, "VND");

                // Gọi hành vi nghiệp vụ của Order để tự sinh OrderItem nội bộ và tự tính lại tổng tiền!
                order.AddOrderItem(
                    productId: itemDto.ProductId,
                    productName: itemDto.ProductName,
                    productImage: itemDto.ProductImage,
                    unitPrice: unitPrice,
                    quantity: itemDto.Quantity
                );
            }

            // 4. Lưu cả cụm Aggregate Root (Order và tự động lưu cả OrderItems) xuống DB
            await _orderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(order.Id);
        }
    }
}
