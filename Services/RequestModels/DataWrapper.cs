using Newtonsoft.Json;

namespace AirReplyAPI.Data.RequestModels
{
    public class DataWrapper
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("fields")]
        public object? Fields { get; set; }
    }
}
