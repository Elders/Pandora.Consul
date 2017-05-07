using System;
using System.Configuration;
using Elders.Pandora.Box;

namespace Elders.Pandora.Consul.Tests
{
    public class ConsulForPandoraFactory : ConsulForPandora
    {
        public static ConsulForPandora Create()
        {
            return new ConsulForPandora(new Uri(ConfigurationManager.AppSettings.Get("consul-host")));
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
