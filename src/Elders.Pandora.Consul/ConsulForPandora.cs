using System;
using System.Collections.Generic;
using System.Text;
using Consul;

namespace Elders.Pandora
{
    public class ConsulForPandora : IConfigurationRepository
    {
        readonly ConsulClientConfiguration cfg;

        public ConsulForPandora() : this(new ConsulClientConfiguration()) { }

        public ConsulForPandora(ConsulClientConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));

            try
            {
                using (var client = new ConsulClient((conf) => conf = this.cfg))
                {
                    var getAttempt = client.KV.Get(key).Result;
                    if (getAttempt.Response.Value == null || getAttempt.Response.Value.Length == 0)
                        throw new KeyNotFoundException("Unable to set consul value for key: " + key);

                    byte[] valBytes = getAttempt.Response.Value;
                    return Encoding.UTF8.GetString(valBytes);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));
            if (string.IsNullOrEmpty(value)) throw new ArgumentException(nameof(value));

            try
            {
                using (var client = new ConsulClient((conf) => conf = this.cfg))
                {
                    var putPair = new KVPair(key)
                    {
                        Value = Encoding.UTF8.GetBytes(value)
                    };

                    var putAttempt = client.KV.Put(putPair).Result;

                    if (putAttempt.Response)
                        throw new KeyNotFoundException("Unable to set consul value for key: " + key);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
