using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Api.Models.Categories;
using MiniCommerce.Api.Validations.Categories;

namespace MiniCommerce.Api.Features.Categories.Commands;

public class UpdateCategoryCommand : IRequest<CategoryResponse>
{
    public UpdateCategoryCommand(int id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly UpdateCategoryCommandValidator _validator;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _validator = new UpdateCategoryCommandValidator();
    }

    public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var category = await _categoryRepository.GetByIdAsync(request.Id);
        
        if (category == null)
            throw new NotFoundException($"Category not found.");

        bool hasChanges = false;

        if (category.Name != request.Name)
        {
            category.Name = request.Name;
            hasChanges = true;
        }

        if (category.Description != request.Description)
        {
            category.Description = request.Description;
            hasChanges = true;
        }

        if (hasChanges)
        {
            await _categoryRepository.UpdateAsync(category);
        }

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
    }
}
