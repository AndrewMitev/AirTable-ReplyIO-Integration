using AirReplyAPI.Data.Interfaces;
using AirReplyAPI.Data.Models;
using AirReplyAPI.Data.RequestModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AirReplyAPI.Data.Services
{
    public class ReplyIOService : IReplyIOService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<ReplyIOService> logger;

        public ReplyIOService(IHttpClientFactory _httpClientFactory, IConfiguration _configuration, ILogger<ReplyIOService> _logger)
        { 
            this.configuration = _configuration;
            httpClient = _httpClientFactory.CreateClient();
            logger = _logger;
            httpClient.DefaultRequestHeaders.Add(configuration["reply-io-api-auth-header-name"],
                configuration["reply-io-api-key"]);
        }

        public async Task<UpdateSentStatus> OnSent_GetClientDataFromReplyAsync(EmailSentResponse sentEmailResponse)
        {
            switch (sentEmailResponse?.Sequence_fields?.StepNumber)
            {
                case 0: return await OnSent_SequenceOne(sentEmailResponse);
                case 3: return await OnSent_SequenceFour(sentEmailResponse);
                default: return null;
            }
        }

        public async Task<UpdateRepliedStatus> OnReplied_GetClientDataFromReplyAsync(EmailRepliedResponse emailReplied)
        {
            string contactStatus = await GetContactStatusAsync(emailReplied.Contact_fields?.Email);
            (_, string body) = await GetEmailDataAsync(emailReplied.Reply_message_id ?? string.Empty);
            logger.LogInformation("-----------Reply Body:" + body);

            var fields = new UpdateRepliedStatus
            {
                Email_Replied_Date = DateTime.Now.ToString("MM-dd-yyyy"),
                Z_Email_Replied = true,
                RepliedStep = emailReplied?.Sequence_fields?.StepNumber,
                ContactStatus = contactStatus,
                Z_EmailResponse = body
            };

            return fields;
        }

        public async Task<UpdateSentStatus> OnSent_SequenceOne(EmailSentResponse sentEmailResponse)
        {
            (string subject, string body) = await GetEmailDataAsync(sentEmailResponse?.Sent_email_message_id ?? string.Empty);
            string campaignName = await GetCampaignNameAsync(sentEmailResponse?.Sequence_fields?.Id);
            string contactStatus = await GetContactStatusAsync(sentEmailResponse?.Contact_fields?.Email);
            logger.LogInformation("-----------Campaign Name:" + campaignName);
            logger.LogInformation("-----------Contact Status:" + contactStatus);
            logger.LogInformation("-----------Subject:" + subject);
            logger.LogInformation("-----------Body:" + body);


            var fields = new UpdateSentStatus
            {
                Email_Sent_Date = DateTime.Now.ToString("MM-dd-yyyy"),
                Z_Email_Sent = true,
                SenderEmail = sentEmailResponse.Email_from,
                EmailSubject = subject,
                EmailBody = body,
                CampaignName = campaignName,
                ContactStatus = contactStatus
            };

            return fields;
        }

        public async Task<UpdateSentStatus> OnSent_SequenceFour(EmailSentResponse sentEmailResponse)
        {
            var fields = new UpdateSentStatus
            {
                ContactStatus = ContactStatus.Unresponsive
            };

            return await Task.Run(() => fields);
        }

        private async Task<(string subject, string body)> GetEmailDataAsync(string emailMessageId)
        {
            string url = $"https://api.reply.io/api/v2/emails/{emailMessageId}/content";
            var emailData = await GetDataAsync<EmailDataResponse>(url);
            return (emailData?.Subject ?? "Default Subject" , emailData?.Body ?? "Default Body");
        }

        private async Task<string> GetCampaignNameAsync(int? campaignId)
        {
            string url = $"https://api.reply.io/v1/campaigns?id={campaignId}";
            var campaignData = await GetDataAsync<CampaignDataResponse>(url);
            return campaignData?.Name ?? "Default campaign";
        }

        private async Task<string> GetContactStatusAsync(string contactEmail)
        {
            string url = $"https://api.reply.io/v1/stats/status_in_campaign?email={contactEmail}";
            var contactStatusData = await GetDataAsync<ContactStatusResponse>(url);

            switch (contactStatusData?.Status)
            {
                case ReplyContactStatus.Active: return ContactStatus.AttemptingToContact;
                case ReplyContactStatus.Contacted: return ContactStatus.Interested;
                case ReplyContactStatus.Finished:
                case ReplyContactStatus.Replied: return ContactStatus.Replied;
                default: return ContactStatus.Bounced;
            }
        }

        private async Task<T> GetDataAsync<T>(string url)
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(content);
                return data;
            }

            logger.LogWarning("Request Failed:" + url);
            logger.LogWarning("Status code:" + response.StatusCode + " Reason:" + response.ReasonPhrase);
            return default;
        }

        private class EmailDataResponse
        {
            [JsonProperty("subject")]
            internal string? Subject { get; set; }

            [JsonProperty("textBody")]
            internal string? Body { get; set; }
        }

        private class CampaignDataResponse
        { 
            [JsonProperty("name")]
            internal string? Name { get; set; }
        }

        private class ContactStatusResponse
        { 
            [JsonProperty("status")]
            internal string? Status { get; set; }
        }
    }
}
