using Newtonsoft.Json;

namespace Telemetree.Net.DataContracts
{
    public class TelemetreeEvent
    {
        [JsonProperty("eventType")]
        public string EventName { get; set; }

        [JsonProperty("userDetails", Required = Required.Always)]
        public EventUserDetails UserDetails { get; set; }

        [JsonProperty("app")]
        public string AppName { get; set; }

        [JsonProperty("eventDetails", Required = Required.Always)]
        public EventDetails EventDetails { get; set; }

        [JsonProperty("telegramID", Required = Required.Always)]
        public string TelegramId { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("device", Required = Required.Always)]
        public string Device { get; set; }

        [JsonProperty("referrerType")]
        public string ReferrerType { get; set; } = "N/A";

        [JsonProperty("referrer")]
        public string Referrer { get; set; } = "0";

        [JsonProperty("timestamp", Required = Required.Always)]
        public string Timestamp { get; set; }

        [JsonProperty("isAutocapture", Required = Required.Always)]
        public bool IsAutocapture { get; set; }

        [JsonProperty("wallet")]
        public string? Wallet { get; set; }

        [JsonProperty("sessionIdentifier", Required = Required.Always)]
        public string SessionIdentifier { get; set; }

        [JsonProperty("eventSource")]
        public string EventSource { get; set; } = "telemetree_net";
    }
}
