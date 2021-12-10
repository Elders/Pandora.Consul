using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Elders.Pandora.Consul.Tests
{
    public class When_get_all_values
    {
        Establish context = () =>
        {
            consul = ConsulForPandoraFactory.Create();
            consul.Set("key".CreatePandoraRawKey(), "value");
            consul.Set("key1".CreatePandoraRawKey(), "value1");
            consul.Set("Key2".CreatePandoraRawKey(), "value2");

            pandoraContext = new ApplicationContext("elders.pandora.consul.tests/test/*");
        };

        Because of = () => result = consul.GetAll(pandoraContext);

        It should_have_count_equal_to_3 = () => result.Count().ShouldEqual(3);

        static IPandoraContext pandoraContext;
        static IEnumerable<DeployedSetting> result;
        static ConsulForPandora consul;
    }
}
