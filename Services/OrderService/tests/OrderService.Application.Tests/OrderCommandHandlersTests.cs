using Moq;
using OrderService.Application.Features.Orders.Commands.CreateOrder;
using OrderService.Application.Features.Orders.Commands.UpdateOrder;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Domain.Interfaces;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Interfaces;
using Shared.Domain.ValueObjects;

namespace OrderService.Application.Tests
{
    public class OrderCommandHandlersTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        public OrderCommandHandlersTests()
        {
            // Khởi tạo các Mock object trước mỗi test case
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
        }

        #region --- HÀM TRỢ GIÚP TẠO DỮ LIỆU MẪU (HELPERS) ---

        // Helper tạo cấu trúc Đơn hàng phục vụ test Cancel / Update Address
        private Order CreateValidPendingOrder(Guid orderId)
        {
            var email = EmailAddress.Create("test@gmail.com");
            var phone = PhoneNumber.Create("0912345678");
            var address = new Address("123 Test St", "Test City", "Test State", "Test Country", "12345");
            var shippingFee = Money.Of(30000, "VND");

            var order = new Order(Guid.NewGuid(), "Test User", email, phone, address, shippingFee);

            // Dùng Reflection để gán Id nội bộ
            var idProperty = typeof(Order).GetProperty("Id");
            idProperty?.SetValue(order, orderId);

            return order;
        }

        // Helper tạo Command mẫu phục vụ test Create Order
        private CreateOrderCommand CreateValidCreateCommand()
        {
            var shippingAddress = new AddressDto(
                Street: "123 Đường Nguyễn Trãi",
                City: "Quận 5",
                State: "Hồ Chí Minh",
                Country: "Việt Nam",
                ZipCode: "700000"
            );

            var items = new List<OrderItemDto>
            {
                new OrderItemDto(Guid.NewGuid(), "Điện thoại iPhone 15", "iphone15.png", 25000000, 1),
                new OrderItemDto(Guid.NewGuid(), "Ốp lưng Silicone", "op-lung.png", 500000, 2)
            };

            return new CreateOrderCommand(
                CustomerName: "Nguyễn Văn A",
                CustomerEmail: "vana@gmail.com",
                CustomerPhone: "0912345678",
                ShippingAddress: shippingAddress,
                OrderItems: items
            );
        }

        #endregion

        #region --- SECTION 1: TEST CHO CREATE ORDER ---

        [Fact]
        public async Task CreateOrder_WhenCommandIsValid_ShouldCreateOrderSuccessfully()
        {
            // Arrange
            var command = CreateValidCreateCommand();
            _orderRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                                .Returns(Task.CompletedTask);

            var handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object, _currentUserServiceMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Data);
            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_WhenItemQuantityIsZeroOrNegative_ShouldThrowArgumentException()
        {
            // Arrange (Số lượng sản phẩm lỗi = 0)
            var invalidItems = new List<OrderItemDto>
            {
                new OrderItemDto(Guid.NewGuid(), "Sản phẩm lỗi", "error.png", 100000, 0)
            };
            var command = CreateValidCreateCommand() with { OrderItems = invalidItems };
            var handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object, _currentUserServiceMock.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );

