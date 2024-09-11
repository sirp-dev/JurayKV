using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
  
    public class PaymentResponse
    {
        public int Amount { get; set; }
        public string CardNumber { get; set; }
        public string MerchantReference { get; set; }
        public string PaymentReference { get; set; }
        public string RetrievalReferenceNumber { get; set; }
        public object[] SplitAccounts { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string AccountNumber { get; set; }

    }

}
