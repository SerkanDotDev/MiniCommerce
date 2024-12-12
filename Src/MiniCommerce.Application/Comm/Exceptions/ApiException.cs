using System.Net;

namespace MiniCommerce.Api.Common.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public object? Errors { get; }

    public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, object? errors = null) 
        : base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
    }
}
