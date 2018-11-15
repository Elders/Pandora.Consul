using System;
using Elders.Pandora.Box;
using Microsoft.Extensions.Configuration;

namespace Elders.Pandora.Consul.Tests
{
    public class ConsulForPandoraFactory : ConsulForPandora
    {
        public static ConsulForPandora Create()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            string consulUrl = config["consulHost"];

            return new ConsulForPandora(new Uri(consulUrl));
        }
    }

    public static class TestConsulPandoraKeyExtensions
    {
        public static string CreatePandoraRawKey(this string settingKey)
        {
            return NameBuilder.GetSettingName("Elders.Pandora.Consul.Tests", "test", Elders.Pandora.Box.Machine.NotSpecified, settingKey);
        }
    }

    public class TestContext : ApplicationContext
    {
        public TestContext() : base("Elders.Pandora.Consul.Tests")
        {

        }
    }
}
