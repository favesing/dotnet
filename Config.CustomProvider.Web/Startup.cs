using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Config.CustomProvider.Web
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
            var builder = new ConfigurationBuilder();
            builder.Add(new MyConfigSource());

            var config = builder.Build();

            app.Run(async ctx => 
            {
                ctx.Response.ContentType = "text/plain";
                await DumpConfig(ctx.Response, config);
            });
        }

        private static async Task DumpConfig(HttpResponse response, IConfiguration config, string indentation = "")
        {
            foreach (var child in config.GetChildren())
            {
                await response.WriteAsync($"{indentation}[{child.Key}] = [{config[child.Key]}]\r\n");
                await DumpConfig(response, child, indentation + "  ");
            }
        }
    }
}
