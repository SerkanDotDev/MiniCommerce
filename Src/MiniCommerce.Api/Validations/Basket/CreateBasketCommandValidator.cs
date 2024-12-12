using FluentValidation;
using MiniCommerce.Api.Features.Basket.Commands;
using MiniCommerce.Api.Models.Requests;

namespace MiniCommerce.Api.Validations.Basket;

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");

        RuleFor(x => x.Products)
            .NotEmpty()
            .WithMessage("At least one product is required");

        RuleForEach(x => x.Products)
            .SetValidator(new ProductItemDtoValidator());
    }
}

public class ProductItemDtoValidator : AbstractValidator<ProductItemDto>
{
    public ProductItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}
