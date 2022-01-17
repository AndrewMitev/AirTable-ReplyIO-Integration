using Newtonsoft.Json;

namespace AirReplyAPI.Data.RequestModels
{
    public class UpdateOpenedStatus
    {
        [JsonProperty("Email Opened Date")]
        public string Email_Opened_Date { get; set; }

        [JsonProperty("Email Opened")]
        public bool Z_Email_Opened { get; set; }
    }
}
