using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Api.Services.FileUpload;
using MiniCommerce.Api.Models.Products;
using MiniCommerce.Api.Validations.Products;

namespace MiniCommerce.Api.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<ProductResponse>
    {
        public UpdateProductCommand(int id, string name, string description, decimal price, int categoryId, IFormFile? image)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            Image = image;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly UpdateProductCommandValidator _validator;

        public UpdateProductCommandHandler(IProductRepository productRepository, IFileUploadService fileUploadService)
        {
            _productRepository = productRepository;
            _fileUploadService = fileUploadService;
            _validator = new UpdateProductCommandValidator();
        }

        public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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
            string imagePath = null;
            if (request.Image != null)
            {
                imagePath = await _fileUploadService.UploadFileAsync(request.Image, "products");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            await _productRepository.UpdateAsync(product);

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
