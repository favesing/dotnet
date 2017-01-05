using Microsoft.Extensions.Configuration;
using Microsoft.Extentions.Configuration.ConfigFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Config.CustomConfigurationProvider.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddConfigFile("Web.config");

            var configuration = builder.Build();

            Console.WriteLine("---------- AppSettings ----------");
            Console.WriteLine($"PreserveLoginUrl: [{configuration.GetAppSetting("PreserveLoginUrl")}]");
            Console.WriteLine($"ClientValidationEnabled: [{configuration.GetAppSetting("ClientValidationEnabled")}]");
            Console.WriteLine($"UnobtrusiveJavaScriptEnabled: [{configuration.GetAppSetting("UnobtrusiveJavaScriptEnabled")}]");

            Console.WriteLine("---------- ConnectionStrings ----------");
            foreach (var kvp in configuration.GetSection("ConnectionStrings").GetChildren())
            {
                Console.WriteLine($"{kvp.Key}: [{kvp.Value}]");
            }

            Console.WriteLine("---------- configNode:nestedNode ---------- ");
            foreach (var kvp in configuration.GetSection("configNode", "nestedNode"))
            {
                Console.WriteLine($"{kvp.Key}: [{kvp.Value.Value}]");
            }

            Console.WriteLine("---------- Specific Key ---------- ");
            var value = configuration.GetValue("sampleSection", "setting2");
            Console.WriteLine($"KEY: sampleSection:setting2, VALUE: {value}");

            Console.WriteLine("Press ENTER to exit program...");
            Console.ReadLine();
        }
    }
}
