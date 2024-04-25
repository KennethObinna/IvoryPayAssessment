



using IvoryPayAssessment.Application.Common.Constants.ErrorBuldles;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace IvoryPayAssessment.Application.Common.Filters
{
    public class LanguageFilter : IAsyncActionFilter
    {
        private readonly IMessageProvider _messageProvider;
        private readonly ILogger<LanguageFilter> _logger;
        public LanguageFilter(IMessageProvider messageProvider, ILoggerFactory logger)
        {
            _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
            _logger = logger.CreateLogger<LanguageFilter>() ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool hasLanguage = context.HttpContext.Request.Headers.TryGetValue(ResponseCodes.LANGUAGE,out var language);
            if (hasLanguage)
            {
              await  next();
            }
            else
            {
                context.Result = new ObjectResult(
                    new ErrorResponse
                    {
                        responseCode = ResponseCodes.REQUIRE_COUNTRY_FLAG,
                        responseDescription = _messageProvider.GetMessage(ResponseCodes.REQUIRE_COUNTRY_FLAG, ResponseCodes.DEFAULT_LANGUAGE)
                    })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            return;
            }
        }
    }
}
