using FluentValidation;
using MiniCommerce.Api.Features.Basket.Commands;

namespace MiniCommerce.Api.Validations.Basket;

public class AddProductToBasketCommandValidator : AbstractValidator<AddProductToBasketCommand>
{
    public AddProductToBasketCommandValidator()
    {
        RuleFor(x => x.BasketId).NotEmpty()
            .GreaterThan(0)
            .WithMessage("Basket ID must be greater than 0");

        RuleFor(x => x.ProductId).NotEmpty()
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0");

        RuleFor(x => x.Quantity).NotEmpty()
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}
