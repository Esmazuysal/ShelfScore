using backend_api.Common.Exceptions;
using backend_api.DTOs.Responses;
using System.Net;
using System.Text.Json;

namespace backend_api.Infrastructure.Middleware
{
    /// <summary>
    /// Exception handling middleware
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                ApiException apiException => new ApiResponse
                {
                    Success = false,
                    Message = apiException.Message,
                    Errors = new { ErrorType = apiException.ErrorType }
                },
                ValidationException validationException => new ApiResponse
                {
                    Success = false,
                    Message = validationException.Message,
                    Errors = new { ErrorType = validationException.ErrorType }
                },
                NotFoundException notFoundException => new ApiResponse
                {
                    Success = false,
                    Message = notFoundException.Message,
                    Errors = new { ErrorType = notFoundException.ErrorType }
                },
                UnauthorizedException unauthorizedException => new ApiResponse
                {
                    Success = false,
                    Message = unauthorizedException.Message,
                    Errors = new { ErrorType = unauthorizedException.ErrorType }
                },
                _ => new ApiResponse
                {
                    Success = false,
                    Message = "Beklenmeyen bir hata oluştu",
                    Errors = new { ErrorType = "INTERNAL_SERVER_ERROR" }
                }
            };

            var statusCode = exception switch
            {
                ApiException apiException => apiException.StatusCode,
                ValidationException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
