using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Domain.Entities;
using MiniCommerce.Domain.Repositories;
using MiniCommerce.Domain.ValueObjects;
using MiniCommerce.Api.Services.FileUpload;
using MiniCommerce.Api.Validations.Users;

namespace MiniCommerce.Api.Features.Users.Commands;

public class UpdateUserCommand : IRequest<UserEntity>
{
    public int UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public IFormFile? ProfilePicture { get; }

    public UpdateUserCommand(int userId, string firstName, string lastName, string email, IFormFile? profilePicture)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ProfilePicture = profilePicture;
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserEntity>
{
    private readonly IUserRepository _userRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandHandler(IUserRepository userRepository, IFileUploadService fileUploadService)
    {
        _userRepository = userRepository;
        _validator = new UpdateUserCommandValidator();
        _fileUploadService = fileUploadService;
    }

    public async Task<UserEntity> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException($"User not found.");

        user.Name = new UserName(request.FirstName, request.LastName);
        user.Email = request.Email;

        if (request.ProfilePicture != null)
        {
            user.ProfilePicture = await _fileUploadService.UploadFileAsync(request.ProfilePicture, "profile-photos");

        }

        await _userRepository.UpdateAsync(user);
        return user;
    }
}
