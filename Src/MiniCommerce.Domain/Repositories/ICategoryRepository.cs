using System.Collections.Generic;
using System.Threading.Tasks;
using MiniCommerce.Domain.Entities;

namespace MiniCommerce.Domain.Repositories
{
    public interface ICategoryRepository : IBaseRepository<CategoryEntity>
    {
        Task<List<CategoryEntity>> GetAllAsync();
    }
}
