using FluentValidation;
using MiniCommerce.Api.Features.Users.Commands;
using System.Text.RegularExpressions;

namespace MiniCommerce.Api.Validations.Users;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled);

    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .MaximumLength(100)
            .WithMessage("Email cannot exceed 100 characters")
            .Matches(EmailRegex)
            .WithMessage("Invalid email format");

        RuleFor(x => x.ProfilePicture)
            .Must((command, file) => file == null || IsValidImageFile(command.ProfilePicture))
            .WithMessage("Profile picture must be a valid image file");

        bool IsValidImageFile(IFormFile file)
        {
            if (file == null) return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
