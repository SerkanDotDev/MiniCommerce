using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Api.Validations.Categories;
using MiniCommerce.Domain.Repositories;

namespace MiniCommerce.Api.Features.Categories.Commands;

public class DeleteCategoryCommand : IRequest<Unit>
{
    public int Id { get; }

    public DeleteCategoryCommand(int id)
    {
        Id = id;
    }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly DeleteCategoryCommandValidator _validator;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _validator = new DeleteCategoryCommandValidator();
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var category = await _categoryRepository.GetByIdAsync(request.Id);
        
        if (category == null)
            throw new NotFoundException($"Category not found.");

        var products = await _productRepository.GetByCategoryIdAsync(request.Id);
        foreach (var product in products)
        {
            await _productRepository.DeleteAsync(product.Id);
        }

        await _categoryRepository.DeleteAsync(category.Id);
        
        return Unit.Value;
    }
}
