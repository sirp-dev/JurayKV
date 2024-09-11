using JurayKV.Application.Infrastructures;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Services
{
    public sealed class VoiceSender : IVoiceSender
    {
        private readonly IConfiguration _configManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public VoiceSender(IConfiguration configManager, UserManager<ApplicationUser> userManager)
        {
            _configManager = configManager;
            _userManager = userManager;
        }

        public async Task<bool> SendAsync(string smsMessage, string id)
        {
            string apiToken = _configManager.GetValue<string>("SmsApiToken");
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return false;
                }
                string vmessage = smsMessage + "   "+ smsMessage;
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://kudisms.vtudomain.com/api/texttospeech");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(apiToken), "token");
                content.Add(new StringContent("2347060530000"), "callerID");
                content.Add(new StringContent(user.PhoneNumber), "recipients");
                content.Add(new StringContent(vmessage), "message");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<VoiceResponse>(responseBody);
                if (result.status == "success")
                {
                    return true;
                }
                return false;
            }
            catch (Exception c)
            {

            }
            return false;
        }
    }
}
