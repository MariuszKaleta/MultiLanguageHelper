using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiLanguage.Core.ViewModel.Validation;

namespace MultiLanguage.Core.Middleware
{
    public class ValidationFilter : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var httpContext = context.HttpContext;
                httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                httpContext.Response.ContentType = "application/json";

                var result = new ValidationResultViewModel();

                foreach (var (parameterName, value) in context.ModelState)
                    result.Errors.Add(new FieldValidationResultViewModel(parameterName, value));

                var text = JsonSerializer.Serialize(result);
                await httpContext.Response.WriteAsync(text);
            }

            await Task.CompletedTask;
        }
    }
}