using MiniCommerce.Domain.Entities;

namespace MiniCommerce.Api.Services.Authentication;

public interface IJwtService
{
    string GenerateAccessToken(UserEntity user);
    RefreshTokenEntity GenerateRefreshToken(UserEntity user);
    Task<(string AccessToken, RefreshTokenEntity RefreshToken)> GenerateTokensAsync(UserEntity user);
    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    string GetUserIdFromToken(string token);
}
