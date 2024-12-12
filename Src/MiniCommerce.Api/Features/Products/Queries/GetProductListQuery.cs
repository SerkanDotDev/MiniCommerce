using MediatR;
using MiniCommerce.Domain.Repositories;
using FluentValidation;
using MiniCommerce.Api.Models.Products;
using MiniCommerce.Api.Validations.Products;

namespace MiniCommerce.Api.Features.Products.Queries
{
    public class GetProductListQuery : IRequest<List<ProductResponse>>
    {
        public GetProductListQuery(int pageNumber, int pageSize, int? categoryId = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            CategoryId = categoryId;
        }

        public int PageNumber { get; }
        public int PageSize { get; }
        public int? CategoryId { get; }
    }

    public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, List<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly GetProductListQueryValidator _validator;

        public GetProductListQueryHandler(IProductRepository productRepository, GetProductListQueryValidator validator)
        {
            _productRepository = productRepository;
            _validator = validator;
        }

        public async Task<List<ProductResponse>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var products = request.CategoryId.HasValue 
                ? await _productRepository.GetPaginatedListByCategoryAsync(request.CategoryId.Value, request.PageNumber, request.PageSize)
                : await _productRepository.GetPaginatedListAsync(request.PageNumber, request.PageSize);

            var productResponses = new List<ProductResponse>();
            foreach (var product in products)
            {
                productResponses.Add(new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId
                });
            }

            return productResponses;
        }
    }
}
