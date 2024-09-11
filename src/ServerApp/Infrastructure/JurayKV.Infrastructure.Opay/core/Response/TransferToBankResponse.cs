using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Response
{
    
    public class TransferToBankResponse
    {
        public string reference { get; set; }
        public string orderNo { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string fee { get; set; }
        public string status { get; set; }
        public string failureReason { get; set; }
        public string bankCode { get; set; }
        public string bankAccountNumber { get; set; }
    }

}
