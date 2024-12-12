using Microsoft.EntityFrameworkCore;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Infra.Data;

namespace MiniCommerce.Infra.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshTokenEntity>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RefreshTokenEntity> GetByTokenAsync(string token)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.Token == token && !r.IsRevoked);
    }

    public async Task<List<RefreshTokenEntity>> GetValidTokensByUserIdAsync(int userId)
    {
        return await _dbSet
            .Where(r => r.UserId == userId && !r.IsRevoked && r.ExpiryDate > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task RevokeAllUserTokensAsync(int userId)
    {
        var tokens = await _dbSet
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
        }

        await _context.SaveChangesAsync();
    }
}
