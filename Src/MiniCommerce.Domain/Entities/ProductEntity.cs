namespace MiniCommerce.Domain.Entities {
    public class ProductEntity : BaseEntity {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
    }
}
