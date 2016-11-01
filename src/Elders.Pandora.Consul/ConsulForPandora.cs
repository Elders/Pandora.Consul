﻿using System;
using System.Collections.Generic;
using System.Text;
using Consul;

namespace Elders.Pandora
{
    public class ConsulForPandora : IConfigurationRepository
    {
        readonly Func<IConsulClient> getClient;

        public ConsulForPandora(Uri address = null)
        {
            getClient = () => new ConsulClient(cfg =>
            {
                cfg.Address = address ?? cfg.Address;
            });
        }

        public void Delete(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));

            string normalizedKey = key.ToLower();
            using (var client = getClient())
            {
                var getAttempt = client.KV.Delete(normalizedKey)?.Result;
                if (getAttempt.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new KeyNotFoundException("Unable to delete key/value with key: " + normalizedKey);
            }
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));
            string normalizedKey = key.ToLower();
            using (var client = getClient())
            {
                var getAttempt = client.KV.Get(normalizedKey)?.Result;
                if (getAttempt.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException("Unable to find value for key: " + normalizedKey);

                byte[] valBytes = getAttempt.Response.Value;
                return Encoding.UTF8.GetString(valBytes);
            }
        }

        public IEnumerable<DeployedSetting> GetAll()
        {
            throw new NotImplementedException("It is open source. Pull requests are welcome => https://github.com/Elders/Pandora.Consul");
        }

        public void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));
            if (string.IsNullOrEmpty(value)) throw new ArgumentException(nameof(value));

            string normalizedKey = key.ToLower();
            using (var client = getClient())
            {
                var putPair = new KVPair(normalizedKey)
                {
                    Value = Encoding.UTF8.GetBytes(value)
                };

                var putAttempt = client.KV.Put(putPair).Result;
                if (putAttempt.Response == false || putAttempt.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new KeyNotFoundException("Unable to store key/value: " + normalizedKey + "  " + value);
            }
        }
    }
}
