using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.ResponseDto
{
    
    public class ElectricityResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public Guid TransactionId { get; set; }
        public ElectricityResponseData data { get; set; }
    }

    public class ElectricityResponseData
    {
        public string electricity { get; set; }
        public string meter_number { get; set; }
        public string token { get; set; }
        public string units { get; set; }
        public string phone { get; set; }
        public string amount { get; set; }
        public string amount_charged { get; set; }
        public string order_id { get; set; }
    }

}
