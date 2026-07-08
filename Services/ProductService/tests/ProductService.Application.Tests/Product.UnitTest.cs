using ProductService.Application.Features.Products.Commands.CreateProduct;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using Shared.Domain.Interfaces;
using Moq;
using Shared.Application.Common.Interfaces;

namespace ProductService.Application.Tests;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICurrentUserService> _currentUser;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currentUser = new Mock<ICurrentUserService>();

        // Khởi tạo handler với các bản giả (mock)
        _handler = new CreateProductCommandHandler(
            _productRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _currentUser.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateProductAndReturnSuccess()
    {
        // Arrange (Chuẩn bị dữ liệu test)
        var command = new CreateProductCommand(
            Name: "Iphone 15",
            Description: "Apple Smartphone",
            Price: 1000m,
            StockQuantity: 10,
            CategoryId: Guid.NewGuid(),
            ImageUrl: "http://image.com/iphone.png"
        );

        // Act (Thực hiện hành động)
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert (Kiểm tra kết quả)

        // 1. Kiểm tra Result trả về có thành công không
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Data.Should().NotBeEmpty();

        // 2. Kiểm tra xem Repository có được gọi để Add product không
        _productRepositoryMock.Verify(
            repo => repo.AddAsync(
                It.Is<Product>(p => p.Name == command.Name && p.Price.Amount == command.Price),
                It.IsAny<CancellationToken>()),
            Times.Once);

        // 3. Quan trọng nhất: Kiểm tra UnitOfWork đã SaveChanges chưa 
        // (Đây là lúc Domain Event thường được xử lý ngầm)
        _unitOfWorkMock.Verify(
            uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Raise_DomainEvent_Inside_Product()
    {
        // Test này để đảm bảo Product được tạo ra có chứa Event trước khi Save
        // Arrange
        var command = new CreateProductCommand("Test", "Desc", 100, 5, Guid.NewGuid(), null);

        // Act & Assert
        // Chúng ta dùng một kỹ thuật Capture để "bắt" đối tượng product truyền vào AddAsync
        Product? capturedProduct = null;
        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Callback<Product, CancellationToken>((p, ct) => capturedProduct = p);

        await _handler.Handle(command, CancellationToken.None);

        // Kiểm tra xem đối tượng Product bị "bắt" có chứa Event ProductCreated không
        capturedProduct.Should().NotBeNull();
        capturedProduct!.DomainEvents.Should().ContainSingle(e => e.GetType().Name == "ProductCreatedEvent");
    }
}
