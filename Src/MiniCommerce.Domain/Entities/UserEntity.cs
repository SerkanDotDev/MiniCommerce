using System.ComponentModel.DataAnnotations.Schema;
using MiniCommerce.Domain.ValueObjects;

namespace MiniCommerce.Domain.Entities {
    [Table("Users")]
    public class UserEntity : BaseEntity {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserName Name { get; set; }
        public string? ProfilePicture { get; set; }
        public string Role { get; set; } = "User";
    }
}
