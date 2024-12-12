using System.ComponentModel.DataAnnotations;

namespace MiniCommerce.Api.Models.Authentication;

public class RefreshTokenRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
