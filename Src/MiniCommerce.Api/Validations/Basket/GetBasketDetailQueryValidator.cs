using FluentValidation;
using MiniCommerce.Api.Features.Basket.Queries;

namespace MiniCommerce.Api.Validations.Basket;

public class GetBasketDetailQueryValidator : AbstractValidator<GetBasketDetailQuery>
{
    public GetBasketDetailQueryValidator()
    {
        RuleFor(x => x.BasketId)
            .GreaterThan(0)
            .WithMessage("Basket ID must be greater than 0");
    }
}
