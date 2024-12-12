using MediatR;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Api.Validations.Basket;

namespace MiniCommerce.Api.Features.Basket.Commands
{
    public class RemoveProductFromBasketCommand : IRequest<Unit>
    {
        public int BasketId { get; }
        public int ProductId { get; }
        public int Quantity { get; }

        public RemoveProductFromBasketCommand(int basketId, int productId, int quantity)
        {
            BasketId = basketId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

    public class RemoveProductFromBasketCommandHandler : IRequestHandler<RemoveProductFromBasketCommand, Unit>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly RemoveProductFromBasketCommandValidator _validator;

        public RemoveProductFromBasketCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
            _validator = new RemoveProductFromBasketCommandValidator();
        }

        public async Task<Unit> Handle(RemoveProductFromBasketCommand request, CancellationToken cancellationToken)
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

            var basketItem = basket.Products.FirstOrDefault(p => p.ProductId == request.ProductId);
            if (basketItem == null)
            {
                throw new NotFoundException("Product not found in basket.");
            }

            if (request.Quantity > basketItem.Quantity)
            {
                throw new InvalidOperationException("Cannot remove more items than exist in basket.");
            }

            if (request.Quantity == basketItem.Quantity)
            {
                basket.Products.Remove(basketItem);
            }
            else
            {
                basketItem.Quantity -= request.Quantity;
            }

            basket.TotalAmount -= basketItem.ProductPrice * request.Quantity;

            await _basketRepository.UpdateAsync(basket);

            return Unit.Value;
        }
    }
}
