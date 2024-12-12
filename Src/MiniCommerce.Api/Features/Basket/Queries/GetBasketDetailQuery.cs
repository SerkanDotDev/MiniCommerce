using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Api.Models.Basket;
using MiniCommerce.Api.Validations.Basket;

namespace MiniCommerce.Api.Features.Basket.Queries
{
    public class GetBasketDetailQuery : IRequest<BasketResponse>
    {
        public int BasketId { get; }

        public GetBasketDetailQuery(int basketId)
        {
            BasketId = basketId;
        }
    }

    public class GetBasketDetailQueryHandler : IRequestHandler<GetBasketDetailQuery, BasketResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly GetBasketDetailQueryValidator _validator;

        public GetBasketDetailQueryHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
            _validator = new GetBasketDetailQueryValidator();
        }

        public async Task<BasketResponse> Handle(GetBasketDetailQuery request, CancellationToken cancellationToken)
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

                var response = new BasketResponse
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

                return response;
            }
        }
    }

