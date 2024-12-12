namespace MiniCommerce.Api.Models.Requests
{
    public class RemoveProductFromBasketRequest
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
