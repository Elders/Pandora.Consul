using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using System.Text.RegularExpressions;

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

        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));

            string normalizedKey = key.ToLower();
            using (var client = getClient())
            {
                var getAttempt = client.KV.Get(normalizedKey)?.Result;
                return getAttempt.StatusCode == System.Net.HttpStatusCode.OK;

            }
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
            var regexMachine = new Regex(@"([^@]+)@@([^\^]+)\^([^~]+)~~(.+)");
            var regexCluster = new Regex(@"([^@]+)@@([^\^]+)~~(.+)");
            IList<DeployedSetting> result = new List<DeployedSetting>();

            using (var client = getClient())
            {
                var getAttempt = client.KV.List("")?.Result;
                var response = getAttempt.Response;

                if (!ReferenceEquals(null, response))
                {
                    foreach (var setting in getAttempt.Response)
                    {
                        if (ReferenceEquals(null, setting) || ReferenceEquals(null, setting.Value))
                            continue;

                        var mappedKey = regexMachine.Match(setting.Key.ToString());
                        if (mappedKey.Success)
                        {
                            result.Add(new DeployedSetting(
                                    raw: mappedKey.Groups[0].Value,
                                    applicationName: mappedKey.Groups[1].Value,
                                    cluster: mappedKey.Groups[2].Value,
                                    machine: mappedKey.Groups[3].Value,
                                    key: mappedKey.Groups[4].Value,
                                    value: Encoding.UTF8.GetString(setting.Value)));
                        }
                        else
                        {
                            mappedKey = regexCluster.Match(setting.Key.ToString());

                            result.Add(new DeployedSetting(
                                   raw: mappedKey.Groups[0].Value,
                                   applicationName: mappedKey.Groups[1].Value,
                                   cluster: mappedKey.Groups[2].Value,
                                   machine: null,
                                   key: mappedKey.Groups[3].Value,
                                   value: Encoding.UTF8.GetString(setting.Value)));
                        }
                    }
                }

                return result;
            }
        }

        public void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));

            if (string.IsNullOrEmpty(value) == false)
            {
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
}
