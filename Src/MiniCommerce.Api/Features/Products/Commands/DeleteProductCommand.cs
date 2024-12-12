using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Api.Validations.Products;
using MiniCommerce.Domain.Repositories;

namespace MiniCommerce.Api.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public DeleteProductCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly DeleteProductCommandValidator _validator;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _validator = new DeleteProductCommandValidator();
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
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

            await _productRepository.DeleteAsync(product.Id);

            return Unit.Value;
        }
    }
}
