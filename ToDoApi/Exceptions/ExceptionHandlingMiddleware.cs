using System.Net;
using System.Text.Json;

namespace ToDoApi.Exceptions
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An unexpected error occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is TodoItemAlreadyExistsException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            
                var result = JsonSerializer.Serialize(new
                {
                    message = exception.Message
                });

                return context.Response.WriteAsync(result);
            }

            

            var defaultResult = JsonSerializer.Serialize(new
            {
                message = "An unexpected error occurred. Please try again later."
            });

            return context.Response.WriteAsync(defaultResult);
        }


    }
}
