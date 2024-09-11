using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmarkEmailService
{
    public interface ISimpleHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}