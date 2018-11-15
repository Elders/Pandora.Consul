using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Elders.Pandora
{
    public class PandoraConsulConfigurationSource : PandoraConfigurationSource
    {
        private readonly string consulHost;

        /// <summary>
        /// Initializes PandoraConsulConfigurationSource
        /// </summary>
        /// <param name="consulHost">The consul host. Ex: http://consul.local.com:8500</param>
        public PandoraConsulConfigurationSource(string consulHost = null)
        {
            this.consulHost = consulHost ?? "http://consul.local.com:8500";
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            IPandoraContext context = new ApplicationContext();
            IConfigurationRepository repository = new ConsulForPandora(new Uri(consulHost));
            Pandora = new Pandora(context, repository);
            ChangeTokenConsumer = (provider) => Task.Factory.StartNew(() => provider.Load(reload: true));

            return new PandoraConfigurationProvider(this);
        }
    }
}
