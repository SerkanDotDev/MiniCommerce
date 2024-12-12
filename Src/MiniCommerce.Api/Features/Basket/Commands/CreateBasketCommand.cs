using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MiniCommerce.Api.Models.Requests;
using MiniCommerce.Api.Models.Basket;
using MiniCommerce.Api.Validations.Basket;

namespace MiniCommerce.Api.Features.Basket.Commands
{
    public class CreateBasketCommand : IRequest<BasketResponse>
    {
        public int UserId { get; }
        public List<ProductItemDto> Products { get; }

        public CreateBasketCommand(int userId, List<ProductItemDto> products)
        {
            UserId = userId;
            Products = products ?? throw new ArgumentNullException(nameof(products));
            if (!Products.Any())
            {
                throw new ArgumentException("Basket must contain at least one product.", nameof(products));
            }
        }
    }

    public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, BasketResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly CreateBasketCommandValidator _validator;

        public CreateBasketCommandHandler(
            IBasketRepository basketRepository,
            IProductRepository productRepository)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _validator = new CreateBasketCommandValidator();
        }

        public async Task<BasketResponse> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var basketProducts = new List<BasketItemEntity>();
            decimal totalAmount = 0;

            foreach (var item in request.Products)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.IsDeleted)
                {
                    throw new NotFoundException($"Product not found.");
                }

                basketProducts.Add(new BasketItemEntity
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = item.Quantity
                });

                totalAmount += product.Price * item.Quantity;
            }

            var basket = new BasketEntity
            {
                UserId = request.UserId,
                Products = basketProducts,
                TotalAmount = totalAmount
            };

            await _basketRepository.AddAsync(basket);

            return new BasketResponse
            {
                Id = basket.Id,
                UserId = basket.UserId,
                TotalAmount = basket.TotalAmount,
                Products = basket.Products.Select(p => new BasketItemEntity
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice,
                    Quantity = p.Quantity
                }).ToList()
            };
        }
    }
}
