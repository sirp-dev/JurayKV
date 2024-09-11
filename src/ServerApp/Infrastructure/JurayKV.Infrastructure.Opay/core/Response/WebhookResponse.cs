using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Response
{
   
    public class WebhookResponse
    {
        public WebhookResponsePayload payload { get; set; }
        public string sha512 { get; set; }
        public string type { get; set; }
    }

    public class WebhookResponsePayload
    {
        public string amount { get; set; }
        public string channel { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public string displayedFailure { get; set; }
        public string fee { get; set; }
        public string feeCurrency { get; set; }
        public string instrumentType { get; set; }
        public string reference { get; set; }
        public bool refunded { get; set; }
        public string status { get; set; }
        public DateTime timestamp { get; set; }
        public string token { get; set; }
        public string transactionId { get; set; }
        public DateTime updated_at { get; set; }
    }

}
