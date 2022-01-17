using Newtonsoft.Json;

namespace AirReplyAPI.Data.RequestModels
{
    public class UpdateOpenedStatus
    {
        [JsonProperty("Email Opened")]
        public string Email_Opened_Date { get; set; }

        [JsonProperty("Z_Email Opened")]
        public bool Z_Email_Opened { get; set; }
    }
}
