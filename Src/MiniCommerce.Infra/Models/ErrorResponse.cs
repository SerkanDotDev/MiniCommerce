namespace MiniCommerce.Api.Common.Models;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public object? Errors { get; set; }
    public string? StackTrace { get; set; }
}
