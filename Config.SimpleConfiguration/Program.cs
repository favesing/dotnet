using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Config.SimpleConfiguration
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Console.WriteLine($"option1 = {Configuration["option1"]}");
            Console.WriteLine($"option2 = {Configuration["option2"]}");
            Console.WriteLine($"option1 = {Configuration["subsection:suboption1"]}");

            Console.ReadKey();
        }
    }
}
