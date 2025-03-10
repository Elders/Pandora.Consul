using System;
using System.Collections.Generic;
using Machine.Specifications;

namespace Elders.Pandora.Consul.Tests
{
    public class When_key_value_is_deleted
    {
        Establish context = async () =>
        {
            consul = ConsulForPandoraFactory.Create();
            await consul.SetAsync("key".CreatePandoraRawKey(), "value").ConfigureAwait(false);
            await consul.GetAsync("key".CreatePandoraRawKey()).ConfigureAwait(false);
            await consul.DeleteAsync("key".CreatePandoraRawKey()).ConfigureAwait(false);
        };

        Because of = async () => exception = await Catch.ExceptionAsync(async () => await consul.GetAsync("key".CreatePandoraRawKey()).ConfigureAwait(false)).ConfigureAwait(false);

        It should_be_able_to_find_key_value = () => exception.ShouldBeOfExactType<KeyNotFoundException>();

        static Exception exception;
        static ConsulForPandora consul;
    }
}
