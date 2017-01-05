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
using Microsoft.Extensions.Configuration.Memory;

namespace Config.SettingObject.Web
{
    public class MySettings
    {
        public MySettings()
        {
            RetryCount = 3;
            AdBlocks = new Dictionary<string, AdBlock>(StringComparer.OrdinalIgnoreCase);
        }

        public int RetryCount { get; set; }
        public string DefaultAdBlock { get; set; }
        public IDictionary<string, AdBlock> AdBlocks { get; private set; }

        public void Read(IConfiguration configuration)
        {
            var value = configuration["RetryCount"];
            if (!string.IsNullOrEmpty(value))
            {
                RetryCount = int.Parse(value);
            }
            value = configuration["DefaultAdBlock"];
            if (!string.IsNullOrEmpty(value))
            {
                DefaultAdBlock = value;
            }

            var items = new List<AdBlock>();
            foreach (var subConfig in configuration.GetSection("AdBlock").GetChildren())
            {
                var item = new AdBlock { Name = subConfig.Key };
                value = subConfig["Origin"];
                if (!string.IsNullOrEmpty(value))
                {
                    item.Origin = value;
                }
                value = subConfig["ProductCode"];
                if (!string.IsNullOrEmpty(value))
                {
                    item.ProductCode = value;
                }
                items.Add(item);
            }
            AdBlocks = items.ToDictionary(
                item => item.Name,
                item => item,
                StringComparer.OrdinalIgnoreCase);
        }

        public class AdBlock
        {
            public string Name { get; set; }
            public string ProductCode { get; set; }
            public string Origin { get; set; }
        }
    }
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
            List<KeyValuePair<string, string>> data = new Dictionary<string, string>
            {
                { "MySettings:RetryCount", "42"},
                { "MySettings:DefaultAdBlock", "House"},
                { "MySettings:AdBlock:House:ProductCode", "123"},
                { "MySettings:AdBlock:House:Origin", "blob-456"},
                { "MySettings:AdBlock:Contoso:ProductCode", "contoso2014"},
                { "MySettings:AdBlock:Contoso:Origin", "sql-789"},
            }.ToList();

            builder.Add(new MemoryConfigurationSource { InitialData = data });
            var config = builder.Build();

            var mySettings = new MySettings();
            mySettings.Read(config.GetSection("MySettings"));

            app.Run(async ctx =>
            {
                ctx.Response.ContentType = "text/plain";

                await ctx.Response.WriteAsync(string.Format("Retry Count {0}\r\n", mySettings.RetryCount));
                await ctx.Response.WriteAsync(string.Format("Default Ad Block {0}\r\n", mySettings.DefaultAdBlock));
                foreach (var adBlock in mySettings.AdBlocks.Values)
                {
                    await ctx.Response.WriteAsync(string.Format(
                        "Ad Block {0} Origin {1} Product Code {2}\r\n",
                        adBlock.Name, adBlock.Origin, adBlock.ProductCode));
                }
            });
        }
    }
}
