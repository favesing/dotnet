using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace Error.DeveloperExceptionPage
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            env.EnvironmentName = EnvironmentName.Production;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //config an exception handler page to use when app is not running in "development" env.
                app.UseExceptionHandler("/error");
                app.UseExceptionHandler(options => 
                {
                    options.Run(async context => 
                    {
                        if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                        {
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "text/html";
                            await context.Response.SendFileAsync($"{env.WebRootPath}/errors/500.html");
                        }
                    });
                });
            }
            //return "Status Code: 404; Not Found"
            //app.UseStatusCodePages();

            //return 302 and redirect error hander
            //app.UseStatusCodePagesWithRedirects("/error/{0}");

            //execute error hander with status code and return 404
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            //var feature = app.ServerFeatures.Get<IStatusCodePagesFeature>();
            //if (feature != null)
            //{
            //    feature.Enabled = false;
            //}

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Run(async (context) =>
            //{
            //    if (context.Request.Query.ContainsKey("throw"))
            //    {
            //        throw new Exception("throw Exception!");
            //    }

            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
