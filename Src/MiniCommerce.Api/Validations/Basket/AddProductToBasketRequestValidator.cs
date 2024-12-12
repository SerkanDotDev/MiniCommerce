using FluentValidation;
using MiniCommerce.Api.Features.Basket.Commands;

namespace MiniCommerce.Api.Validations.Basket;

public class AddProductToBasketRequestValidator : AbstractValidator<AddProductToBasketCommand>
{
    public AddProductToBasketRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity).NotEmpty()
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}
