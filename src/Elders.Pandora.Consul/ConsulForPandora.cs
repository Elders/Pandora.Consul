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
            string normalizedKey = key.ToLower();
            try
            {
                using (var client = new ConsulClient((conf) => conf = this.cfg))
                {
                    var getAttempt = client.KV.Get(normalizedKey).Result;
                    if (getAttempt.Response.Value == null || getAttempt.Response.Value.Length == 0)
                        throw new KeyNotFoundException("Unable to set consul value for key: " + normalizedKey);

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
            string normalizedKey = key.ToLower();
            try
            {
                using (var client = new ConsulClient((conf) => conf = this.cfg))
                {
                    var putPair = new KVPair(normalizedKey)
                    {
                        Value = Encoding.UTF8.GetBytes(value)
                    };

                    var putAttempt = client.KV.Put(putPair).Result;

                    if (putAttempt.Response == false)
                        throw new KeyNotFoundException("Unable to set consul value for key: " + normalizedKey);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
