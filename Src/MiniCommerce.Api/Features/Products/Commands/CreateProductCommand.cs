using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MiniCommerce.Api.Models.Products;
using MiniCommerce.Api.Services.FileUpload;
using MiniCommerce.Api.Validations.Products;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniCommerce.Api.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductResponse>
    {
        public CreateProductCommand(string name, string description, decimal price, int categoryId, IFormFile image)
        {
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            Image = image;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly CreateProductCommandValidator _validator;
        private readonly IFileUploadService _fileUploadService;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IFileUploadService fileUploadService)
        {
            _productRepository = productRepository;
            _fileUploadService = fileUploadService;
            _validator = new CreateProductCommandValidator();
        }

        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            string imagePath = null;
            if (request.Image != null)
            {
                imagePath = await _fileUploadService.UploadFileAsync(request.Image, "products");
            }

            var product = new ProductEntity
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Image = imagePath
            };

            await _productRepository.AddAsync(product);

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Image = product.Image
            };
        }
    }
}
