using Machine.Specifications;

namespace Elders.Pandora.Consul.Tests
{
    public class When_key_value_is_stored
    {
        Establish context = () =>
        {
            consul = ConsulForPandoraFactory.Create();
            consul.Set("key", "value");
        };

        Because of = () => valueFromConsul = consul.Get("key");

        It should_get_value = () => valueFromConsul.ShouldEqual("value");

        static string valueFromConsul;
        static ConsulForPandora consul;
    }
}
