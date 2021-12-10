using Elders.Pandora.Consul.Consul.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Elders.Pandora.Consul.Consul
{
    internal class ConsulClient
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        public ConsulClient(Uri address)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = address
            };
        }

        public async Task<bool> CreateKeyValueAsync(string key, string value)
        {
            var content = new StringContent(value);
            var path = $"/v1/kv/{key}";

            using (HttpResponseMessage response = await _httpClient.PutAsync(path, content).ConfigureAwait(false))
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

            using (HttpResponseMessage response = await _httpClient.DeleteAsync(path).ConfigureAwait(false))
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

            using (HttpResponseMessage response = await _httpClient.GetAsync(path).ConfigureAwait(false))
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

        public async Task<IEnumerable<ReadKeyValueResponse>> ReadAllKeyValueAsync(string key, TimeSpan wait)
        {
            string path = $"/v1/kv/{key}?recurse=true&wait={wait.TotalMinutes}m";

            using (HttpResponseMessage response = await _httpClient.GetAsync(path).ConfigureAwait(false))
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

            using (var response = await _httpClient.GetAsync(path).ConfigureAwait(false))
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
