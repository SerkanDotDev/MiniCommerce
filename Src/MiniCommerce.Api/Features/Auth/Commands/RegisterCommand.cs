using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Api.Models.Authentication;
using MiniCommerce.Api.Services.Authentication;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Domain.ValueObjects;
using MiniCommerce.Api.Services.FileUpload;
using MiniCommerce.Api.Validations.Auth;

namespace MiniCommerce.Api.Features.Auth.Commands;

public record RegisterCommand : IRequest<AuthResponse>
{
    public RegisterCommand(string email, string password, string firstName, string lastName, IFormFile? profilePhoto)
    {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        ProfilePhoto = profilePhoto;
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
}
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IFileUploadService _fileUploadService;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtService jwtService, IFileUploadService fileUploadService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _fileUploadService = fileUploadService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var validator = new RegisterRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new ApiException("Email already registered");
        }

        string? profilePhotoPath = null;
        if (request.ProfilePhoto != null)
        {
            profilePhotoPath = await _fileUploadService.UploadFileAsync(request.ProfilePhoto, "profile-photos");
        }

        var user = new UserEntity
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Name = new UserName(request.FirstName, request.LastName),
            ProfilePicture = profilePhotoPath
        };

        await _userRepository.AddAsync(user);

        var (accessToken, refreshToken) = await _jwtService.GenerateTokensAsync(user);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };
    }
}