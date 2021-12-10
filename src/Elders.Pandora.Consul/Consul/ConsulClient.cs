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
    public class ConsulClient
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ConsulClient(Uri address)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = address,
                Timeout = TimeSpan.FromSeconds(45)
            };
        }

        public async Task<bool> CreateKeyValueAsync(string key, string value)
        {
            var content = new StringContent(value);
            var path = $"/v1/kv/{key}";

            var response = await _httpClient.PutAsync(path, content).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return bool.Parse(responseString);
            }

            return false;
        }

        public async Task<bool> DeleteKeyValueAsync(string key)
        {
            var path = $"/v1/kv/{key}";

            var response = await _httpClient.DeleteAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return bool.Parse(responseString);
            }

            return false;
        }

        public async Task<ReadKeyValueResponse> ReadKeyValueAsync(string key, bool recurce = false)
        {
            var path = $"/v1/kv/{key}?recurce={recurce}";

            var response = await _httpClient.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                List<ReadKeyValueResponse> result = JsonSerializer.Deserialize<List<ReadKeyValueResponse>>(responseString, serializerOptions);
                if (result.Any() == false)
                    return null;

                return result.FirstOrDefault();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            return null;
        }

        public async Task<IEnumerable<ReadKeyValueResponse>> ReadAllKeyValueAsync(string key, bool recurce = false)
        {
            var path = $"/v1/kv/{key}?recurce={recurce}&consistent";

            var response = await _httpClient.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                List<ReadKeyValueResponse> result = JsonSerializer.Deserialize<List<ReadKeyValueResponse>>(responseString, serializerOptions);
                return result;
            }

            return Enumerable.Empty<ReadKeyValueResponse>();
        }

        public async Task<bool> ExistKeyValueAsync(string key)
        {
            var path = $"/v1/kv/{key}";

            var response = await _httpClient.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return bool.Parse(responseString);
            }

            return false;
        }
    }
}
