using FluentValidation;
using MiniCommerce.Api.Features.Auth.Commands;
using MiniCommerce.Api.Models.Authentication;

namespace MiniCommerce.Api.Validations.Auth;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
}
