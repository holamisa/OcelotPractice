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

            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);

            return true;
        }

        public ProblemDetails GetProblemDetails(
            HttpContext httpContext,
            Exception exception)
        {
            ProblemDetails result = new ProblemDetails();

            switch (exception)
            {
                case NotFoundException notFoundException:
                    result = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.Unauthorized,
                        Type = notFoundException.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = _env.IsDevelopment() ? notFoundException.Message : null,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    break;
                case BadRequestException badRequestException:
                    result = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Type = badRequestException.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = _env.IsDevelopment() ? badRequestException.Message : null,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    break;
                case ValidationException validationException:
                    result = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Type = validationException.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = _env.IsDevelopment() ? validationException.Message : null,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    break;
                case ArgumentNullException argumentNullException:
                    result = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Type = argumentNullException.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = _env.IsDevelopment() ? argumentNullException.Message : null,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    break;
                case UnauthorizedException unauthorizedException:
                    result = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.Unauthorized,
                        Type = unauthorizedException.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = _env.IsDevelopment() ? unauthorizedException.Message : null,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    break;
                default:
                    result = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Type = exception.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = _env.IsDevelopment() ? exception.Message : null,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                    };
                    _logger.LogError(exception, $"Exception occured : {exception.Message}");
                    break;
            }

            return result;
        }
    }
}
