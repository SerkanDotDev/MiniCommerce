using System.Threading.Tasks;
using MiniCommerce.Domain.Entities;

namespace MiniCommerce.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        public Task<UserEntity> GetByEmailAsync(string email);
    }
}
