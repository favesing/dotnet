using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Builder.Middleware.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseXHttpHeaderOverride();

            app.UseMiddleware<MyMiddleware>("Yo");
        }
    }

    public static class BuilderExtentions
    {
        public static IApplicationBuilder UseXHttpHeaderOverride(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<XHttpHeaderOverrideMiddleware>();
        }
    }

    public class XHttpHeaderOverrideMiddleware
    {
        private readonly RequestDelegate _next;
        public XHttpHeaderOverrideMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var headerValue = context.Request.Headers["X-HTTP-Method-Override"];
            var queryValue = context.Request.Query["X-HTTP-Method-Override"];
            if (!string.IsNullOrEmpty(headerValue))
            {
                context.Request.Method = headerValue;
            }
            else if (!string.IsNullOrEmpty(queryValue))
            {
                context.Request.Method = queryValue;
            }
            return _next.Invoke(context);
        }
    }

    public class MyMiddleware
    {
        private RequestDelegate _next;
        private string _greeting;
        private IServiceProvider _services;

        public MyMiddleware(RequestDelegate next, string greeting, IServiceProvider services)
        {
            _next = next;
            _greeting = greeting;
            _services = services;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync(_greeting + ", middleware!\r\n");
            await context.Response.WriteAsync("This request is a " + context.Request.Method + "\r\n");
        }
    }
}
