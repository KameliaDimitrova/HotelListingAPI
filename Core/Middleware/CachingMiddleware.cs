using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace HotelListingAPI.Middleware;

public class CachingMiddleware
{
    private readonly RequestDelegate next;

    public CachingMiddleware(RequestDelegate next) 
        => this.next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(10)
        };
        context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };

        await this.next(context);
    }
}
