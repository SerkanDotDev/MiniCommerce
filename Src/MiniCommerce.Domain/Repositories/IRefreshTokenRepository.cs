using MiniCommerce.Domain.Entities;

namespace MiniCommerce.Domain.Repositories;

public interface IRefreshTokenRepository : IBaseRepository<RefreshTokenEntity>
{
    Task<RefreshTokenEntity> GetByTokenAsync(string token);
    Task<List<RefreshTokenEntity>> GetValidTokensByUserIdAsync(int userId);
    Task RevokeAllUserTokensAsync(int userId);
}
