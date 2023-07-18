using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace WebAPIDemo.Filters
{
    public class RateLimitFilter : IAsyncActionFilter
    {
        private readonly IMemoryCache _memCache;

        public RateLimitFilter(IMemoryCache memCache)
        {
            _memCache = memCache;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string? remoteIP = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            string cacheKey = $"LastVisitTick_{remoteIP}";
            long? lastTick = _memCache.Get<long?>(cacheKey);
            if (lastTick.HasValue && Environment.TickCount64 - lastTick.Value < 1000)
            {
                context.Result = new ContentResult { StatusCode = 429,Content="访问太频繁" };
                return Task.CompletedTask;
            }
            else 
            {
                _memCache.Set(cacheKey,Environment.TickCount64,TimeSpan.FromSeconds(10));
                return next();
            }
        }
    }
}
