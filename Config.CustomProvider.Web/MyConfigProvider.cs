using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Config.CustomProvider.Web
{
    public class MyConfigProvider : IConfigurationProvider
    {
        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }

        public void Load()
        {
            // no loading or reloading, this source is all hardcoded
        }

        public void Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(string key, out string value)
        {
            switch (key)
            {
                case "Hardcoded:1:Caption":
                    value = "One";
                    return true;
                case "Hardcoded:2:Caption":
                    value = "Two";
                    return true;
            }
            value = null;
            return false;
        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            if (string.IsNullOrEmpty(parentPath))
            {
                return earlierKeys.Concat(new[] { "Hardcoded" });
            }
            if (parentPath == "Hardcoded")
            {
                return earlierKeys.Concat(new[] { "1", "2" });
            }
            if (parentPath == "Hardcoded:1" || parentPath == "Hardcoded:2")
            {
                return earlierKeys.Concat(new[] { "Caption" });
            }
            return earlierKeys;
        }
    }
}
