using Newtonsoft.Json;

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

            if (!string.IsNullOrWhiteSpace(apiKey))
                _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

            if (!string.IsNullOrWhiteSpace(projectId))
                _httpClient.DefaultRequestHeaders.Add("X-Project-Id", projectId);

            _httpClient.BaseAddress = new Uri("https://api-analytics.ton.solutions/events");
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<string> PostEvent<T>(T eventObject)
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
