using FluentValidation;
using MiniCommerce.Api.Features.Products.Commands;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MiniCommerce.Api.Validations.Products;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(200)
            .WithMessage("Product name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Product description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than 0");

        RuleFor(x => x.Image)
            .NotNull()
            .WithMessage("Image file is required")
            .Must(file => file == null || IsValidImageFile(file))
            .WithMessage("Invalid image file. Only .jpg, .jpeg, .png files are allowed");
    }

    private bool IsValidImageFile(IFormFile file)
    {
        if (file == null) return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        return allowedExtensions.Contains(fileExtension);
    }
}
