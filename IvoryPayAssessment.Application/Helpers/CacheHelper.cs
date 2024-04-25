using Microsoft.Extensions.Caching.Memory;

namespace IvoryPayAssessment.Application.Helpers
{
    public class CacheHelper
    {
        private readonly IMemoryCache _cache;
        public CacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task SetMemory(string email)
        {

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(60))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(3600))
                    .SetPriority(CacheItemPriority.Normal);
            //  .SetSize(1024);
            _cache.Set(email, true, cacheEntryOptions);
            await Task.CompletedTask;
        }

        public async Task<bool> GetMemoryValue(string email)
        {

            var data = _cache.TryGetValue<bool>(email, out bool result);
            return await Task.FromResult(result );
        }
        public async Task<bool> RemoveCachedValue(string email)
        {

            var data = _cache.TryGetValue<bool>(email, out bool result);
            if (result)
            {
                _cache.Remove(email);
                return await Task.FromResult(true);

            }
            return await Task.FromResult(false);
        }
    }
}
