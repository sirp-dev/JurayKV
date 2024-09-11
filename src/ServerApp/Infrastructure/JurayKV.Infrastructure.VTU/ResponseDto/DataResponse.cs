using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.ResponseDto
{
   
    public class DataResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public DataResponseData data { get; set; }
    }

    public class DataResponseData
    {
        public string network { get; set; }
        public string data_plan { get; set; }
        public string phone { get; set; }
        public string amount { get; set; }
        public string order_id { get; set; }
    }

}
