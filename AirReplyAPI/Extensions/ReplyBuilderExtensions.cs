using Newtonsoft.Json;

namespace AirReplyAPI.Extensions
{
    public static class ReplyBuilderExtensions
    {
        private static readonly string apiUrl = "https://syncio.azurewebsites.net/email/api";
        private static readonly string reply_apiUrl = "https://api.reply.io/api/v2/webhooks";
        public record class Payload(bool includeEmailUrl, bool includeProspectCustomFields);
        public record class RegisteredHook(int id, string @event, string url, bool isDisabled, DateTime createdDate, Payload payload);
        public record class NewSubscription(string @event, string url, Payload payload);

        public static IApplicationBuilder RegisterHooks(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var configuration = scope.ServiceProvider.GetService<IConfiguration>();

            if (configuration == null)
            {
                return app;
            }

            string reply_headerName = configuration["reply-io-api-auth-header-name"];
            string reply_apiSecret = configuration["reply-io-api-key"];

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add(reply_headerName, reply_apiSecret);
                var response = httpClient.GetAsync(reply_apiUrl).Result;

                if (response?.StatusCode == System.Net.HttpStatusCode.OK)
                { 
                    var subsciptions = JsonConvert.DeserializeObject<RegisteredHook[]>(response.Content.ReadAsStringAsync().Result);
                    if (subsciptions?.Length == 0)
                    {
                        registerSubscription(httpClient, SubscriptionType.email_sent.ToString());
                        registerSubscription(httpClient, SubscriptionType.email_opened.ToString());
                        registerSubscription(httpClient, SubscriptionType.email_replied.ToString());
                    }
                }
            }

            return app;
        }

        private static bool registerSubscription(HttpClient httpClient, string type)
        {
            var payload = new Payload(true, true);
            var newSubscription = new NewSubscription(type, apiUrl + trimType(type), payload);
            var response = httpClient.PostAsJsonAsync<NewSubscription>(reply_apiUrl, newSubscription).Result;

            return response.StatusCode == System.Net.HttpStatusCode.Created;
        }

        private enum SubscriptionType
        { 
            email_sent,
            email_opened,
            email_replied
        }

        private static string trimType(string type)
        {
            if (Enum.IsDefined(typeof(SubscriptionType), type))
            {
                return "/" + type.Replace("email_", string.Empty);
            }

            throw new ArgumentException("Pass subscription type!");
        }
    }
}
