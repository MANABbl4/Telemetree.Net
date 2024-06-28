using Newtonsoft.Json;

namespace Telemetree.Net.DataContracts
{
    public class EventDetails
    {
        [JsonProperty("startParameter")]
        public string StartParameter { get; set; }

        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; }

        [JsonProperty("params", Required = Required.Always)]
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }
}
