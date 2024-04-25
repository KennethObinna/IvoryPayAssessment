using IvoryPayAssessment.Application.Common.Constants.ErrorBuldles;
using IvoryPayAssessment.Application.Common.Helpers;
using IvoryPayAssessment.Application.Interfacses.UserAccounts;
using IvoryPayAssessment.Application.Interfacses.UserSessions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace IvoryPayAssessment.Application.Common.Filters
{
    public class SessionFilter : IAsyncActionFilter
    {

        private readonly IMessageProvider _messageProvider;
        private readonly ILogger<SessionFilter> _logger;
        private readonly ISessionsService _sessionsService;
        private readonly IConfiguration _config;

        private readonly IAccountLogInService _accountLogIn;
        private readonly AllowableActionmethods allowedActionmethodsWithoutAuthorization;
        public SessionFilter(IMessageProvider messageProvider, ILogger<SessionFilter> logger,
            ISessionsService sessionsService, IConfiguration config, IOptions<AllowableActionmethods> opt, IAccountLogInService accountLogIn)
        {
            _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionsService = sessionsService;
            _config = config;
            allowedActionmethodsWithoutAuthorization = opt.Value;
            _accountLogIn = accountLogIn;
            //  allowedActionmethodsWithoutAuthorization =


        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            bool hasLanguage = context.HttpContext.Request.Headers.TryGetValue(ResponseCodes.LANGUAGE, out var language);

            var action = context.RouteData.Values["action"].ToString();

         

            var actions = allowedActionmethodsWithoutAuthorization.Actions?.ToList();

            if (!actions.Any())
            {
                var act = _config.GetSection("AllowableActionmethods:Actions");
                actions = act.Get<List<string>>();
            }

            var checkAction = actions.Any(p => p.ToString().ToLower() == action.ToLower());

            if (checkAction)
            {

                await next();
            }
            else
            {
                var myToken = context.HttpContext.GetTokenAsync("access_token")?.GetAwaiter().GetResult();

                
                if(myToken is null)
                {

                    context.Result = new ObjectResult(
                                       new ErrorResponse
                                       {
                                           responseCode = ResponseCodes.INVALID_TOKEN,
                                           responseDescription = _messageProvider.GetMessage(ResponseCodes.UNAUTHORIZED, language)
                                       })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }
                bool validToken = TokenHelper.IsValid(myToken);

                if (!validToken)
                {

                    context.Result = new ObjectResult(
                                       new ErrorResponse
                                       {
                                           responseCode = ResponseCodes.INVALID_TOKEN,
                                           responseDescription = _messageProvider.GetMessage(ResponseCodes.UNAUTHORIZED, language)
                                       })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }
                if (myToken == null)
                {

                    context.Result = new ObjectResult(
                                       new ErrorResponse
                                       {
                                           responseCode = ResponseCodes.UNAUTHORIZED,
                                           responseDescription = _messageProvider.GetMessage(ResponseCodes.UNAUTHORIZED, language)
                                       })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }

                var userExists = await _sessionsService.IsSessionValid();

                if (userExists)
                {
                    await next();
                }
                else
                {
                    context.Result = new ObjectResult(
                                       new ErrorResponse
                                       {
                                           responseCode = ResponseCodes.UNAUTHORIZED,
                                           responseDescription = _messageProvider.GetMessage(ResponseCodes.UNAUTHORIZED, language)
                                       })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }

            }






        }
    }
}
