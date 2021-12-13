using Elders.Pandora.Consul.Consul;
using Elders.Pandora.Consul.Consul.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Elders.Pandora
{
    public class ConsulForPandora : IConfigurationRepository
    {
        public const string RootFolder = "pandora";

        private readonly ConsulClient _client;

        public ConsulForPandora(Uri address = null)
        {
            _client = new ConsulClient(address);
        }

        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));

            string normalizedKey = key.ToLower().ToConsulKey();

            return _client.ExistKeyValueAsync(normalizedKey).GetAwaiter().GetResult();
        }

        public void Delete(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));

            string normalizedKey = key.ToLower().ToConsulKey();

            bool result = _client.DeleteKeyValueAsync(normalizedKey).GetAwaiter().GetResult();

            if (result == false)
                throw new KeyNotFoundException("Unable to delete key/value with key: " + normalizedKey);
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));
            string normalizedKey = key.ToLower().ToConsulKey();

            var result = _client.ReadKeyValueAsync(normalizedKey).GetAwaiter().GetResult();

            if (result is null)
                throw new KeyNotFoundException("Unable to find value for key: " + normalizedKey);

            byte[] data = Convert.FromBase64String(result.Value);

            var value = Encoding.UTF8.GetString(data);

            return value;
        }

        static readonly TimeSpan WaitTime = TimeSpan.FromMinutes(5);
        ulong waitIndexGetAll = 0;

        public IEnumerable<DeployedSetting> GetAll(IPandoraContext context)
        {
            string pandoraApplication = context.ToApplicationKeyPrefix();
            Console.WriteLine($"Refreshing {pandoraApplication} configuration from Consul - {Thread.CurrentThread.ManagedThreadId}");

            (IEnumerable<ReadKeyValueResponse> KV, ulong lastIndex) response = _client.ReadAllKeyValueAsync(pandoraApplication, WaitTime, waitIndexGetAll).GetAwaiter().GetResult();
            waitIndexGetAll = response.lastIndex;
            List<DeployedSetting> kv = response.KV.Select(x => new DeployedSetting(x.Key.FromConsulKey(), Encoding.UTF8.GetString(Convert.FromBase64String(x.Value)))).ToList();

            Console.WriteLine($"Refreshing {pandoraApplication} configuration from Consul completed - {Thread.CurrentThread.ManagedThreadId}");

            return kv;
        }

        public void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));

            if (string.IsNullOrEmpty(value) == false)
            {
                string normalizedKey = key.ToLower().ToConsulKey();

                var result = _client.CreateKeyValueAsync(normalizedKey, value).GetAwaiter().GetResult();

                if (result == false)
                    throw new KeyNotFoundException("Unable to store key/value: " + normalizedKey + "  " + value);
            }
        }
    }
}
