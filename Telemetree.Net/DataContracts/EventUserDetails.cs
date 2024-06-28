using Newtonsoft.Json;

namespace Telemetree.Net.DataContracts
{
    public class EventUserDetails
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("firstName", Required = Required.Always)]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("isPremium")]
        public bool IsPremium { get; set; }

        [JsonProperty("writeAccess")]
        public bool WriteAccess { get; set; }
    }
}
