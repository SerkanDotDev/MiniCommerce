using System.Net;

namespace MiniCommerce.Api.Common.Exceptions;

public class ValidationException : ApiException
{
    public ValidationException(object errors) 
        : base("Validation failed", HttpStatusCode.BadRequest, errors)
    {
    }
}
