using System.Collections.Generic;
using System.Threading.Tasks;
using MiniCommerce.Domain.Entities;

namespace MiniCommerce.Domain.Repositories
{
    public interface IProductRepository : IBaseRepository<ProductEntity>
    {
        Task<List<ProductEntity>> GetByCategoryIdAsync(int categoryId);
        Task<List<ProductEntity>> GetPaginatedListAsync(int pageNumber, int pageSize);
        Task<List<ProductEntity>> GetPaginatedListByCategoryAsync(int categoryId, int pageNumber, int pageSize);
    }
}
