using Newtonsoft.Json;
using Telemetree.Net.DataContracts;

namespace Telemetree.Net.Helpers
{
    public static class TelemetreeHelper
    {
        public static async Task<TelemetreeConfig> GetTelemetreeConfig(string apiKey, string projectId)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var response = await httpClient.GetAsync(new Uri($"https://config.ton.solutions/v1/client/config?project={projectId}"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Received error: {await response.Content.ReadAsStringAsync()}");

                return JsonConvert.DeserializeObject<TelemetreeConfig>(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
