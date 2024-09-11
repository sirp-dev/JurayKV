using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.ResponseDto
{
   
    public class BalanceResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public BalanceResponseData data { get; set; }
    }

    public class BalanceResponseData
    {
        public string balance { get; set; }
        public string currency { get; set; }
    }

}
