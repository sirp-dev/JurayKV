using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.ResponseDto
{
    
    public class VerifyResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public VerifyResponseData data { get; set; }
    }

    public class VerifyResponseData
    {
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_arrears { get; set; }
        public object decoder_status { get; set; }
        public object decoder_due_date { get; set; }
        public object decoder_balance { get; set; }
    }

}
