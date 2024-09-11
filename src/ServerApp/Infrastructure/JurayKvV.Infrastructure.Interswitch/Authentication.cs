using JurayKvV.Infrastructure.Interswitch.ResponseModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch
{
    public class Authentication
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public Authentication(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
        }
        public async Task<TokenResponse> GetBillersAccessTokenAsync(string clientId, string clientSecret)
        {
            try
            {
                var tokenUrl = "https://apps.qa.interswitchng.com/passport/oauth/token";
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));

                var formData = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("scope", "profile"),

        });

                // Set Content-Type on the FormUrlEncodedContent instance, not on DefaultRequestHeaders
                formData.Headers.Clear();
                formData.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

                var response = await _httpClient.PostAsync(tokenUrl, formData);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Extract access_token from responseContent
                // You may want to use a JSON parsing library like Newtonsoft.Json for a more robust solution
                //TokenResponse Here, I'm just using a simple string manipulation for demonstration purposes
                // var accessToken = responseContent.Split('"')[3];
                // Process the response content
                TokenResponse accesstoken = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                 
                return accesstoken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            try {
                var tokenUrl = "https://passport.k8.isw.la/passport/oauth/token";
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));

                var formData = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

                // Set Content-Type on the FormUrlEncodedContent instance, not on DefaultRequestHeaders
                formData.Headers.Clear();
                formData.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

                var response = await _httpClient.PostAsync(tokenUrl, formData);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Extract access_token from responseContent
                // You may want to use a JSON parsing library like Newtonsoft.Json for a more robust solution
                // Here, I'm just using a simple string manipulation for demonstration purposes
                var accessToken = responseContent.Split('"')[3];

                return accessToken;
            }catch (Exception ex)
            {
                return null;
            }
        }
    }
}
