using FluentValidation;
using MiniCommerce.Api.Features.Basket.Commands;

namespace MiniCommerce.Api.Validations.Basket;

public class RemoveProductFromBasketRequestValidator : AbstractValidator<RemoveProductFromBasketCommand>
{
    public RemoveProductFromBasketRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.BasketId)
            .NotEmpty()
            .WithMessage("Basket ID is required");
    }
}
