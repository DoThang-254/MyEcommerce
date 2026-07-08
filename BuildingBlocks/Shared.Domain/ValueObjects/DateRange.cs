namespace Shared.Domain.ValueObjects;

public record DateRange(DateTime StartDate, DateTime EndDate)
{
    public static DateRange Create(DateTime start, DateTime end)
    {
        if (start > end)
            throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");

        return new DateRange(start, end);
    }

    public bool Overlaps(DateRange other) => StartDate < other.EndDate && EndDate > other.StartDate;
}
