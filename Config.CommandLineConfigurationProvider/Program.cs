using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Config.CommandLineConfigurationProvider
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static Dictionary<string, string> GetSwitchMappings(IReadOnlyDictionary<string, string> configurationStrings)
        {
            return configurationStrings.Select(item => 
            {
                var key = "-" + item.Key.Substring(item.Key.LastIndexOf(":") + 1);
                var val = item.Key;
                return new KeyValuePair<string, string>(key, val);
            }).ToDictionary(item => item.Key, item => item.Value);
        }
        public static void Main(string[] args)
        {
            var dict = new Dictionary<string, string>
            {
                { "Profile:MachineName", "Rick"},
                { "App:MainWindow:Left", "11"}
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(dict)
                //dotnet run /Profile:MachineName=zhaorui /App:MainWindow:Left=20 or
                //dotnet run -MachineName=zhaorui -Left=20 for switchMapping
                .AddCommandLine(args, GetSwitchMappings(dict));

            Configuration = builder.Build();

            Console.WriteLine($"Hello {Configuration["Profile:MachineName"]}");
            Console.WriteLine($"Left : {Configuration.GetValue<int>("App:MainWindow:Left", 80)}");

            Console.ReadKey();
        }
    }
}
