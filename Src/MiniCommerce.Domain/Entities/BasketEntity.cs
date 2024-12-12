using System.Collections.Generic;

namespace MiniCommerce.Domain.Entities {
    public class BasketEntity : BaseEntity {
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<BasketItemEntity> Products { get; set; } 
    }
}
