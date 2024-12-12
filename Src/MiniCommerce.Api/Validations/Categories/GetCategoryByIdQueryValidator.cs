using FluentValidation;
using MiniCommerce.Api.Features.Categories.Queries;

namespace MiniCommerce.Api.Validations.Categories;

public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Category ID must be greater than 0");
    }
}
