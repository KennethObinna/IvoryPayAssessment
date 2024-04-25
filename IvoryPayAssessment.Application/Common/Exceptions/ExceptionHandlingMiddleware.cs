

namespace IvoryPayAssessment.Application.Common.Exceptions
{

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
     
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;


        }

        public async Task InvokeAsync(HttpContext httpContext, IMessageProvider messageProvider, IAppDbContext context)
        {
             
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, messageProvider);
                

            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IMessageProvider messageProvider)
          {
            var getLanguage = Convert.ToString(context.Request.Headers[ResponseCodes.LANGUAGE]);
            context.Response.ContentType = "Application/json";

            var response = context.Response;

            var message = string.Empty;

            var errorResponse = new ErrorResponse
            {
                responseDescription = messageProvider.GetMessage(ResponseCodes.EXCEPTION, getLanguage),
                responseCode = ResponseCodes.EXCEPTION

            };
            CaseSwirching(exception, messageProvider, getLanguage, response, errorResponse);
            var result = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(result);
        }

        private void CaseSwirching(Exception exception, IMessageProvider messageProvider, string getLanguage, HttpResponse response, ErrorResponse errorResponse)
        {
            switch (exception)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.responseDescription = messageProvider.GetMessage(ResponseCodes.INVALID_TOKEN, getLanguage);
                        errorResponse.responseCode = ResponseCodes.INVALID_TOKEN;
                        _logger.LogError(ex, "Invalid token");
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.responseDescription = messageProvider.GetMessage(ResponseCodes.BAD_REQUEST, getLanguage);
                    errorResponse.responseCode = ResponseCodes.BAD_REQUEST;
                    _logger.LogError(ex, "Bad request");
                    break;
                case KeyNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.responseDescription = messageProvider.GetMessage(ResponseCodes.NOT_FOUND, getLanguage);
                    errorResponse.responseCode = ResponseCodes.NOT_FOUND;
                    _logger.LogError(ex, "Not found");
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.responseDescription = messageProvider.GetMessage(ResponseCodes.EXCEPTION, getLanguage);
                    errorResponse.responseCode = ResponseCodes.EXCEPTION;
                    _logger.LogError("An error occurred");
                    break;
            }
            _logger.LogError(exception.Message);
        }
    }
}
