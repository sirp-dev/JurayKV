using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Request
{
     
    public class BankListResponse
{
        public string code { get; set; }
        public string message { get; set; }
        public BankListResponseDatum[] data { get; set; }
    }

    public class BankListResponseDatum
{
        public string code { get; set; }
        public string altCode { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public object subscriptIcon { get; set; }
        public string color { get; set; }
        public bool isSupportBankAccount { get; set; }
        public object opayBank { get; set; }
    }

}
