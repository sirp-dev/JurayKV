using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Response
{
 

    public class TransactionResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public TransactionResponseData data { get; set; }
    }

    public class TransactionResponseData
    {
        public string reference { get; set; }
        public string orderNo { get; set; }
        public string cashierUrl { get; set; }
        public string status { get; set; }
        public TransactionResponseAmount amount { get; set; }
        public TransactionResponseVat vat { get; set; }
    }

    public class TransactionResponseAmount
    {
        public int total { get; set; }
        public string currency { get; set; }
    }

    public class TransactionResponseVat
    {
        public int total { get; set; }
        public string currency { get; set; }
    }



}
