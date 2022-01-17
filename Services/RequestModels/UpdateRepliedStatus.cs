using Newtonsoft.Json;

namespace AirReplyAPI.Data.RequestModels
{
    public class UpdateRepliedStatus
    {
        [JsonProperty("Email Replied")]
        public string Email_Replied_Date { get; set; }

        [JsonProperty("Email Replied")]
        public bool? Z_Email_Replied { get; set; }

        [JsonProperty("Email Response")]
        public string? Z_EmailResponse { get; set; }

        [JsonProperty("Replied Step#")]
        public int? RepliedStep { get; set; }

        [JsonProperty("Contact Status")]
        public string? ContactStatus { get; set; }
    }
}
