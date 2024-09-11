using JurayKV.Infrastructure.Flutterwave.Dtos;
using JurayKV.Infrastructure.Flutterwave.Models;
using JurayKV.Infrastructure.Flutterwave.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Flutterwave.Repositories
{
    public class FlutterTransactionService : IFlutterTransactionService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configManager;
        public FlutterTransactionService(HttpClient client, IConfiguration configManager)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.DefaultRequestHeaders.Accept.Clear();
           
            _configManager = configManager;
        }

       
        public async Task<FlutterResponseDto> InitializeTransaction(PaymentRequestDto model)
        {
            string apiKey = _configManager.GetSection("FLWSECK-Key").Value;

            RestClientHelper restClientHelper = new RestClientHelper(apiKey);
            var request = new RestRequest(Method.POST);

            //// Combine base URL and endpoint to get the full URL
            request.Resource = restClientHelper.BuildEndpointUrl(Endpoints.Transaction);

            // Create TransactionResponseModel
            var transactionModel = new TransactionResponseModel
            {
                tx_ref = model.TxRef,
                amount = model.Amount,
                currency = model.Currency,
                redirect_url = model.RedirectUrl,
                payment_options = model.PaymentOptions,
                meta = new Models.Meta
                {
                    consumer_id = model.ConsumerId,
                    consumer_mac = model.ConsumerMac
                },
                customer = new Models.Customer
                {
                    email = model.Email,
                    phonenumber = model.PhoneNumber,
                    name = model.Name
                },
                customizations = new Customizations
                {
                    title = model.Title,
                    description = model.Description,
                    logo = model.Logo
                }
            };

            // Add the TransactionResponseModel to the request as JSON body
            request.AddJsonBody(transactionModel);

            var response = await Task.Run(() => restClientHelper.Execute<FlutterResponseDto>(request));

            return response.Data;
        }

        public async Task<InitializeTransactionTransferDto> InitializeTransactionTransfer(TransactionTransferInitialize model)
        {
             string apiKey = _configManager.GetSection("FLWSECK-Key").Value;

            RestClientHelper restClientHelper = new RestClientHelper(apiKey);
            var request = new RestRequest(Method.POST);

            //// Combine base URL and endpoint to get the full URL
            request.Resource = restClientHelper.BuildEndpointUrl(Endpoints.TransactionTransfer);

            // Create TransactionResponseModel
            var transactionModel = new TransactionTransferInitialize
            {
                tx_ref = model.tx_ref,
                amount = model.amount,
                email = model.email,
                phone_number = model.phone_number,
                currency = model.currency,
                client_ip = model.client_ip,
                device_fingerprint = model.device_fingerprint,
                narration = model.narration,
                is_permanent = model.is_permanent,
            };

            // Add the TransactionResponseModel to the request as JSON body
            request.AddJsonBody(transactionModel);

            var response = await Task.Run(() => restClientHelper.Execute<InitializeTransactionTransferDto>(request));

            return response.Data;
        }

        public async Task<BillPaymentDto> PayBill(BillPaymentModel model)
        {
            string apiKey = _configManager.GetSection("FLWSECK-Key").Value;

            RestClientHelper restClientHelper = new RestClientHelper(apiKey);
            var request = new RestRequest(Method.POST);

            //// Combine base URL and endpoint to get the full URL
            request.Resource = restClientHelper.BuildEndpointUrl(Endpoints.Bill);
             

            // Add the TransactionResponseModel to the request as JSON body
            request.AddJsonBody(model);

            var response = await Task.Run(() => restClientHelper.Execute<BillPaymentDto>(request));

            return response.Data;
        }

        public async Task<FlutterTransactionVerifyDto> VerifyTransaction(string tx_ref)
        {
            string apiKey = _configManager.GetSection("FLWSECK-Key").Value;

            RestClientHelper restClientHelper = new RestClientHelper(apiKey);
            var request = new RestRequest(Method.GET);

            //// Combine base URL and endpoint to get the full URL
            request.Resource = restClientHelper.BuildEndpointUrl(Endpoints.VerifyTransaction.Replace("{tx_ref}", tx_ref));
            var response = await Task.Run(() => restClientHelper.Execute<FlutterTransactionVerifyDto>(request));
            return response.Data;
        }
    }
}
