using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Response
{
    

    public class TransactionStatusResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public TransactionStatusResponseData data { get; set; }
    }

    public class TransactionStatusResponseData
    {
        public string reference { get; set; }
        public string orderNo { get; set; }
        public string status { get; set; }
        public TransactionStatusResponseVat vat { get; set; }
        public TransactionStatusResponseAmount amount { get; set; }
        public long createTime { get; set; }
    }

    public class TransactionStatusResponseVat
    {
        public int total { get; set; }
        public string currency { get; set; }
    }

    public class TransactionStatusResponseAmount
    {
        public int total { get; set; }
        public string currency { get; set; }
    }

}
