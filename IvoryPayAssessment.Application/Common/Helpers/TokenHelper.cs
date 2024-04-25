using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.Helpers
{
    public class TokenHelper
    {

        public static string GetToken(IHttpContextAccessor httpContext)
        {
            var token = httpContext.HttpContext.GetTokenAsync("access_token")?.GetAwaiter().GetResult();
            
            return token;
        }
        public static bool HasToken(ActionExecutingContext httpContext, out string output)
        {
            var token = httpContext.HttpContext.GetTokenAsync("access_token")?.GetAwaiter().GetResult();
             
            output = token;
            return !string.IsNullOrWhiteSpace(token);
        }
        public static bool IsValid(string token)
        {
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(token);
            }
            catch (Exception)
            {
                return false;
            }

            return jwtSecurityToken.ValidTo > DateTime.UtcNow;
        }
    }
}
