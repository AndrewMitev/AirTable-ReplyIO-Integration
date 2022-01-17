namespace AirReplyAPI.Data.Models
{
    public class CommonResponseModel
    {
        public int? Id { get; set; }

        public string? Email { get; set; }

        public string? Domain { get; set; }

        public string? First_name { get; set; }

        public string? Last_name { get; set; }

        public string? Full_name { get; set; }

        public string? Title { get; set; }

        public string? Company { get; set; }

        public string? Phone { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? Time_zone_id { get; set; }

        public string? Linkedin_profile_url { get; set; }

        public string? Company_size { get; set; }

        public string? Industry { get; set; }

        public string? Linkedin_sales_navigator { get; set; } = "none";

        public string? Linkedin_recruiter { get; set; } = "none";
    }
}
