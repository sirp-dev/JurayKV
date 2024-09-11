using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
    

    public class VirtualAccountTransactionResponse
    {
        public string accountNumber { get; set; }
        public string bankName { get; set; }
        public int amount { get; set; }
        public string transactionReference { get; set; }
        public string responseCode { get; set; }
        public int validityPeriodMins { get; set; }
        public string accountName { get; set; }
    }

}