            Assert.Contains("Số lượng phải lớn hơn 0", actualException.Message, StringComparison.OrdinalIgnoreCase);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateOrder_WhenProductNameIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange (Tên sản phẩm trống)
            var invalidItems = new List<OrderItemDto>
            {
                new OrderItemDto(Guid.NewGuid(), "   ", "error.png", 100000, 2)
            };
            var command = CreateValidCreateCommand() with { OrderItems = invalidItems };
            var handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object, _currentUserServiceMock.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );

            Assert.Contains("Tên sản phẩm không được trống", actualException.Message, StringComparison.OrdinalIgnoreCase);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region --- SECTION 2: TEST CHO CANCEL ORDER ---

        [Fact]
        public async Task CancelOrder_WhenOrderExistsAndStatusIsPending_ShouldSucceed()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = CreateValidPendingOrder(orderId);

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);

            var handler = new CancelOrderCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new CancelOrderCommand(orderId, "Khách hàng đổi ý");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(OrderStatus.Cancelled, order.Status);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancelOrder_WhenOrderIsShipping_ShouldReturnFailureResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = CreateValidPendingOrder(orderId);
            order.TransitionToStatus(OrderStatus.Shipping);

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);

            var handler = new CancelOrderCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new CancelOrderCommand(orderId, "Hủy khi đang giao");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Không thể hủy", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CancelOrder_WhenOrderIsAlreadyDelivered_ShouldReturnFailureResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = CreateValidPendingOrder(orderId);
            order.TransitionToStatus(OrderStatus.Delivered);

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);

            var handler = new CancelOrderCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new CancelOrderCommand(orderId, "Đã nhận hàng xong");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Không thể hủy", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region --- SECTION 3: TEST CHO UPDATE ORDER ADDRESS ---

        [Fact]
        public async Task UpdateAddress_WhenOrderIsPendingAndAddressIsValid_ShouldSucceed()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = CreateValidPendingOrder(orderId);

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);

            var handler = new UpdateOrderAddressCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new UpdateOrderAddressCommand(
                OrderId: orderId,
                Street: "99 New Street",
                City: "New City",
                State: "New State",
                Country: "New Country",
                ZipCode: "99999"
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("99 New Street", order.ShippingAddress.Street);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAddress_WhenOrderIsProcessingAndAddressIsValid_ShouldSucceed()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = CreateValidPendingOrder(orderId);
            order.TransitionToStatus(OrderStatus.Processing);

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);

            var handler = new UpdateOrderAddressCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new UpdateOrderAddressCommand(orderId, "456 Đống Đa", "Hà Nội", "HN", "Việt Nam", "100000");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("456 Đống Đa", order.ShippingAddress.Street);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAddress_WhenStreetIsEmpty_ShouldReturnFailureResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = CreateValidPendingOrder(orderId);

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);

            var handler = new UpdateOrderAddressCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new UpdateOrderAddressCommand(orderId, "", "City", "State", "Country", "Zip");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Dữ liệu địa chỉ không hợp lệ", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAddress_WhenOrderIsDelivered_ShouldReturnFailureResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = CreateValidPendingOrder(orderId);
            order.TransitionToStatus(OrderStatus.Delivered);

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);

            var handler = new UpdateOrderAddressCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new UpdateOrderAddressCommand(orderId, "St", "City", "State", "Country", "Zip");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Không thể đổi địa chỉ khi đơn hàng đang được giao", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAddress_WhenOrderIsShipping_ShouldReturnFailureResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = CreateValidPendingOrder(orderId);
            order.TransitionToStatus(OrderStatus.Shipping);

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);

            var handler = new UpdateOrderAddressCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new UpdateOrderAddressCommand(orderId, "Địa chỉ mới", "City", "State", "Country", "Zip");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Không thể đổi địa chỉ khi đơn hàng đang được giao", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task BothHandlers_WhenOrderNotFound_ShouldReturnFailure()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Order?)null);

            var cancelHandler = new CancelOrderCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
            var addressHandler = new UpdateOrderAddressCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);

            // Act
            var cancelResult = await cancelHandler.Handle(new CancelOrderCommand(orderId, "Lý do"), CancellationToken.None);
            var addressResult = await addressHandler.Handle(new UpdateOrderAddressCommand(orderId, "St", "C", "S", "C", "Z"), CancellationToken.None);

            // Assert
            Assert.False(cancelResult.IsSuccess);
            Assert.Equal("Không tìm thấy đơn hàng.", cancelResult.ErrorMessage);

            Assert.False(addressResult.IsSuccess);
            Assert.Equal("Không tìm thấy đơn hàng.", addressResult.ErrorMessage);
        }

        #endregion
    }
}