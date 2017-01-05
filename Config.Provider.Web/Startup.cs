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

namespace Config.Provider.Web
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
            builder.AddIniFile("Config.Providers.ini");
            builder.AddEnvironmentVariables();
            var config = builder.Build();

            app.Run(async ctx =>
            {
                ctx.Response.ContentType = "text/plain";

                Func<String, String> formatKeyValue = key => "[" + key + "] " + config[key] + "\r\n\r\n";
                await ctx.Response.WriteAsync(formatKeyValue("Services:One.Two"));
                await ctx.Response.WriteAsync(formatKeyValue("Services:One.Two:Six"));
                await ctx.Response.WriteAsync(formatKeyValue("Data:DefaultConnecection:ConnectionString"));
                await ctx.Response.WriteAsync(formatKeyValue("Data:DefaultConnecection:Provider"));
                await ctx.Response.WriteAsync(formatKeyValue("Data:Inventory:ConnectionString"));
                await ctx.Response.WriteAsync(formatKeyValue("Data:Inventory:Provider"));
                await ctx.Response.WriteAsync(formatKeyValue("PATH"));
                await ctx.Response.WriteAsync(formatKeyValue("COMPUTERNAME"));
            });
        }
    }
}
