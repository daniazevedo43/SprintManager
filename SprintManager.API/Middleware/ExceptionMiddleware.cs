using SprintManager.Application.Exceptions;
using SprintManager.Exceptions.ExceptionsBase;
using System.Net;
using System.Text.Json;

namespace SprintManager.API.Middleware
{
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
                _logger.LogError($"Exception: {ex.Message}, StackTrace: {ex.StackTrace}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Customize status code based on exception type
            context.Response.StatusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest, // 400
                SprintManagerTooShortException => (int)HttpStatusCode.BadRequest, // 400
                SprintManagerTooLongException => (int)HttpStatusCode.BadRequest, // 400
                SprintManagerInvalidDateRangeException => (int)HttpStatusCode.BadRequest, // 400
                SprintManagerDateNotAllowedException => (int)HttpStatusCode.BadRequest, // 400 
                SprintManagerInvalidUsernameException => (int)HttpStatusCode.BadRequest, // 400 
                SprintManagerNotFoundException => (int)HttpStatusCode.NotFound, // 404
                SprintManagerConflictException => (int)HttpStatusCode.Conflict, // 409
                SprintManagerFileNotAllowedException => (int)HttpStatusCode.UnsupportedMediaType, // 415
                _ => (int)HttpStatusCode.InternalServerError // 500
            };

            var response = new
            {
                context.Response.StatusCode, // Return the status code
                exception.Message // Return the exception message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}