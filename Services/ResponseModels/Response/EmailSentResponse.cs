namespace AirReplyAPI.Data.Models
{
    public class EmailSentResponse : Base
    {
        public DateTime Sent_email_date { get; set; }

        public int Sent_email_id { get; set; }

        public string? Sent_email_message_id { get; set; }

        public string? Sent_email_variant { get; set; }

        public int Email_account_id { get; set; }

        public string? Email_from { get; set; }

        public string? Sent_message_url { get; set; }
    }
}
