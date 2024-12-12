using System.Net;

namespace MiniCommerce.Api.Common.Exceptions;

public class NotFoundException : ApiException
{
    public NotFoundException(string message) 
        : base(message, HttpStatusCode.NotFound)
    {
    }
}
