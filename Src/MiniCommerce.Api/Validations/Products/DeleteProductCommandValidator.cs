using FluentValidation;
using MiniCommerce.Api.Features.Products.Commands;

namespace MiniCommerce.Api.Validations.Products;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0");
    }
}
