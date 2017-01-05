using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Container.Fallback.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICall, CallOne>();
            services.AddScoped<ICall, CallTwo>();
            services.AddTransient<ICall, CallThree>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware(typeof(MyMiddleware));

            app.Run(async context =>
                await context.Response.WriteAsync("---------- Done\r\n"));
        }
    }

    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<ICall> _calls;
        public MyMiddleware(RequestDelegate next, IEnumerable<ICall> calls)
        {
            _next = next;
            _calls = calls;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            await context.Response.WriteAsync("---------- MyMiddleware ctor\r\n");
            await context.Response.WriteAsync(_calls.Aggregate("", (a, b) => a + b.Text + "\r\n"));

            var requestContainerCalls = context.RequestServices.GetService<IEnumerable<ICall>>();
            await context.Response.WriteAsync("---------- context.RequestServices\r\n");
            await context.Response.WriteAsync(requestContainerCalls.Aggregate("", (a, b) => a + b.Text + "\r\n"));

            await _next(context);
        }
    }

    public interface ICall
    {
        string Text { get; }
    }

    public abstract class CallBase : ICall
    {
        private static int _lastNumber;
        private readonly int _number;

        public CallBase()
        {
            _number = Interlocked.Increment(ref _lastNumber);
        }

        public string Text
        {
            get { return GetType().Name + "[" + _number + "]"; }
        }
    }

    public class CallOne : CallBase
    {
    }

    public class CallTwo : CallBase
    {
    }

    public class CallThree : CallBase
    {
    }
}
