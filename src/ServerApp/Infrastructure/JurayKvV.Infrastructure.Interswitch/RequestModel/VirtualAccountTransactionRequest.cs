using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.RequestModel
{
    
    public class VirtualAccountTransactionRequest
    {
        public string merchantCode { get; set; }
        public string payableCode { get; set; }
        public string currencyCode { get; set; }
        public string amount { get; set; }
        public string accountName { get; set; }
        public string transactionReference { get; set; }
    }

}
