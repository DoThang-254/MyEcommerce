namespace Shared.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    // Constructor này đảm bảo bắt buộc phải qua bước kiểm tra dữ liệu
    public Money(decimal amount, string currency = "VND")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency must be provided.", nameof(currency));

        Amount = amount;
        Currency = currency;
    }

    // Static Factory để tạo đối tượng nhanh
    public static Money Of(decimal amount, string currency = "VND") => new(amount, currency);
    public static Money Zero(string currency = "VND") => new(0, currency);

    public override string ToString() => $"{Amount} {Currency}";
}
