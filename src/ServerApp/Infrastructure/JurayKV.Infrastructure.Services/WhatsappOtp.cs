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
using Twilio.Rest.Verify.V2.Service;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using RestSharp;
using System.Text.RegularExpressions;

namespace JurayKV.Infrastructure.Services
{
    public sealed class WhatsappOtp : IWhatsappOtp
    {
        private readonly LoggerLibrary _logger;

        private readonly IConfiguration _configManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public WhatsappOtp(IConfiguration configManager, UserManager<ApplicationUser> userManager, LoggerLibrary logger)
        {
            _configManager = configManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> SendAsync(string smsMessage, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            try
            {
               
                if (user == null)
                {
                    return false;
                }

                string phoneNumberWithPlus = user.PhoneNumber; // Replace with the actual phone number

                string formattedPhoneNumber = FormatToNigeria(phoneNumberWithPlus);
                if (!String.IsNullOrEmpty(formattedPhoneNumber))
                {
                    //string instanceId = "instance74902"; // your instanceId
                    //string token = "k5a6qlezjsff9zmt";  
                    //string message = smsMessage;
                    //var url = "https://api.ultramsg.com/" + instanceId + "/messages/chat";
                    //var client = new RestClient(url);
                    //var request = new RestRequest(url, Method.POST);
                    //request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    //request.AddParameter("token", token);
                    //request.AddParameter("to", formattedPhoneNumber);
                    //request.AddParameter("body", message);


                    //var response = await client.ExecuteAsync(request);
                    //var output = response.Content;



                    using (HttpClient client = new HttpClient())
                    {
                        string accessToken = "EAAWZCTKsVyF8BO7VZBfFZBzmsZArtUMlif1Ahw6yRKXDFGQzxFbPcQP9ox6fr8UkFdwrZAG7QkTtZAzpeGpipHfnqPsNhSZBZCnRTw5zru5ZC5pyZCI8XLxxLU3n3ftk8N6IzUY4PauknfJ3BsyCQXQXVFj2sDV5FAW6tFahzo1qoEZBkPw9DQyXzhfEoIZCzdYXmFyeI2h3K0bZAMoR3ZBfz1uFGxNZCBZBfSYPoM2l4clqpNIZD";
                        //string accessToken = "EAAWZCTKsVyF8BO4Dd7Oqzkd4rU7HQCSamFsgPjFZBiPkKgjy0ZAVYlnj75uM0Hroed5n4V89DPPRrGbucO8GimRxYlnmrteAourPeEyKdu6Y6ZBRucBaesq0sUHZCGWnZBivMMCNKZCYbKCHmgeOAKVmPQTZCYbTeZC0YRjh7e5eTHe0ZASNZBh6rwdCQC8Jl25juasJB2cvFJc5BoCp78M";

                        //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                        //var message = new FacebookMessage
                        //{
                        //    messaging_product = "whatsapp",
                        //    recipient_type = "individual",
                        //    to = formattedPhoneNumber,
                        //    type = "text",
                        //    text = new TextObject
                        //    {
                        //        preview_url = false,
                        //        body = smsMessage
                        //    }
                        //};
                        var message = new
                        {
                            messaging_product = "whatsapp",
                            to = formattedPhoneNumber, 
                            text = new
                            {
                                body = smsMessage
                            } };
                        string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(message);
                        StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                        string url = $"https://graph.facebook.com/v18.0/202102952990643/messages";
                        //HttpResponseMessage response = await client.PostAsync(url, content);
                        var request = new HttpRequestMessage(HttpMethod.Post, url);
                        request.Headers.Add("Authorization", "Bearer " + accessToken);
                        request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                        var response = await client.SendAsync(request);

                        Console.WriteLine($"Status Code: {response.StatusCode}");
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Response Content: {responseContent}");

                        _logger.Log($"whatsapp sent text to {user.Email}");

                        return true;
                    }
                }
                return false;
            }
            catch (Exception c)
            {

            _logger.Log($"whatsapp sent text to {user.Email} {c.ToString()}");
            }
            return false;
        }

        static string FormatToNigeria(string phoneNumber)
        {
            try
            {
                // Remove any non-numeric characters from the phone number
                string numericPhoneNumber = Regex.Replace(phoneNumber, @"[^\d]", "");

                // Check if the phone number starts with "+234" and return as is
                if (numericPhoneNumber.StartsWith("234"))
                {
                    return "+" + numericPhoneNumber;
                }

                // Check if the phone number starts with "0" and replace it with "+234"
                if (numericPhoneNumber.StartsWith("0"))
                {
                    numericPhoneNumber = "+234" + numericPhoneNumber.Substring(1);
                }

                return numericPhoneNumber;
            }
            catch (Exception c)
            {

                return "";
            }
        }
    }
    class FacebookMessage
    {
        public string messaging_product { get; set; }
        public string recipient_type { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public TextObject text { get; set; }
    }

    class TextObject
    {
        public bool preview_url { get; set; }
        public string body { get; set; }
    }
}
