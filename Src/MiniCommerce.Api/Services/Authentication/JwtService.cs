using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MiniCommerce.Api.Configuration;
using MiniCommerce.Api.Settings;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Domain.Repositories;

namespace MiniCommerce.Api.Services.Authentication;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtConfig;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public JwtService(JwtSettings jwtConfig, IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtConfig = jwtConfig;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public string GenerateAccessToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiration),
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public RefreshTokenEntity GenerateRefreshToken(UserEntity user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return new RefreshTokenEntity
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiration),
            UserId = user.Id
        };
    }

    public async Task<(string AccessToken, RefreshTokenEntity RefreshToken)> GenerateTokensAsync(UserEntity user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(user);


        await _refreshTokenRepository.RevokeAllUserTokensAsync(user.Id);
        
        await _refreshTokenRepository.AddAsync(refreshToken);

        return (accessToken, refreshToken);
    }

    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        
        if (storedToken == null || 
            storedToken.IsRevoked || 
            storedToken.ExpiryDate <= DateTime.UtcNow)
        {
            return false;
        }

        return true;
    }

    public string GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _jwtConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtConfig.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
    }
}
