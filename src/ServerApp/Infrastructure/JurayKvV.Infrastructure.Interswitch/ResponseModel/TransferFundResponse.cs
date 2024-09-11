using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
    public class TransferFundResponse
    {
        public string MAC { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionReference { get; set; }
        public string Pin { get; set; }
        public string TransferCode { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseCodeGrouping { get; set; }
    }
}
