using System.ComponentModel.DataAnnotations;

namespace MiniCommerce.Api.Models.Authentication;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
