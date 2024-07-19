using Newtonsoft.Json;
using Telemetree.Net.DataContracts;
using Telemetree.Net.Helpers;

namespace Telemetree.Net
{
    public class TelemetreeEventsHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly TelemetreeConfig _config;

        public TelemetreeEventsHttpClient(TelemetreeConfig config, string apiKey, string projectId, HttpClient? httpClient = default)
        {
            _config = config;

            _httpClient = httpClient ?? new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");

            if (!string.IsNullOrWhiteSpace(apiKey))
                _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

            if (!string.IsNullOrWhiteSpace(projectId))
                _httpClient.DefaultRequestHeaders.Add("x-project-id", projectId);

            _httpClient.BaseAddress = new Uri(config.Host);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        /// <summary>
        /// Sends event data to Telemetree
        /// The same as Track in telemetree-react
        /// </summary>
        /// <typeparam name="T">Any object type</typeparam>
        /// <param name="eventObject">Your object data</param>
        /// <returns>Server response if event accepted</returns>
        /// <exception cref="Exception">Received error.</exception>
        public async Task<string> SendAsync<T>(T eventObject)
        {
            var jsonPayload = EncryptEventData(eventObject);
            StringContent content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(string.Empty, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Received error: {await response.Content.ReadAsStringAsync()}");

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private string EncryptEventData<T>(T eventObject)
        {
            // Generate AES key and IV
            var (key, iv) = CryptoHelper.GenerateAesKeyIv();

            // Encrypt AES key and IV using RSA
            string encryptedKey = CryptoHelper.RsaEncrypt(_config.PublicKey, key);
            string encryptedIv = CryptoHelper.RsaEncrypt(_config.PublicKey, iv);

            // Encrypt event data using AES
            string encryptedBody = CryptoHelper.AesEncrypt(key, iv, JsonConvert.SerializeObject(eventObject));

            // Prepare the payload with encrypted data
            var payload = new
            {
                key = encryptedKey,
                iv = encryptedIv,
                body = encryptedBody
            };

            return JsonConvert.SerializeObject(payload);
        }
    }
}
