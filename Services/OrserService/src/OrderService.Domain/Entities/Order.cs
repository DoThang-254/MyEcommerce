using OrderService.Domain.Enums;
using Shared.Domain.Entities;
using Shared.Domain.ValueObjects;

namespace OrderService.Domain.Entities
{
    public class Order : AggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;

        // Tích hợp Value Objects
        public EmailAddress CustomerEmail { get; private set; } = null!;
        public PhoneNumber CustomerPhone { get; private set; } = null!;
        public Address ShippingAddress { get; private set; } = null!;
        public DateRange? DeliveryWindow { get; private set; } // Khung giờ giao hàng dự kiến

        public string? Note { get; private set; }

        // Tiền tệ được định nghĩa chặt chẽ bằng Money
        public Money SubTotal { get; private set; } = Money.Zero();
        public Money DiscountAmount { get; private set; } = Money.Zero();
        public Money ShippingFee { get; private set; } = Money.Zero();
        public Money TaxAmount { get; private set; } = Money.Zero();
        public Money TotalAmount { get; private set; } = Money.Zero();

        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public string PaymentMethod { get; private set; } = string.Empty;
        public bool IsPaid { get; private set; }
        public DateTime? PaidAt { get; private set; }

        public string? TrackingNumber { get; private set; }
        public string? CancelReason { get; private set; }

        // Bảo vệ danh sách OrderItems, chỉ cho đọc từ bên ngoài
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        // Constructor rỗng cho EF Core
        private Order() { }

        // Constructor để tạo Order mới với trạng thái hợp lệ ngay từ đầu
        public Order(Guid userId, string customerName, EmailAddress email, PhoneNumber phone, Address address, Money shippingFee)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CustomerName = customerName;
            CustomerEmail = email;
            CustomerPhone = phone;
            ShippingAddress = address;
            ShippingFee = shippingFee;
            Status = OrderStatus.Pending;
        }

        // HÀNH VI NGHIỆP VỤ (BEHAVIORS)

        public void AddOrderItem(Guid productId, string productName, string? productImage, Money unitPrice, int quantity)
        {
            var existingItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                // Nếu sản phẩm đã tồn tại, có thể có logic gộp số lượng (tùy nghiệp vụ)
                throw new InvalidOperationException("Sản phẩm đã tồn tại trong giỏ hàng.");
            }

            var newItem = new OrderItem(productId, productName, productImage, unitPrice, quantity);
            _orderItems.Add(newItem);

            // Bất cứ khi nào thêm item, AggregateRoot tự động tính lại tổng tiền
            RecalculateTotals();
        }

        public void SetDeliveryWindow(DateRange deliveryWindow)
        {
            DeliveryWindow = deliveryWindow ?? throw new ArgumentNullException(nameof(deliveryWindow));
        }

        public void ApplyDiscount(Money discount)
        {
            // Validate tiền tệ
            if (SubTotal.Currency != discount.Currency)
                throw new InvalidOperationException("Loại tiền tệ của mã giảm giá không khớp.");

            DiscountAmount = discount;
            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            if (!_orderItems.Any()) return;

            string currency = _orderItems.First().TotalPrice.Currency;

            decimal subTotalValue = _orderItems.Sum(item => item.TotalPrice.Amount);
            SubTotal = Money.Of(subTotalValue, currency);

            decimal totalValue = subTotalValue + ShippingFee.Amount + TaxAmount.Amount - DiscountAmount.Amount;

            // Đảm bảo tổng tiền không bị âm
            TotalAmount = Money.Of(Math.Max(0, totalValue), currency);
        }
    }
}