using FluentValidation;

namespace OrderService.Application.Features.Orders.Queries
{
    public class GetOrderQueryValidator : AbstractValidator<GetOrderQuery>
    {
        public GetOrderQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .NotNull().WithMessage("PageIndex is required.")
                .GreaterThan(0).WithMessage("PageIndex must be greater than 0.");

            RuleFor(x => x.PageSize)
                .NotNull().WithMessage("PageSize is required.")
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

            // Logic phụ: Nếu có truyền ngày, ngày bắt đầu không được lớn hơn ngày kết thúc
            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("Từ ngày phải nhỏ hơn hoặc bằng Đến ngày.");
        }
    }
}
