using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Middleware.Logging
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await LogRequestAsync(context);

                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context); // Continue down the pipeline

                    await LogResponseAsync(context);

                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            catch
            {
                // Rethrow the exception to ensure it's caught by the global exception handler
                throw;
            }
        }

        private async Task LogRequestAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            var request = context.Request;
            var requestBodyContent = await ReadStreamAsync(request.Body);

            _logger.LogInformation("Incoming Request: {method} {url} {headers} {body}",
                request.Method,
                request.Path,
                request.Headers.ToString(),
                requestBodyContent);

            request.Body.Position = 0;
        }

        private async Task LogResponseAsync(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("Outgoing Response: {statusCode} {headers} {body}",
                context.Response.StatusCode,
                context.Response.Headers.ToString(),
                responseBodyContent);
        }

        private async Task<string> ReadStreamAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                var result = await reader.ReadToEndAsync();
                stream.Seek(0, SeekOrigin.Begin);
                return result;
            }
        }
    }
}
