namespace AirReplyAPI.Data.Models
{
    public class EmailOpenedResponse : Base
    {
        public DateTime Email_open_date { get; set; }

        public int Sent_email_id { get; set; }

        public string? Sent_email_message_id { get; set; }

        public int Email_account_id { get; set; }

        public int Opens_count { get; set; }
    }
}
