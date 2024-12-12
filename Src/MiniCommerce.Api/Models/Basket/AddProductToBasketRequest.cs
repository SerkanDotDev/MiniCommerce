namespace MiniCommerce.Api.Models.Requests
{
    public class AddProductToBasketRequest
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
