using Microsoft.AspNetCore.Mvc.Filters;

namespace IvoryPayAssessment.Application.Common.Filters
{
    public class AuthorizationFilter : IAsyncActionFilter
    {

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
