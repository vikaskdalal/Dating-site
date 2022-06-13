using DotNetCoreAngular.Errors;
using System.Net;
using System.Text.Json;

namespace DotNetCoreAngular.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this._next = next;
            this._logger = logger;
            this._env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace, ex.InnerException.ToString())
                    : new ApiException(context.Response.StatusCode, "Internal Server error");

                var jsonResponse = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
