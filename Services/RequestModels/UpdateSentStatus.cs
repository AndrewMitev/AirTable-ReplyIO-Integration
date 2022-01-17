using Newtonsoft.Json;

namespace AirReplyAPI.Data.RequestModels
{
    public class UpdateSentStatus
    {
        [JsonProperty("Email Sent")]
        public string Email_Sent_Date { get; set; }

        [JsonProperty("Z_Email Sent")]
        public bool? Z_Email_Sent { get; set; }

        [JsonProperty("Sender Email Address")]
        public string? SenderEmail { get; set; }

        [JsonProperty("Z_Subject Line (From Reply.io)")]
        public string EmailSubject { get; set; }

        [JsonProperty("Z_Email Body (From Reply.io)")]
        public string EmailBody { get; set; }

        [JsonProperty("Campaign Name/Identifier")]
        public string CampaignName { get; set; }

        [JsonProperty("Z_Contact Status")]
        public string ContactStatus { get; set; }
    }
}
