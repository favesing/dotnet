using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Config.InMemory
{
    public class MyWindow
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
    }

    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static void Main(string[] args)
        {
            var dict = new Dictionary<string, string>
            {
                { "Profile:MachineName", "Rick"},
                { "App:MainWindow:Height", "11"},
                { "App:MainWindow:Width", "11"},
                { "App:MainWindow:Top", "11"},
                { "App:MainWindow:Left", "11"}
            };

            var builder = new ConfigurationBuilder();

            builder.AddInMemoryCollection(dict);

            Configuration = builder.Build();

            Console.WriteLine($"Hello {Configuration["Profile:MachineName"]}");

            var window = new MyWindow();
            Console.WriteLine("--- In memory Bind To Instance ---");
            Configuration.GetSection("App:MainWindow").Bind(window);

            Console.WriteLine($"Height:{window.Height}, Width:{window.Width}, Top:{window.Top}, Left:{window.Left}");

            Console.WriteLine($"GetValue -> Height: {Configuration.GetValue<int>("App:MainWindow:Height")}");

            Console.ReadKey();
        }
    }
}
