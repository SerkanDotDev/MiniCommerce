namespace MiniCommerce.Domain.Entities;

public class RefreshTokenEntity : BaseEntity
{
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public int UserId { get; set; }
}
