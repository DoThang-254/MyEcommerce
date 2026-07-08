namespace Shared.Domain.ValueObjects;

public record PhoneNumber(string Value)
{
    public static PhoneNumber Create(string number)
    {
        // Thêm logic Validation hoặc format số điện thoại tại đây
        if (string.IsNullOrWhiteSpace(number) || number.Length < 10)
            throw new ArgumentException("Số điện thoại không hợp lệ");

        return new PhoneNumber(number);
    }
}
