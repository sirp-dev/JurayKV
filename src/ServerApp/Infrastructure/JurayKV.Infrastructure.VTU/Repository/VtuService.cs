using JurayKV.Infrastructure.VTU.RequestDto;
using JurayKV.Infrastructure.VTU.ResponseDto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.Repository
{
    public class VtuService : IVtuService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string BASEURL;
        private readonly string USERNAME;
        private readonly string PASSWORD;


        public VtuService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            // Initialize constants
            BASEURL = _configuration["VTU:BaseUrl"];
            USERNAME = _configuration["VTU:Username"];
            PASSWORD = _configuration["VTU:SecretKey"];
        }

        public async Task<AirtimeResponse> Airtime(AirtimeRequest request)
        {
            AirtimeResponse dataResult = new AirtimeResponse();

            using (HttpClient client = new HttpClient())
            {
                string requestUrl = $"{BASEURL}airtime?username={USERNAME}&password={PASSWORD}&phone={request.PhoneNumber}&network_id={request.Network}&amount={request.Amount}";

                var response = await client.GetAsync(requestUrl);
                var result = await response.Content.ReadAsStringAsync();

                // Parse the response content and return the TransactionResponse
                // (You'll need to implement this part based on your specific response structure)
                dataResult = JsonConvert.DeserializeObject<AirtimeResponse>(result);
                return dataResult;
            }

            //using (HttpClient client = new HttpClient())
            //{
            //    // Construct the request URL with parameters
            //    //https://vtu.ng/wp-json/api/v1/airtime?username=Frank&password=123456&phone=07045461790&network_id=mtn&amount=2000
            //    string requestUrl = $"{BASEURL}airtime?username={USERNAME}&password={PASSWORD}&phone={request.PhoneNumber}&network_id={request.Network}&amount={request.Amount}";

            //    // Send the GET request
            //    HttpResponseMessage response = await client.GetAsync(requestUrl);

            //    // Check if the request was successful (status code 200-299)
            //    if (response.IsSuccessStatusCode)
            //    {
            //        // Read and parse the response content
            //        string result = await response.Content.ReadAsStringAsync();
            //        dataResult = JsonConvert.DeserializeObject<AirtimeResponse>(result);
            //        return dataResult;

            //    }
            //    else
            //    {
            //        // Handle error response
            //        string errorContent = await response.Content.ReadAsStringAsync();
            //        Console.WriteLine($"Error response: {errorContent}");
            //        return null;
            //    }
            //}
        }

        public async Task<BalanceResponse> Balance()
        {
            BalanceResponse dataResult = new BalanceResponse();

            using (HttpClient client = new HttpClient())
            {
                // Construct the request URL with parameters
                string requestUrl = $"{BASEURL}balance?username={USERNAME}&password={PASSWORD}";

                // Send the GET request
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response content
                    string result = await response.Content.ReadAsStringAsync();
                    dataResult = JsonConvert.DeserializeObject<BalanceResponse>(result);
                    return dataResult;

                }
                else
                {
                    // Handle error response
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response: {errorContent}");
                    return null;
                }
            }
        }

        public async Task<DataResponse> DataRequest(DataRequest request)
        {
            DataResponse dataResult = new DataResponse();

            using (HttpClient client = new HttpClient())
            {
                // Construct the request URL with parameters
                //https://vtu.ng/wp-json/api/v1/data?username=Frank&password=123456&phone=07045461790&network_id=mtn&variation_id=M1024
                string requestUrl = $"{BASEURL}data?username={USERNAME}&password={PASSWORD}&phone={request.PhoneNumber}&network_id={request.Network}&variation_id={request.VariationId}";

                // Send the GET request
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response content
                    string result = await response.Content.ReadAsStringAsync();
                    dataResult = JsonConvert.DeserializeObject<DataResponse>(result);
                    return dataResult;

                }
                else
                {
                    // Handle error response
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response: {errorContent}");
                    dataResult.code = "null";
                    return dataResult;
                }
            }
        }

        public async Task<CableTvResponse> SubscribeTV(CableTvRequest request)
        {
            CableTvResponse dataResult = new CableTvResponse();

            using (HttpClient client = new HttpClient())
            {
                // Construct the request URL with parameters
                //https://vtu.ng/wp-json/api/v1/tv?username=Frank&password=123456&phone=07045461790&service_id=gotv&smartcard_number=7032400086&variation_id=gotv-max
                string requestUrl = $"{BASEURL}tv?username={USERNAME}&password={PASSWORD}&phone={request.PhoneNumber}&service_id={request.ServiceId}&smartcard_number={request.CustomerId}&variation_id={request.VariationId}";

                // Send the GET request
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response content
                    string result = await response.Content.ReadAsStringAsync();
                    dataResult = JsonConvert.DeserializeObject<CableTvResponse>(result);
                    return dataResult;

                }
                else
                {
                    // Handle error response
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response: {errorContent}");
                    return null;
                }
            }
        }

        public async Task<ElectricityResponse> Electricity(ElectricityRequest request)
        {
            ElectricityResponse dataResult = new ElectricityResponse();

            using (HttpClient client = new HttpClient())
            {
                // Construct the request URL with parameters

                 string requestUrl = $"{BASEURL}electricity?username={USERNAME}&password={PASSWORD}&phone={request.PhoneNumber}&meter_number={request.MeterNumber}&service_id={request.ServiceId}&variation_id={request.VariationId}&amount={request.Amount}";

                // Send the GET request
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response content
                    string result = await response.Content.ReadAsStringAsync();
                    dataResult = JsonConvert.DeserializeObject<ElectricityResponse>(result);
                    return dataResult;

                }
                else
                {
                    // Handle error response
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response: {errorContent}");
                    return null;
                }
            }
        }

        public async Task<VerifyResponse> VerifyCustomerUtility(VerifyRequest request)
        {
            VerifyResponse dataResult = new VerifyResponse();

            using (HttpClient client = new HttpClient())
            {
                // Construct the request URL with parameters
                 string requestUrl = $"{BASEURL}verify-customer?username={USERNAME}&password={PASSWORD}&customer_id={request.CustomerId}&service_id={request.ServiceId}&variation_id={request.VariationId}";

                // Send the GET request
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response content
                    string result = await response.Content.ReadAsStringAsync();
                    dataResult = JsonConvert.DeserializeObject<VerifyResponse>(result);
                    return dataResult;

                }
                else
                {
                    // Handle error response
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response: {errorContent}");
                    return null;
                }
            }
        }
    }
}
