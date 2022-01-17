using AirReplyAPI.Data.Interfaces;
using AirReplyAPI.Data.RequestModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AirReplyAPI.Data.Services
{
    public class AirtableService : IAirtableService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<AirtableService> logger;

        public AirtableService(IHttpClientFactory _httpClientFactory, IConfiguration config, ILogger<AirtableService> _logger)
        {
            this.httpClient = _httpClientFactory.CreateClient();
            this.configuration = config;
            this.logger = _logger;

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + configuration["airtable-api-key"]);
        }

        public async Task<bool> IsClientContactedAsync(string? baseId,string? airtableClientId)
        {
            if (baseId == null || airtableClientId == null)
            {
                throw new HttpRequestException("Missing clientID or baseID", null, System.Net.HttpStatusCode.BadRequest);
            }

            string url = $"https://api.airtable.com/v0/{baseId}/Contacts/{airtableClientId}";
            var contactStatusData = await GetDataAsync<GetClientResponseModel>(url);

            return contactStatusData?.Fields?.Z_EmailSent ?? false;            
        }

        public async Task<HttpResponseMessage> UpdateContact(object fields, string? baseId, string? airtableClientId)
        {
            if (fields == null)
            {             
                return CreateBadRequest("Fields to be updated are missing");
            }

            if (baseId == null || airtableClientId == null)
            {
                return CreateBadRequest("Base ID or airtable client ID are missing");
            }

            var data = new DataWrapper[]
            {
                new DataWrapper
                {
                    ID = airtableClientId, Fields = fields
                }
            };

            string request_url = $"{configuration["airtable-api"]}/{baseId}/Contacts";

            var payload = new { records = data };
            string json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            StringContent jsonFormatted = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            logger.LogInformation("*******Sending Request to airtable...:");
            logger.LogInformation("*******payload...:" + json);
            var response = await httpClient.PatchAsync(request_url, jsonFormatted);
            logger.LogInformation("*******Response from airtable...:" + response.StatusCode);

            return response;
        }

        private async Task<T> GetDataAsync<T>(string url)
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<T>(content);
                if (data == null)
                    throw new NullReferenceException("Could not json deserialize:" + typeof(T) + content);
                return data;
            }
            else
            {
                throw new HttpRequestException("", null, response.StatusCode);
            }
        }

        private HttpResponseMessage CreateBadRequest(string error)
        {
            logger.LogError(error);
            var badRequest = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            badRequest.ReasonPhrase = error;
            return badRequest;
        }

        private class GetClientResponseModel
        { 
            [JsonProperty("id")]
            internal string? Id { get; set; }
            [JsonProperty("fields")]
            internal Fields? Fields { get; set; }
        }

        private class Fields
        { 
            [JsonProperty("Z_Email Sent")]
            internal bool? Z_EmailSent { get; set; }
        }
    }
}
