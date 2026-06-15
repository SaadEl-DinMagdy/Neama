using Neama.Api.Errors;
using System.Text.Json;

namespace Neama.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {

            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";

                var Respons = _env.IsDevelopment() ?
                    new ApiExceptionResponse(ex.Message, ex.StackTrace) : new ApiExceptionResponse();

                var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(Respons, option);

                await httpContext.Response.WriteAsync(json);

            }

        }
    }
}
