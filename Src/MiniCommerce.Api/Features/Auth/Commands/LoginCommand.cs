using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Api.Models.Authentication;
using MiniCommerce.Api.Services.Authentication;
using MiniCommerce.Api.Validations.Auth;
using MiniCommerce.Domain.Repositories;

namespace MiniCommerce.Api.Features.Auth.Commands;

public record LoginCommand : IRequest<AuthResponse>
{
    public LoginCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; }
    public string Password { get; set; }
   
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validator = new LoginRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        var (accessToken, refreshToken) = await _jwtService.GenerateTokensAsync(user);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };
    }
}
