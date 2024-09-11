using JurayKvV.Infrastructure.Interswitch.ResponseModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch
{
         public class GenerateTokenApp
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public GenerateTokenApp(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
        }

        public async Task<GeneratedTokenResponse> GetAccessTokenAsync(string grantType, string scope)
        {
            try
            {
                var tokenUrl = "https://apps.qa.interswitchng.com/passport/oauth/token";
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{grantType}:{scope}"));

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

                // Process the response content
                var paymentResponse = JsonConvert.DeserializeObject<GeneratedTokenResponse>(responseContent);

                // Now you can access the properties of paymentResponse
                return paymentResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
