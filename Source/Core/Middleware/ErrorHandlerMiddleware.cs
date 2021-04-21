using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MultiLanguage.Core.Exception;
using MultiLanguage.Core.ViewModel.Exception;

namespace MultiLanguage.Core.Middleware
{
    public class ErrorHandlerMiddleware
    {
        public ErrorHandlerMiddleware(
            ILogger<ErrorHandlerMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (MultiLanguageException exception)
            {
                await BuildResponse(httpContext, exception, () => new ExceptionViewModel(exception));
            }
            catch (System.Exception exception)
            {
                await BuildResponse(httpContext, exception, () => new ExceptionViewModel(exception));
            }
        }

        private async Task BuildResponse(HttpContext httpContext, System.Exception exception, Func<ExceptionViewModel> createModel)
        {
            _logger.Log(LogLevel.Error, exception.ToString());

            httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var error = createModel.Invoke();

            var result = JsonSerializer.Serialize(error);
            await httpContext.Response.WriteAsync(result);
        }

        #region Field

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        #endregion
    }
}