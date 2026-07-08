using FluentValidation;

namespace ProductService.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .NotNull().WithMessage("PageIndex is required.")
                .GreaterThan(0).WithMessage("PageIndex must be greater than 0.");
            RuleFor(x => x.PageSize)
                .NotNull().WithMessage("PageSize is required.")
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
        }
    }
}
