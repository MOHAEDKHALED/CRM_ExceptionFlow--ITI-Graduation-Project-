using System.Net;
using System.Text.Json;
using CRM_ExceptionFlow.DTOs.Common;

namespace CRM_ExceptionFlow.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred. Request: {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = "An error occurred while processing your request.";

            // Handle specific exception types
            switch (exception)
            {
                case ArgumentException argEx:
                    code = HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    break;
                case InvalidOperationException invOpEx:
                    code = HttpStatusCode.BadRequest;
                    message = invOpEx.Message;
                    break;
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    message = "Unauthorized access.";
                    break;
                case KeyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    message = "Resource not found.";
                    break;
            }

            var response = new ErrorResponse
            {
                StatusCode = (int)code,
                Message = message,
                Details = context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true
                    ? exception.ToString()
                    : null
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}

