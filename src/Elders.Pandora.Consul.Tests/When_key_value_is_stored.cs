using Machine.Specifications;

namespace Elders.Pandora.Consul.Tests
{
    public class When_key_value_is_stored
    {
        Establish context = async () =>
        {
            consul = ConsulForPandoraFactory.Create();
            await consul.SetAsync("key".CreatePandoraRawKey(), "value").ConfigureAwait(false);
        };

        Because of = async () => valueFromConsul = await consul.GetAsync("key".CreatePandoraRawKey()).ConfigureAwait(false);

        It should_get_value = () => valueFromConsul.ShouldEqual("value");

        static string valueFromConsul;
        static ConsulForPandora consul;
    }
}
