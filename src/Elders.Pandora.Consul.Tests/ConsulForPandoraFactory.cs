using System;
using System.Configuration;

namespace Elders.Pandora.Consul.Tests
{
    public class ConsulForPandoraFactory : ConsulForPandora
    {
        public static ConsulForPandora Create()
        {
            return new ConsulForPandora(new Uri(ConfigurationManager.AppSettings.Get("consul-host")));
        }
    }
}
