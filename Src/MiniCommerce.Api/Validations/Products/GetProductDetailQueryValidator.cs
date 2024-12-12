using FluentValidation;
using MiniCommerce.Api.Features.Products.Queries;

namespace MiniCommerce.Api.Validations.Products;

public class GetProductDetailQueryValidator : AbstractValidator<GetProductDetailQuery>
{
    public GetProductDetailQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0");
    }
}
