using Newtonsoft.Json;

namespace AirReplyAPI.Data.RequestModels
{
    public class UpdateRepliedStatus
    {
        [JsonProperty("Email Replied")]
        public string Email_Replied_Date { get; set; }

        [JsonProperty("Z_Email Replied")]
        public bool? Z_Email_Replied { get; set; }

        [JsonProperty("Z_Email Response (From Reply.io)")]
        public string? Z_EmailResponse { get; set; }

        [JsonProperty("Replied_Step#")]
        public int? RepliedStep { get; set; }

        [JsonProperty("Z_Contact Status")]
        public string? ContactStatus { get; set; }
    }
}
