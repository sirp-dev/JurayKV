using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using JurayKV.Infrastructure.Services;

namespace JurayKV.UI.Pages
{
    //[AllowAnonymous]
    public class WebhookWhatsappModel : PageModel
    {
        private readonly LoggerLibrary _logger;

        private readonly string _whatsappToken = "YOUR_WHATSAPP_TOKEN";
        private readonly string _verifyToken = "KvAdminToken0123";

        public WebhookWhatsappModel(LoggerLibrary logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            
            string hubMode = Request.Query["hub.mode"];
            string hubVerifyToken = Request.Query["hub.verify_token"];
            string hubChallenge = Request.Query["hub.challenge"];


            _logger.Log($"log whatspp code {hubMode}, {hubVerifyToken}, {hubChallenge}");
            if (hubMode == "subscribe" && hubVerifyToken == _verifyToken)
            {
                System.Console.WriteLine("WEBHOOK_VERIFIED");
                return Content(hubChallenge);
            }
            else
            {
                return new ForbidResult();
            }
         }

        public async Task<IActionResult> OnPostAsync([FromBody] JObject body)
        {
            // Check the Incoming webhook message
            System.Console.WriteLine(body.ToString());

            if (body["object"] != null)
            {
                var changes = body["entry"]?[0]?["changes"]?[0];
                if (changes != null && changes["value"]?["messages"]?[0] != null)
                {
                    var phone_number_id = changes["value"]?["metadata"]?["phone_number_id"];
                    var from = changes["value"]?["messages"]?[0]?["from"];
                    var msgBody = changes["value"]?["messages"]?[0]?["text"]?["body"]?.ToString();

                    if (phone_number_id != null && from != null && msgBody != null)
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var url = $"https://graph.facebook.com/v12.0/{phone_number_id}/messages?access_token={_whatsappToken}";
                            var payload = new
                            {
                                messaging_product = "whatsapp",
                                to = from,
                                text = new { body = $"Ack: {msgBody}" }
                            };

                            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                            var response = await httpClient.PostAsync(url, content);

                            if (response.IsSuccessStatusCode)
                            {
                                return new OkResult();
                            }
                            else
                            {
                                return new BadRequestResult();
                            }
                        }
                    }
                }
            }

            return new NotFoundResult();
        }

        public IActionResult OnGetVerify([FromQuery] string hub_mode, [FromQuery] string hub_verify_token, [FromQuery] string hub_challenge)
        {
            if (hub_mode == "subscribe" && hub_verify_token == _verifyToken)
            {
                System.Console.WriteLine("WEBHOOK_VERIFIED");
                return Content(hub_challenge);
            }
            else
            {
                return new ForbidResult();
            }
        }
    }
}
