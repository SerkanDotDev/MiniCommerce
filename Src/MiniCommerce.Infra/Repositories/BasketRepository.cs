using Microsoft.EntityFrameworkCore;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Infra.Data;

namespace MiniCommerce.Infra.Repositories
{
    public class BasketRepository : BaseRepository<BasketEntity>, IBasketRepository
    {
        public BasketRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
