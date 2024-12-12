using MiniCommerce.Domain.Entities;
using System.Collections.Generic;

namespace MiniCommerce.Api.Models.Basket
{
    public class BasketResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<BasketItemEntity> Products { get; set; }
    }
}
