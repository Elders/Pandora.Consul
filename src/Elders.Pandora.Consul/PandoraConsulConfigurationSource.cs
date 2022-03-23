using System;
using System.Threading.Tasks;
using Elders.Pandora.Consul.Consul;
using Microsoft.Extensions.Configuration;

namespace Elders.Pandora
{
    public class PandoraConsulConfigurationSource : PandoraConfigurationSource
    {
        private const string ConsulDefaultAddress = "http://consul.local.com:8500";

        private readonly Uri consulHost;

        /// <summary>
        /// Initializes PandoraConsulConfigurationSource
        /// </summary>
        /// <param name="consulHost">The consul host. Ex: http://consul.local.com:8500</param>
        public PandoraConsulConfigurationSource(string consulHost = null)
        {
            this.consulHost = new Uri(consulHost ?? ConsulDefaultAddress);
            ReloadDelay = TimeSpan.FromMinutes(5);
        }

        public override IPandoraWatcher ReloadWatcher { get; set; }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            IPandoraContext context = new ApplicationContext();

            ConsulForPandora repository = new ConsulForPandora(consulHost);
            Pandora = new Pandora(context, repository);
            ReloadWatcher = new ConsulRefresher(Pandora, new ConsulClient(consulHost), ReloadDelay);
            ChangeTokenConsumer = (provider) => Task.Factory.StartNew(() => provider.Load());

            return new PandoraConfigurationProvider(this);
        }
    }
}
