using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Config.BindToObjectGraph
{
    public class AppOptions
    {
        public Window Window { get; set; }
        public Connection Connection { get; set; }
        public Profile Profile { get; set; }
    }

    public class Window
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class Connection
    {
        public string Value { get; set; }
    }

    public class Profile
    {
        public string Machine { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var appConfig = new AppOptions();
            config.GetSection("App").Bind(appConfig);

            appConfig = config.GetSection("App").Get<AppOptions>();

            Console.WriteLine($"Height {appConfig.Window.Height}");

            Console.ReadKey();
        }
    }
}
