using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCaching;
using System;
using System.Threading.Tasks;

namespace TodoApi.Middleware
{
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;

        public CachingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Sample of global cache for any request that contains in QueryString "Param1"
            // Note that the middleware ignores requests doesn't return 200 (OK)
            context.Response.GetTypedHeaders().CacheControl =
            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(10)
            };

            var responseCachingFeature = context.Features.Get<IResponseCachingFeature>();
            if (responseCachingFeature != null)
            {
                responseCachingFeature.VaryByQueryKeys = new[] { "Param1" };
            }
            await _next(context);
        }
    }
}