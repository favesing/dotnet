using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extentions.Configuration.ConfigFile
{
    public static class ConfigurationExtensions
    {
        public const string AppSettings = "appsettings";

        public static string GetAppSetting(this IConfiguration configuration, string name)
        {
            return configuration?.GetSection(AppSettings)[name];
        }
        /// <summary>
        /// Gets all of the configuration sections for a set of keys
        /// </summary>
        /// <param name="sectionNames">The names of the sections from the top-most level to lowest</param>
        public static ImmutableDictionary<string, IConfigurationSection> GetSection(this IConfiguration configuration, params string[] sectionNames)
        {
            if (sectionNames.Length == 0)
                return ImmutableDictionary<string, IConfigurationSection>.Empty;

            var fullkey = string.Join(ConfigurationPath.KeyDelimiter, sectionNames);

            return configuration?.GetSection(fullkey).GetChildren()?.ToImmutableDictionary(x => x.Key, x => x);
        }

        /// <summary>
        /// Given a set of keys, will join them and get the value at that level
        /// </summary>
        /// <param name="keys">Names of the sections from top-level to lowest level</param>
        /// <returns>The value of that key</returns>
        public static string GetValue(this IConfiguration configuration, params string[] keys)
        {
            if (keys.Length == 0)
            {
                throw new ArgumentException("Need to provide keys", nameof(keys));
            }

            var fullKey = string.Join(ConfigurationPath.KeyDelimiter, keys);

            return configuration?[fullKey];
        }
    }
}
