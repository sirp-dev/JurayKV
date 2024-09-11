using JurayKV.Infrastructure.Opay.core.Request;
using JurayKV.Infrastructure.Opay.core.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace JurayKV.Infrastructure.Opay.core.Repositories
{
    public class OpayRepository : IOpayRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string BASEURL;
        private readonly string MERCHANTID;
        private readonly string PUBLICKEY;
        private readonly string PRIVATEKEY;


        public OpayRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            // Initialize constants
            BASEURL = _configuration["OpayInit:BaseUrl"];
            MERCHANTID = _configuration["OpayInit:MarchantId"];
            PUBLICKEY = _configuration["OpayInit:PublicKey"];
            PRIVATEKEY = _configuration["OpayInit:PrivateKey"];
        }

        public async Task<TransactionResponse> initializeTransaction(TransactionRequest model)
        {
            TransactionResponse dataResult = new TransactionResponse();

            try
            {

                using (HttpClient client = new HttpClient())
                {
                    var dataString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var content = new StringContent(dataString, System.Text.Encoding.UTF8, "application/json");

                    // Do not add Content-Type to DefaultRequestHeaders

                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + PUBLICKEY);
                    client.DefaultRequestHeaders.Add("MerchantId", MERCHANTID);

                    var response = await client.PostAsync(BASEURL + "/api/v1/international/cashier/create", content);
                    var result = await response.Content.ReadAsStringAsync();

                    // Parse the response content and return the TransactionResponse
                    // (You'll need to implement this part based on your specific response structure)
                    dataResult = JsonConvert.DeserializeObject<TransactionResponse>(result);
                    return dataResult;
                }

            }
            catch (Exception c)
            {
                dataResult.message = "FAIL";
                return dataResult;
            }
        }
        private string CalculateHmacSignature(string data, string privateKey)
        {
            using (var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(privateKey)))
            {
                var hash = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        public async Task<TransactionStatusResponse> transactionStatus(TransactionStatusRequest model)
        {
            TransactionStatusResponse dataResult = new TransactionStatusResponse();

            try
            {

                var dataString = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                using (HttpClient client = new HttpClient())
                {
                    // Calculate HMAC signature
                    var hmacSignature = CalculateHmacSignature(dataString, PRIVATEKEY);

                    var content = new StringContent(dataString, Encoding.UTF8, "application/json");

                    // Do not add Content-Type to DefaultRequestHeaders
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + hmacSignature);
                    client.DefaultRequestHeaders.Add("MerchantId", MERCHANTID);

                    var response = await client.PostAsync(BASEURL + "/api/v1/international/cashier/status", content);
                    var result = await response.Content.ReadAsStringAsync();

                    // Parse the response content and return the TransactionResponse
                    // (You'll need to implement this part based on your specific response structure)
                     dataResult = JsonConvert.DeserializeObject<TransactionStatusResponse>(result);
                    return dataResult;
                }

            }
            catch (Exception c)
            {
                dataResult.message = "FAIL";
                return dataResult;
            }
        }

        public Task WebhookResquest(WebhookResponse model)
        {
            throw new NotImplementedException();
        }

        public async Task<BankListResponse> getBankList()
        {
             BankListResponse dataResult = new BankListResponse();

            try
            {
                 

                using (HttpClient client = new HttpClient())
                { 
                    BankListRequest model = new BankListRequest();
                    model.countryCode = "NG";

                    var dataString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var content = new StringContent(dataString, System.Text.Encoding.UTF8, "application/json");

                    // Do not add Content-Type to DefaultRequestHeaders
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + PUBLICKEY);
                    client.DefaultRequestHeaders.Add("MerchantId", MERCHANTID);
                     
                    var response = await client.PostAsync("https://cashierapi.opayweb.com/api/v3/banks", content);
                    var result = await response.Content.ReadAsStringAsync();

                    // Parse the response content and return the BankListResponse array
                    dataResult = JsonConvert.DeserializeObject<BankListResponse>(result);
                    return dataResult;
                }
            }
            catch (Exception c)
            {
                 
                return dataResult;
            }
        }

        public async Task<VerifyAccountResponse> verifyBankAccount(VerifyAccountRequest model)
        {
            VerifyAccountResponse dataResult = new VerifyAccountResponse();

            try
            {


                using (HttpClient client = new HttpClient())
                {
                     

                    var dataString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var content = new StringContent(dataString, System.Text.Encoding.UTF8, "application/json");

                    // Do not add Content-Type to DefaultRequestHeaders
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + PUBLICKEY);
                    client.DefaultRequestHeaders.Add("MerchantId", MERCHANTID);

                    var response = await client.PostAsync("https://cashierapi.opayweb.com/api/v3/verification/accountNumber/resolve", content);
                    var result = await response.Content.ReadAsStringAsync();

                    // Parse the response content and return the BankListResponse array
                    dataResult = JsonConvert.DeserializeObject<VerifyAccountResponse>(result);
                    return dataResult;
                }
            }
            catch (Exception c)
            {

                return dataResult;
            }
        }

        public async Task<TransferToBankResponse> transferToBank(TransferToBankRequest model)
        {
            TransferToBankResponse dataResult = new TransferToBankResponse();

            try
            {


                using (HttpClient client = new HttpClient())
                {


                    var dataString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var content = new StringContent(dataString, System.Text.Encoding.UTF8, "application/json");

                    // Do not add Content-Type to DefaultRequestHeaders
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + PUBLICKEY);
                    client.DefaultRequestHeaders.Add("MerchantId", MERCHANTID);

                    var response = await client.PostAsync("https://cashierapi.opayweb.com/api/v3/transfer/toBank", content);
                    var result = await response.Content.ReadAsStringAsync();

                    // Parse the response content and return the BankListResponse array
                    dataResult = JsonConvert.DeserializeObject<TransferToBankResponse>(result);
                    return dataResult;
                }
            }
            catch (Exception c)
            {

                return dataResult;
            }
        }
    }


}
