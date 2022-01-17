using Newtonsoft.Json;

namespace AirReplyAPI.Data.Models
{
    public class SequenceModel
    {
        public int? Id { get; set; }

        [JsonProperty("step_number")]
        public int StepNumber { get; set; }
    }
}
