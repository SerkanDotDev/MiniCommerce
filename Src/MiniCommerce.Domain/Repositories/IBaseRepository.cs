using System.Collections.Generic;
using System.Threading.Tasks;
using MiniCommerce.Domain.Entities;

namespace MiniCommerce.Domain.Repositories {
    public interface IBaseRepository<T> where T : BaseEntity {
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task HardDeleteAsync(int id);
    }
}
