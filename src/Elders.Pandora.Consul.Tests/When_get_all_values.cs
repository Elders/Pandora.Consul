using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Elders.Pandora.Consul.Tests
{
    public class When_get_all_values
    {
        Establish context = async () =>
        {
            consul = ConsulForPandoraFactory.Create();
            await consul.SetAsync("key".CreatePandoraRawKey(), "value").ConfigureAwait(false);
            await consul.SetAsync("key1".CreatePandoraRawKey(), "value1").ConfigureAwait(false);
            await consul.SetAsync("Key2".CreatePandoraRawKey(), "value2").ConfigureAwait(false);

            pandoraContext = new ApplicationContext("elders.pandora.consul.tests/test/*");
        };

        Because of = () => result = consul.GetAll(pandoraContext);

        It should_have_count_equal_to_3 = () => result.Count().ShouldEqual(3);

        static IPandoraContext pandoraContext;
        static IEnumerable<DeployedSetting> result;
        static ConsulForPandora consul;
    }
}
