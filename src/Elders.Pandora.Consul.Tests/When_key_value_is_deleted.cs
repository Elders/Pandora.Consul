using System;
using System.Collections.Generic;
using Machine.Specifications;

namespace Elders.Pandora.Consul.Tests
{
    public class When_key_value_is_deleted
    {
        Establish context = () =>
        {
            consul = new ConsulForPandora();
            consul.Set("key", "value");
            consul.Get("key");
            consul.Delete("key");
        };

        Because of = () => exception = Catch.Exception(() => consul.Get("key"));

        It should_be_able_to_find_key_value = () => exception.ShouldBeOfExactType<KeyNotFoundException>();

        static Exception exception;
        static ConsulForPandora consul;
    }
}