using Microsoft.Extensions.Configuration;
using System;

namespace Elders.Pandora
{
    public class PandoraConsulConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            string applicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME");
            IPandoraContext context = new ClusterContext(applicationName);
            IConfigurationRepository repository = new ConsulForPandora(new Uri("http://consul.local.com:8500"));
            Pandora pandora = new Pandora(context, repository);

            return new PandoraConfigurationProvider(pandora);
        }
    }
}
