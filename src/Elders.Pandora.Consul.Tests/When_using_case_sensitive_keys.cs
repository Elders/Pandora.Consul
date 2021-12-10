using Machine.Specifications;
using System.Collections.Generic;

namespace Elders.Pandora.Consul.Tests
{
    public class When_using_case_sensitive_keys
    {
        Establish context = () =>
        {
            consul = ConsulForPandoraFactory.Create();
            consul.Set("key1".CreatePandoraRawKey(), "value1");
            consul.Set("Key2".CreatePandoraRawKey(), "value2");
        };

        Because of = () =>
        {
            valueFromConsul1 = consul.Get("Key1".CreatePandoraRawKey());
            valueFromConsul2 = consul.Get("key2".CreatePandoraRawKey());
        };

        It should_get_value1 = () => valueFromConsul1.ShouldEqual("value1");

        It should_get_value2 = () => valueFromConsul2.ShouldEqual("value2");

        static string valueFromConsul1;
        static string valueFromConsul2;
        static ConsulForPandora consul;
    }

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

        It should_be_able_to_find_key_value = () => result.ShouldBeNull();

        static IPandoraContext pandoraContext;
        static IEnumerable<DeployedSetting> result;
        static ConsulForPandora consul;
    }
}
