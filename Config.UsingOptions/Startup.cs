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

namespace Config.UsingOptions
{
    public class MyOptions
    {
        public MyOptions()
        {
            Option1 = "value1_from_ctor";
        }

        public string Option1 { get; set; }
        public int Option2 { get; set; } = 5;
    }

    public class MySubOptions
    {
        public MySubOptions()
        {
            SubOption1 = "value1_from_ctor";
            SubOption2 = 5;
        }
        public string SubOption1 { get; set; }
        public int SubOption2 { get; set; }
    }

    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<MyOptions>(Configuration);

            services.Configure<MyOptions>(options => 
            {
                options.Option1 = "value1_from_action";
            });

            services.Configure<MySubOptions>(Configuration.GetSection("subsection"));

            services.AddMvc();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc(routes => 
            {
                routes.MapRoute(
                    name:"default",
                    template:"{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}
