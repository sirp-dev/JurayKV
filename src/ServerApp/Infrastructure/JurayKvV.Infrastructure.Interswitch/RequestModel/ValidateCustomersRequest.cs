using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.RequestModel
{
    public class ValidateCustomersRequest
    {
        public List<CustomerInfo> Customers { get; set; }
        public string TerminalId { get; set; }
    }

    public class CustomerInfo
    {
        public string PaymentCode { get; set; }
        public string CustomerId { get; set; }
    }

}
