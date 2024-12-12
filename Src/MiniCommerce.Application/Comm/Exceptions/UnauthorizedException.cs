using System.Net;

namespace MiniCommerce.Api.Common.Exceptions;

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message) 
        : base(message, HttpStatusCode.Unauthorized)
    {
    }
}
