using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.ResponseDto
{
    
    public class CableTvResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public CableTvResponseData data { get; set; }
    }

    public class CableTvResponseData
    {
        public string cable_tv { get; set; }
        public string subscription_plan { get; set; }
        public string smartcard_number { get; set; }
        public string phone { get; set; }
        public string amount { get; set; }
        public string amount_charged { get; set; }
        public string service_fee { get; set; }
        public string order_id { get; set; }
    }

}
