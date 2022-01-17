namespace AirReplyAPI.Data.Models
{
    public class EmailRepliedResponse : Base
    {
        public string? Reply_message_id { get; set; }

        public DateTime Reply_date  { get; set; }

        public int Sent_email_id { get; set; }

        public int Email_account_id { get; set; }

        public string? Reply_message_url { get; set; }

        public int Reason { get; set; }
    }
}
