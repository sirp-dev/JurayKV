using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Flutterwave.Validation
{
    public class RestClientHelper
    {
        private RestClient client;

        public RestClientHelper(string apiKey)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            client = new RestClient(Endpoints.BaseUrl);
            client.AddDefaultHeader("content-type", "application/json");
            client.AddDefaultHeader("authorization", apiKey);
        }

        public IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            return client.Execute<T>(request);
        }

        public string BuildEndpointUrl(string endpoint)
        {
            return $"{client.BaseUrl}/{endpoint.TrimStart('/')}";
        }
    }
}
