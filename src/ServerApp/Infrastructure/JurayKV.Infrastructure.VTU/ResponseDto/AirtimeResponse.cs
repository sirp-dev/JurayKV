using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.ResponseDto
{
     
    public class AirtimeResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public AirtimeResponseData data { get; set; }
    }

    public class AirtimeResponseData
    {
        public string network { get; set; }
        public string phone { get; set; }
        public string amount { get; set; }
        public string order_id { get; set; }
    }

}
