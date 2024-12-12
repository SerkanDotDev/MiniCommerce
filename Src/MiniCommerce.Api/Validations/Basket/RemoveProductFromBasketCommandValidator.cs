using FluentValidation;
using MiniCommerce.Api.Features.Basket.Commands;

namespace MiniCommerce.Api.Validations.Basket;

public class RemoveProductFromBasketCommandValidator : AbstractValidator<RemoveProductFromBasketCommand>
{
    public RemoveProductFromBasketCommandValidator()
    {
        RuleFor(x => x.BasketId)
            .GreaterThan(0)
            .WithMessage("Basket ID must be greater than 0");

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}
