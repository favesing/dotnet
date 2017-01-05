using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Testing.ConfigTest
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

    public class ConfigObjectTest
    {
        [Fact]
        public void BindObjectTree()
        {
            var dict = new Dictionary<string, string>
               {
                   {"App:Profile:Machine", "Rick"},
                   {"App:Connection:Value", "connectionstring"},
                   {"App:Window:Height", "11"},
                   {"App:Window:Width", "11"}
               };
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(dict);
            var config = builder.Build();

            var options = new AppOptions();
            config.GetSection("App").Bind(options);

            Assert.Equal("Rick", options.Profile.Machine);
            Assert.Equal(11, options.Window.Height);
            Assert.Equal(11, options.Window.Width);
            Assert.Equal("connectionstring", options.Connection.Value);
        }
    }
}
