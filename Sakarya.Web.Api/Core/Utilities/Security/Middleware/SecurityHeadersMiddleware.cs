using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Utilities.Security.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("X-Frame-Options", "DENY");
            httpContext.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            //httpContext.Response.Headers.Add(
            //    "Content-Security-Policy",
            //    "default-src 'self'; " +
            //    "img-src 'self' {your_awesome_url}; " +
            //    "font-src 'self'; " +
            //    "style-src 'self'; " +
            //    "script-src 'self' 'nonce-{your_awesome_key}' " +
            //    " 'nonce-{your_awesome_key}'; " +
            //    "frame-src 'self';" +
            //    "connect-src 'self';");
            httpContext.Response.Headers.Add("Referrer-Policy", "no-referrer");
            httpContext.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none';" +
                                                               " geolocation 'none'; gyroscope 'none'; " +
                                                               "magnetometer 'none'; microphone 'none'; " +
                                                               "payment 'none'; usb 'none'");

            await next(httpContext);
        }
    }
}