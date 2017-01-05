using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Config.OptionsSnapshot
{
    public class TimeOptions
    {
        public TimeOptions()
        { }
        //Records the time when options are created.
        public DateTime CreationTime { get; set; } = DateTime.Now;
        // Bound to config. Changes to the value of "Message"
        // in config.json will be reflected in this property.
        public string Message { get; set; }
    }

    public class Controller
    {
        public readonly TimeOptions _options;
        public Controller(IOptionsSnapshot<TimeOptions> options)
        {
            this._options = options.Value;
        }

        public Task DisplayTimeAsync(HttpContext context)
        {
            return context.Response.WriteAsync(_options.Message + _options.CreationTime);
        }
    }
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional:false, reloadOnChange:true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Controller>();

            services.Configure<TimeOptions>(Configuration.GetSection("Time"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.Run(context => 
            {
                context.Response.ContentType = "text/plain";
                return context.RequestServices.GetRequiredService<Controller>().DisplayTimeAsync(context);
            });
        }
    }
}
