using System.Net;

namespace backend_api.Common.Exceptions
{
    /// <summary>
    /// API exception base class
    /// </summary>
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorType { get; }

        public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string errorType = "GENERAL_ERROR") 
            : base(message)
        {
            StatusCode = statusCode;
            ErrorType = errorType;
        }

        public ApiException(string message, Exception innerException, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string errorType = "GENERAL_ERROR") 
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ErrorType = errorType;
        }
    }

    /// <summary>
    /// Validation exception
    /// </summary>
    public class ValidationException : ApiException
    {
        public ValidationException(string message) : base(message, HttpStatusCode.BadRequest, "VALIDATION_ERROR")
        {
        }
    }

    /// <summary>
    /// Not found exception
    /// </summary>
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound, "NOT_FOUND")
        {
        }
    }

    /// <summary>
    /// Unauthorized exception
    /// </summary>
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message) : base(message, HttpStatusCode.Unauthorized, "UNAUTHORIZED")
        {
        }
    }
}
