using System;
using System.Collections.Generic;
using Machine.Specifications;

namespace Elders.Pandora.Consul.Tests
{
    public class When_key_value_is_deleted
    {
        Establish context = () =>
        {
            consul = ConsulForPandoraFactory.Create();
            consul.Set("key".CreatePandoraRawKey(), "value");
            consul.Get("key".CreatePandoraRawKey());
            consul.Delete("key".CreatePandoraRawKey());
        };

        Because of = () => exception = Catch.Exception(() => consul.Get("key".CreatePandoraRawKey()));

        It should_be_able_to_find_key_value = () => exception.ShouldBeOfExactType<KeyNotFoundException>();

        static Exception exception;
        static ConsulForPandora consul;
    }
}