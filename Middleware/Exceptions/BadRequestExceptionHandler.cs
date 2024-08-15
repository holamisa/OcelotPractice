using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Middleware.Exceptions
{
    public class BadExceptionHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<BadExceptionHandler> _logger;

        public BadExceptionHandler(IHostEnvironment env, ILogger<BadExceptionHandler> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Bad Exception");
            
            if (exception is not BadHttpRequestException badRequestException)
            {
                return false;
            }

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Bad request error occurred",
                Detail = _env.IsDevelopment() ? exception.Message : null,
            }, cancellationToken: cancellationToken);
            return true;
        }
    }
}
