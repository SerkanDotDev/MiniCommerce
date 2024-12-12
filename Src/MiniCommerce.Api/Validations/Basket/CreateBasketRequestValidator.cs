using FluentValidation;
using MiniCommerce.Api.Features.Basket.Commands;

namespace MiniCommerce.Api.Validations.Basket;

public class CreateBasketRequestValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}
