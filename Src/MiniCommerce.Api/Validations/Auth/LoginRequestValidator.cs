using FluentValidation;
using MiniCommerce.Api.Features.Auth.Commands;
using MiniCommerce.Api.Models.Authentication;

namespace MiniCommerce.Api.Validations.Auth;

public class LoginRequestValidator : AbstractValidator<LoginCommand>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long");
    }
}
