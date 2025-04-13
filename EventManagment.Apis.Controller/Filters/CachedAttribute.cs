using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace EventManagment.Apis.Controller.Filters
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timetolive;

        public CachedAttribute(int timetolive)
        {
            _timetolive = timetolive;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheservice = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cachkey = GenerateCacheKeyFromRequest(context.HttpContext);

            var cachresponse = await cacheservice.GetCachedResponseAsync(cachkey);
            if (!string.IsNullOrEmpty(cachresponse))
            {
                var result = new ContentResult()
                {
                    Content = cachresponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = result;

                return;
            }
            var executedcontext = await next.Invoke();
            if (executedcontext.Result is OkObjectResult objectResult && objectResult.Value is not null)
            {
                await cacheservice.CacheResponseAsync(cachkey, objectResult.Value, TimeSpan.FromSeconds(_timetolive));

            }




        }

        private string GenerateCacheKeyFromRequest(HttpContext httpContext)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(httpContext.Request.Path);

            foreach (var (key, value) in httpContext.Request.Query.OrderBy(x => x.Key))
            {
                KeyBuilder.Append($"|{key}-{value}");
            }
            return KeyBuilder.ToString();
        }
    }
}
