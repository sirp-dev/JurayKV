using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.RequestModel
{
    public class SendBillPaymentRequest
    {

        public string TerminalId { get; set; }
        public string paymentCode { get; set; }
        public string customerId { get; set; }
        public string customerMobile { get; set; }
        public string customerEmail { get; set; }
        public string amount { get; set; }
        public string requestReference { get; set; }

    }
}
