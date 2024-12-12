using MediatR;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Domain.Repositories;
using FluentValidation;
using MiniCommerce.Api.Models.Categories;
using MiniCommerce.Api.Validations.Categories;

namespace MiniCommerce.Api.Features.Categories.Commands;

public class CreateCategoryCommand : IRequest<CategoryResponse>
{
    public CreateCategoryCommand(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string? Description { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CreateCategoryCommandValidator _validator;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _validator = new CreateCategoryCommandValidator();
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var category = new CategoryEntity
        {
            Name = request.Name,
            Description = request.Description
        };

        await _categoryRepository.AddAsync(category);

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
    }
}
