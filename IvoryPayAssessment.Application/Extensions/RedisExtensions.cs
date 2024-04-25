using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IvoryPayAssessment.Application.Extensions
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("RedisSettings").GetValue<string>("Configuration");
                options.InstanceName = Configuration.GetSection("RedisSettings").GetValue<string>("InstanceName");
            });
            return services;
        }
    }
}
