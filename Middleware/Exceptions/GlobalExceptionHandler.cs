using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Middleware.Exceptions.types;
using System.Net;

namespace Middleware.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
               exception, "Exception occurred: {Message}", exception.Message);

            ProblemDetails result = GetProblemDetails(httpContext, exception);

            httpContext.Response.StatusCode = result.Status ?? (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken).ConfigureAwait(false);

            return true;
        }

        public ProblemDetails GetProblemDetails(
            HttpContext httpContext,
            Exception exception)
        {
            var statusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                BadRequestException or ValidationException or ArgumentNullException => (int)HttpStatusCode.BadRequest,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Type = exception.GetType().Name,
                Title = "An unexpected error occurred",
                Detail = _env.IsDevelopment() ? exception.Message : null,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };

            return problemDetails;
        }
    }
}
