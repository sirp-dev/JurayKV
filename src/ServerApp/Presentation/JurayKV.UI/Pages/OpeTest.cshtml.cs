using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using System.Text;

namespace JurayKV.UI.Pages
{
    [Authorize]
    public class OpeTestModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWhatsappOtp _whatsappOtp;
        private readonly LoggerLibrary _logger;

        private readonly IEmailSender _sender;
        public OpeTestModel(IEmailSender sender, UserManager<ApplicationUser> userManager, IWhatsappOtp whatsappOtp, LoggerLibrary logger)
        {
            _sender = sender;
            _userManager = userManager;
            _whatsappOtp = whatsappOtp;
            _logger = logger;
        }
        public bool Result = false;
        public async Task<IActionResult> OnGetAsync()
        {


            string userId = _userManager.GetUserId(HttpContext.User);

            try
            {
                bool result = await _whatsappOtp.SendAsync("Your Koboview OTP is 119085", userId.ToString());

                //string apiUrl = "https://graph.facebook.com/v17.0/202102952990643/messages";
                //string accessToken = "EAAWZCTKsVyF8BO2NRelOi99jlTznZCrmukOpSoRJpwQrNDBqb3o0BNiW2nBZBJC8SITxpW2xBODYeFZBFHLMmgKaYo4RNnOzPCrTqPt2foxT156q5UoRvB8TGX6SaBLkyHzv5pBHplimwONHZBAbSoj59GPx6Kec55P7dMMZCFNGsZBEPxSZAE8p4M802gmuRxVUAr68heOizdA3H1w6SzxeHqFbj8iyBJxeYccr";
                string url = "https://graph.facebook.com/v18.0/105954558954427/messages";
                string accessToken = "EAAWZCTKsVyF8BO2NRelOi99jlTznZCrmukOpSoRJpwQrNDBqb3o0BNiW2nBZBJC8SITxpW2xBODYeFZBFHLMmgKaYo4RNnOzPCrTqPt2foxT156q5UoRvB8TGX6SaBLkyHzv5pBHplimwONHZBAbSoj59GPx6Kec55P7dMMZCFNGsZBEPxSZAE8p4M802gmuRxVUAr68heOizdA3H1w6SzxeHqFbj8iyBJxeYccr";

                //using (HttpClient client = new HttpClient())
                //{
                //    client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                //    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                //    var payload = new
                //    {
                //        messaging_product = "whatsapp",
                //        recipient_type = "individual",
                //        to = "+2348165680904",
                //        type = "template",
                //        template = new
                //        {
                //            name = "verification_code",
                //            language = new
                //            {
                //                code = "en_US"
                //            },
                //            components = new object[]
                //            {
                //        new
                //        {
                //            type = "body",
                //            parameters = new[]
                //            {
                //                new
                //                {
                //                    type = "text",
                //                    text = "J$FpnYnP"
                //                }
                //            }
                //        },
                //        new
                //        {
                //            type = "button",
                //            sub_type = "url",
                //            index = "0",
                //            parameters = new[]
                //            {
                //                new
                //                {
                //                    type = "text",
                //                    text = "J$FpnYnP"
                //                }
                //            }
                //        }
                //    }
                //        }
                //    };

                //    var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                //    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    
                    
                //    var response = await client.PostAsync(url, content);
                //    string responseContent = await response.Content.ReadAsStringAsync();
                //    if (response.IsSuccessStatusCode)
                //    {
                //        Console.WriteLine("Message sent successfully!");
                //    }
                //    else
                //    {
                //        Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
                //    }
                //}

            }
            catch (Exception ex)
            {
                _logger.Log($"whatsapp sent text to {userId} {ex.ToString()}");
            }
            return Page();
        }
    }
}
