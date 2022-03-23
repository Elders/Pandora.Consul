using Elders.Pandora.Consul.Consul.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Elders.Pandora.Consul.Consul
{
    internal class ConsulClient
    {
        private const string Header_ConsulIndex = "X-Consul-Index";

        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        public ConsulClient(Uri address)
        {
            httpClient = new HttpClient
            {
                BaseAddress = address,
                Timeout = TimeSpan.FromMinutes(15) // This was the default configuration from the nuget consul client which we were using in the past.
            };
        }

        public async Task<bool> CreateKeyValueAsync(string key, string value)
        {
            var content = new StringContent(value);
            var path = $"/v1/kv/{key}";

            using (HttpResponseMessage response = await httpClient.PutAsync(path, content).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    using var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    return JsonSerializer.Deserialize<bool>(contentStream, serializerOptions);
                }
            }

            return false;
        }

        public async Task<bool> DeleteKeyValueAsync(string key)
        {
            var path = $"/v1/kv/{key}";

            using (HttpResponseMessage response = await httpClient.DeleteAsync(path).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    using var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    return JsonSerializer.Deserialize<bool>(contentStream, serializerOptions);
                }
            }

            return false;
        }

        public async Task<ReadKeyValueResponse> ReadKeyValueAsync(string key)
        {
            var path = $"/v1/kv/{key}";

            using (HttpResponseMessage response = await httpClient.GetAsync(path).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var contentStrem = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        List<ReadKeyValueResponse> result = JsonSerializer.Deserialize<List<ReadKeyValueResponse>>(contentStrem, serializerOptions);

                        if (result?.Any() == true)
                            return result.Single();
                    }
                }
            }

            return default;
        }

        public async Task<(IEnumerable<ReadKeyValueResponse> KV, ulong lastIndex)> ReadAllKeyValuesAndMonitorAsync(string key, TimeSpan wait, ulong lastIndex)
        {
            string path = $"/v1/kv/{key}?index={lastIndex}&recurse=true&wait={wait.TotalMinutes}m";

            using (HttpResponseMessage response = await httpClient.GetAsync(path).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    if (response.Headers.Contains(Header_ConsulIndex))
                    {
                        try
                        {
                            lastIndex = ulong.Parse(response.Headers.GetValues(Header_ConsulIndex).First());
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException($"Failed to parse {Header_ConsulIndex}. StatusCode: {response.StatusCode}", ex);
                        }
                    }

                    using (var contentStrem = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {

                        List<ReadKeyValueResponse> result = JsonSerializer.Deserialize<List<ReadKeyValueResponse>>(contentStrem, serializerOptions);

                        if (result?.Any() == true)
                            return (result, lastIndex);
                    }
                }
            }

            return (Enumerable.Empty<ReadKeyValueResponse>(), 0);
        }

        public async Task<IEnumerable<ReadKeyValueResponse>> ReadAllKeyValuesAsync(string key)
        {
            string path = $"/v1/kv/{key}?recurse=true";

            using (HttpResponseMessage response = await httpClient.GetAsync(path).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var contentStrem = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        List<ReadKeyValueResponse> result = JsonSerializer.Deserialize<List<ReadKeyValueResponse>>(contentStrem, serializerOptions);

                        if (result?.Any() == true)
                            return result;
                    }
                }
            }

            return Enumerable.Empty<ReadKeyValueResponse>();
        }

        public async Task<bool> ExistKeyValueAsync(string key)
        {
            var path = $"/v1/kv/{key}";

            using (var response = await httpClient.GetAsync(path).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
