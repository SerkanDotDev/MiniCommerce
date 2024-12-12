using System.Net;
using System.Text.Json;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Api.Common.Models;

namespace MiniCommerce.Api.Common.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            Message = exception.Message
        };

        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = (int)validationEx.StatusCode;
                errorResponse.Errors = validationEx.Errors;
                break;

            case ApiException apiEx:
                response.StatusCode = (int)apiEx.StatusCode;
                errorResponse.Errors = apiEx.Errors;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "An unexpected error occurred.";
                break;
        }

        if (_env.IsDevelopment())
        {
            errorResponse.StackTrace = exception.StackTrace;
        }

        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }
}
