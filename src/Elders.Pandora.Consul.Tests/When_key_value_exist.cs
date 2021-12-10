using Machine.Specifications;

namespace Elders.Pandora.Consul.Tests
{
    public class When_key_value_exist
    {
        Establish context = () =>
        {
            consul = ConsulForPandoraFactory.Create();

            //fist delete keys to be sure that didn't exists
            consul.Delete("key1".CreatePandoraRawKey());
            consul.Delete("key2".CreatePandoraRawKey());

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
