using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace IvoryPayAssessment.Application.Common.Filters
{

    public class RateLimitingMiddleware
    {

        private readonly IConfiguration _config;
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, TokenBucket> _buckets;
        private readonly AllowableActionmethods _allowedActionmethodsWithoutAuthorization;
        private readonly ILogger _logger;
        public RateLimitingMiddleware(RequestDelegate next, IOptions<AllowableActionmethods> opt, IConfiguration config, ILoggerFactory logger)
        {
            _next = next;
            _buckets = new ConcurrentDictionary<string, TokenBucket>();
            _config = config;
            _allowedActionmethodsWithoutAuthorization = opt.Value;
            _logger = logger.CreateLogger<RateLimitingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var action = context.Request.RouteValues["action"].ToString();

            var actions = _allowedActionmethodsWithoutAuthorization.Actions?.ToList();

            if (!actions.Any())
            {
                var act = _config.GetSection("AllowableActionmethods:Actions");
                actions = act.Get<List<string>>();
            }

            var checkAction = actions.Any(p => p.ToString().ToLower() == action.ToLower());

            if (checkAction)
            {
                await _next(context);
            }
            else
            {
                var clientId = Convert.ToString(context.Request.Headers["Authorization"]);

                if(clientId == null)
                { context.Response.StatusCode =  StatusCodes.Status204NoContent;  
                    _logger.LogError("Rate limit exceeded");
                    await context.Response.WriteAsync("Rate limit exceeded");

                }
                var bucket = _buckets.GetOrAdd(clientId, id => new TokenBucket(capacity: 100, refillRate: 10)); // Adjust capacity and refill rate as needed

                if (bucket.TryConsume())
                {
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = 429; // Too Many Requests

                    _logger.LogError("Rate limit exceeded");
                    await context.Response.WriteAsync("Rate limit exceeded");
                }
            }

        }
    }

    public class TokenBucket
    {
        private readonly int _capacity;
        private readonly int _refillRate;
        private long _tokens;
        private long _lastRefillTime;

        public TokenBucket(int capacity, int refillRate)
        {
            _capacity = capacity;
            _refillRate = refillRate;
            _tokens = capacity;
            _lastRefillTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public bool TryConsume()
        {
            Refill();

            lock (this)
            {
                if (_tokens >= 1)
                {
                    _tokens--;
                    return true;
                }

                return false;
            }
        }

        private void Refill()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var timePassed = now - _lastRefillTime;
            var tokensToAdd = timePassed * _refillRate;
            _tokens = Math.Min(_capacity, _tokens + tokensToAdd);
            _lastRefillTime = now;
        }
    }

}
