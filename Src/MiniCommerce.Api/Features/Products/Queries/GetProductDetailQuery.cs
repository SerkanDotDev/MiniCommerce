using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Api.Models.Products;
using MiniCommerce.Api.Validations.Products;

namespace MiniCommerce.Api.Features.Products.Queries
{
    public class GetProductDetailQuery : IRequest<ProductResponse>
    {
        public GetProductDetailQuery(int id)
        {
            Id = id;
        }
        
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetProductDetailQuery, ProductResponse>
        {
            private readonly IProductRepository _productRepository;
            private readonly GetProductDetailQueryValidator _validator;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
                _validator = new GetProductDetailQueryValidator();
            }

            public async Task<ProductResponse> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var product = await _productRepository.GetByIdAsync(request.Id);
                if (product == null)
                {
                    throw new NotFoundException("Product not found.");
                }

                return new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId
                };
            }
        }
    }
}
