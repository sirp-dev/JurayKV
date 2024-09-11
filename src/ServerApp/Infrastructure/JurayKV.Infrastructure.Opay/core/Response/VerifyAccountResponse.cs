using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Response
{
   
    public class VerifyAccountResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public VerifyAccountResponseData data { get; set; }
    }

    public class VerifyAccountResponseData
    {
        public string accountNo { get; set; }
        public string accountName { get; set; }
    }

}
