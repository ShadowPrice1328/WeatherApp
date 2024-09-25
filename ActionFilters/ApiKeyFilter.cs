using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Data;

namespace WeatherApp.ActionFilters
{
    public class ApiKeyFilter : IAsyncActionFilter
    {
        private readonly IApiKeyValidation _apiKeyValidation;

        public ApiKeyFilter(IApiKeyValidation apiKeyValidation)
        {
            _apiKeyValidation = apiKeyValidation;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string? apiKey = context.HttpContext.Request.Headers[Constants.ApiKeyHeaderName];

            if (string.IsNullOrWhiteSpace(apiKey) || !_apiKeyValidation.IsValidApiKey(apiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
