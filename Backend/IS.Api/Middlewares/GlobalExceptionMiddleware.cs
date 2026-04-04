using IS.Domain.Exceptions;
using IS.Shared.Constants;
using IS.Shared.Wrappers;
using System.Net;

namespace IS.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            var (statusCode, message) = exception switch
            {
                NotFoundException => (HttpStatusCode.NotFound, exception.Message),
                BusinessException => (HttpStatusCode.BadRequest, exception.Message),
                _ => (HttpStatusCode.InternalServerError, ErrorMessages.UnexpectedError)
            };

            _logger.LogError(
                exception,
                "{Method} {Path} responded {StatusCode} - {Message}",
                context.Request.Method,
                context.Request.Path,
                (int)statusCode,
                exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = ApiResponse<object>.Fail(message);
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}