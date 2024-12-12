using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MiniCommerce.Api.Models.Authentication;

public class RegisterRequest
{
    
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
}
