namespace AirReplyAPI.Data.Models
{
    public abstract class Base
    {
        public EventModel? Event { get; set; }

        public CommonResponseModel? Contact_fields { get; set; }

        public CustomFieldsModel? Contact_custom_fields { get; set; }

        public SequenceModel? Sequence_fields { get; set; }
    }
}
