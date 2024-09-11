using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Request
{
      public class TransferToBankRequest
    {
        public string reference { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string country { get; set; }
        public TransferToBankReceiver receiver { get; set; }
        public string reason { get; set; }
    }

    public class TransferToBankReceiver
    {
        public string name { get; set; }
        public string bankCode { get; set; }
        public string bankAccountNumber { get; set; }
    }

}
