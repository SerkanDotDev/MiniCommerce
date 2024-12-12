namespace MiniCommerce.Api.Models.Requests
{
    public class CreateBasketRequest
    {
        public int UserId { get; set; }

        public List<ProductItemDto> Products { get; set; }

    }

    public class ProductItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
