namespace MiniCommerce.Api.Features.Users.Queries;

public class GetUserProfileResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfilePicture { get; set; }
}
