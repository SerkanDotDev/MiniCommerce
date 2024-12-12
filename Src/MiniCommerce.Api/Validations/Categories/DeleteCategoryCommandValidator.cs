using FluentValidation;
using MiniCommerce.Api.Features.Categories.Commands;

namespace MiniCommerce.Api.Validations.Categories;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Category ID must be greater than 0");
    }
}
