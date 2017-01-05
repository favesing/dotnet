using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Builder.Filtering.Web
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
            app.Use(next => context => FilterAsync(context, next));

            app.Run(HelloWorldAsync);
        }

        public async Task FilterAsync(HttpContext context, RequestDelegate next)
        {
            var body = context.Response.Body;
            var buffer = new MemoryStream();
            context.Response.Body = buffer;
            try
            {
                context.Response.Headers["CustomHeader"] = "My Header";
                await context.Response.WriteAsync("Before\r\n");
                await next(context);
                await context.Response.WriteAsync("After\r\n");

                buffer.Position = 0;
                await buffer.CopyToAsync(body);
            }
            finally
            {
                context.Response.Body = body;
            }
        }

        public async Task HelloWorldAsync(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Hello world\r\n");
        }
    }
}
