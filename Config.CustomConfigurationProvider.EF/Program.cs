using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Config.CustomConfigurationProvider.EF
{
    public class ConfigurationValue
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }

    public class ConfigurationContext : DbContext
    {
        public ConfigurationContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ConfigurationValue> Values { get; set; }
    }

    public class EFConfigSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;
        public EFConfigSource(Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EFConfigProvider(_optionsAction);
        }
    }

    public class EFConfigProvider : ConfigurationProvider
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;
        public EFConfigProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }
        // load config data from EF db.
        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<ConfigurationContext>();
            _optionsAction(builder);

            using (var dbContext = new ConfigurationContext(builder.Options))
            {
                dbContext.Database.EnsureCreated();
                Data = !dbContext.Values.Any()
                    ? CreateAndSaveDefaultValues(dbContext)
                    : dbContext.Values.ToDictionary(n => n.Id, n => n.Value);
            }
        }

        private static IDictionary<string, string> CreateAndSaveDefaultValues(ConfigurationContext dbContext)
        {
            var configValues = new Dictionary<string, string>
            {
                { "key1", "value_from_ef_1"},
                { "key2", "value_from_ef_2"}
            };

            dbContext.Values.AddRange(configValues.Select(n => new ConfigurationValue { Id = n.Key, Value = n.Value }));
            dbContext.SaveChanges();

            return configValues;
        }
    }
    public static class EFExtentsions
    {
        public static IConfigurationBuilder AddEFConfig(this IConfigurationBuilder builder, Action<DbContextOptionsBuilder> setup)
        {
            return builder.Add(new EFConfigSource(setup));
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var connConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEFConfig(options =>
                {
                    options.UseSqlServer(connConfig.GetConnectionString("DefaultConnection"));
                })
                .Build();

            Console.WriteLine($"key1 = {config["key1"]}");
            Console.WriteLine($"key2 = {config["key2"]}");
            Console.WriteLine($"key3 = {config["key3"]}");

            Console.ReadKey();
        }
    }
}
