using MediatR;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Api.Validations.Basket;

namespace MiniCommerce.Api.Features.Basket.Commands
{
    public class AddProductToBasketCommand : IRequest<Unit>
    {
        public int BasketId { get; }
        public int ProductId { get; }
        public int Quantity { get; }

        public AddProductToBasketCommand(int basketId, int productId, int quantity)
        {
            BasketId = basketId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

    public class AddProductToBasketCommandHandler : IRequestHandler<AddProductToBasketCommand, Unit>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly AddProductToBasketCommandValidator _validator;

        public AddProductToBasketCommandHandler(
            IBasketRepository basketRepository, 
            IProductRepository productRepository)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _validator = new AddProductToBasketCommandValidator();
        }

        public async Task<Unit> Handle(AddProductToBasketCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var basket = await _basketRepository.GetByIdAsync(request.BasketId);
            if (basket == null)
            {
                throw new NotFoundException("Basket not found.");
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            var basketItem = basket.Products.FirstOrDefault(p => p.ProductId == request.ProductId);
            if (basketItem != null)
            {
                basketItem.Quantity += request.Quantity;
            }
            else
            {
                basket.Products.Add(new BasketItemEntity
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = request.Quantity
                });
            }

            basket.TotalAmount += product.Price * request.Quantity;

            await _basketRepository.UpdateAsync(basket);

            return Unit.Value;
        }
    }
}
