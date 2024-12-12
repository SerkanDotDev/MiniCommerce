using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Api.Models.Categories;
using MiniCommerce.Api.Validations.Categories;

namespace MiniCommerce.Api.Features.Categories.Queries
{
    public class GetCategoryByIdQuery : IRequest<CategoryResponse>
    {
        public int Id { get; }

        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly GetCategoryByIdQueryValidator _validator;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _validator = new GetCategoryByIdQueryValidator();
        }

        public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new NotFoundException("Category not found.");
            }

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
