using Newtonsoft.Json;

namespace Telemetree.Net.DataContracts
{
    public class TelemetreeConfig
    {
        [JsonProperty("auto_capture")]
        public bool AutoCapture { get; set; }

        [JsonProperty("auto_capture_tags")]
        public string[] AutoCaptureTags { get; set; }

        [JsonProperty("auto_capture_classes")]
        public string[] AutoCaptureClasses { get; set; }

        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }
    }
}
