

using MediatR;
using MiniCommerce.Api.Models.Categories;
using MiniCommerce.Domain.Repositories;

namespace MiniCommerce.Application.Categories.Queries;

public class GetCategoryListQuery : IRequest<List<CategoryResponse>>
{
}

public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, List<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryListQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryResponse>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {

        var categories = await _categoryRepository.GetAllAsync();
        
        return categories.Where(c => !c.IsDeleted).Select(category => new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        }).ToList();
    }
}
