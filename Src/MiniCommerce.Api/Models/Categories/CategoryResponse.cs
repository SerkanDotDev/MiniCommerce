namespace MiniCommerce.Api.Models.Categories;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
