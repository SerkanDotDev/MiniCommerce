using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Api.Models.Authentication;
using MiniCommerce.Api.Services.Authentication;
using MiniCommerce.Api.Validations.Auth;
using MiniCommerce.Domain.Repositories;

namespace MiniCommerce.Api.Features.Auth.Commands;

public record RefreshTokenCommand : IRequest<AuthResponse>
{
    public RefreshTokenCommand(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    
}
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;

    public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService, IUserRepository userRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validator = new RefreshTokenRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (refreshToken == null)
        {
            throw new UnauthorizedException("Invalid refresh token");
        }

        if (refreshToken.IsRevoked || refreshToken.ExpiryDate < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Token is expired or revoked");
        }
        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);


        var (accessToken, newRefreshToken) = await _jwtService.GenerateTokensAsync(user);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}