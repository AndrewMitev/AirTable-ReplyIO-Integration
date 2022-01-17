namespace AirReplyAPI.Data.RequestModels
{
    public class ContactStatus
    {
        public const string Unresponsive = "Unresponsive";
        public const string Replied = "Replied - Update me";
        public const string ReferredToColleague = "Referred To Colleague";
        public const string Push = "Push (Follow Up Later)";
        public const string NotInterested = "Not Interested";
        public const string New = "New";
        public const string Interested = "Interested";
        public const string Disqualified = "Disqualified";
        public const string Bounced = "Bounced";
        public const string AttemptingToContact = "Attempting To Contact";
    }

    public class ReplyContactStatus
    {
        public const string Active = "Active";
        public const string Contacted = "Contacted";
        public const string Replied = "Replied";
        public const string Finished = "Finished";
    }
}
