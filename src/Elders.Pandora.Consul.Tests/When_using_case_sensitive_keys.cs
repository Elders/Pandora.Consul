using Machine.Specifications;

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

    public class When_key_value_exist
    {
        Establish context = () =>
        {
            consul = ConsulForPandoraFactory.Create();
            consul.Set("key1".CreatePandoraRawKey(), "value1");
        };

        Because of = () =>
        {
            key1Exist = consul.Exists("key1".CreatePandoraRawKey());
            key2Exist = consul.Exists("key2".CreatePandoraRawKey());
        };

        It should_be_true = () => key1Exist.ShouldBeTrue();

        It should_be_false = () => key2Exist.ShouldBeFalse();

        static bool key1Exist;
        static bool key2Exist;
        static ConsulForPandora consul;
    }
}
