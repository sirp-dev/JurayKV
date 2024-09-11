using JurayKV.Application.Infrastructures;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Services
{
    public sealed class SmsSender : ISmsSender
    {
        private readonly IConfiguration _configManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public SmsSender(IConfiguration configManager, UserManager<ApplicationUser> userManager)
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
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://my.kudisms.net/api/corporate");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(apiToken), "token");
                content.Add(new StringContent("KoboView"), "senderID");
                content.Add(new StringContent(user.PhoneNumber), "recipient");
                content.Add(new StringContent(smsMessage), "message");
                request.Content = content;
               
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SmsResponse>(responseBody);
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
