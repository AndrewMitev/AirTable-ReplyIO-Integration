using AirReplyAPI.Data.Interfaces;
using AirReplyAPI.Data.Models;
using AirReplyAPI.Data.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace AirReplyAPI.Controllers
{
    [ApiController]
    [Route("email/api")]
    public class EmailEventsController : ControllerBase
    {
        private readonly ILogger<EmailEventsController> _logger;
        private readonly IReplyIOService _replyService;
        private readonly IAirtableService _airtableService;
        private readonly IConfiguration _configuration;

        public EmailEventsController(ILogger<EmailEventsController> logger,
            IConfiguration configuration,
            IReplyIOService replyService,
            IAirtableService airtableService)
        {
            _logger = logger;
            _replyService = replyService;
            _airtableService = airtableService;
            _configuration = configuration;
        }

        public string Index()
        {
            _logger.LogInformation("************This is test!!!!!");
            _logger.LogInformation("key:" + _configuration["reply-io-api-key"]);
            return "Welcome to this test API!\r\n This api is designed to accept requests at:\r\n" +
                "* email/api/sent - catch webhooks for email sent event.\r\n" +
                "* emal/api/opened - catch webhooks for email opened event.\r\n" +
                "* emal/api/replied - catch webhooks for email replied event.\r\n";
        }

        [Route("sent")]
        [HttpPost]
        public async Task<IActionResult> EmailSent(EmailSentResponse emailSent)
        {
            if (!ModelState.IsValid || emailSent == null)
            {
                _logger.LogError("Invalid incoming data:" + string.Join(",", ModelState.Keys.ToArray()));

                return BadRequest("Invalid incoming data:" + string.Join(",", ModelState.Keys.ToArray()));
            }

            _logger.LogInformation("*******Sent email:Received data:" + emailSent.Contact_fields?.Email);

            var fields = await _replyService.OnSent_GetClientDataFromReplyAsync(emailSent);

            var response = await this._airtableService.UpdateContact(fields, 
                emailSent?.Contact_custom_fields?.Base_id,
                emailSent?.Contact_custom_fields?.Airtable_client_id);

            return StatusCode((int)response.StatusCode, response);
        }

        [Route("opened")]
        [HttpPost]
        public async Task<IActionResult> EmailOpened(EmailOpenedResponse emailOpened)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid incoming data:" + string.Join(",", ModelState.Keys));

                return BadRequest("Invalid incoming data:" + string.Join(",", ModelState.Keys.ToArray()));
            }

            _logger.LogInformation("*******EmailOpened:" + emailOpened.Contact_fields?.Email);

            var fields = new UpdateOpenedStatus { Z_Email_Opened = true, Email_Opened_Date = DateTime.Now.ToString("MM-dd-yyyy") };
            
            var response = await this._airtableService.UpdateContact(fields, 
                emailOpened.Contact_custom_fields?.Base_id,
                emailOpened.Contact_custom_fields?.Airtable_client_id);

            return StatusCode((int)response.StatusCode, response);
        }

        [Route("replied")]
        [HttpPost]
        public async Task<IActionResult> EmailReplied(EmailRepliedResponse emailReplied)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid incoming data:" + string.Join(",", ModelState.Keys));

                return BadRequest("Invalid incoming data:" + string.Join(",", ModelState.Keys.ToArray()));
            }

            _logger.LogInformation("*******EmailReplied:" + emailReplied.Contact_fields?.Email);

            var fields =  await _replyService.OnReplied_GetClientDataFromReplyAsync(emailReplied);

            var response = await this._airtableService.UpdateContact(fields, 
                emailReplied.Contact_custom_fields?.Base_id,
                emailReplied.Contact_custom_fields?.Airtable_client_id);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
