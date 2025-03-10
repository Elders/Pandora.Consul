using Machine.Specifications;

namespace Elders.Pandora.Consul.Tests
{
    public class When_using_case_sensitive_keys
    {
        Establish context = async () =>
        {
            consul = ConsulForPandoraFactory.Create();
            await consul.SetAsync("key1".CreatePandoraRawKey(), "value1").ConfigureAwait(false);
            await consul.SetAsync("Key2".CreatePandoraRawKey(), "value2").ConfigureAwait(false);
        };

        Because of = async () =>
        {
            valueFromConsul1 = await consul.GetAsync("Key1".CreatePandoraRawKey()).ConfigureAwait(false);
            valueFromConsul2 = await consul.GetAsync("key2".CreatePandoraRawKey()).ConfigureAwait(false);
        };

        It should_get_value1 = () => valueFromConsul1.ShouldEqual("value1");

        It should_get_value2 = () => valueFromConsul2.ShouldEqual("value2");

        static string valueFromConsul1;
        static string valueFromConsul2;
        static ConsulForPandora consul;
    }
}
