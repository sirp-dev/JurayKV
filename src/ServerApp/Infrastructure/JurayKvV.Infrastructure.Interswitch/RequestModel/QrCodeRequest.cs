using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.RequestModel
{
   
    public class QrCodeRequest
    {
        public string amount { get; set; }
        public string surcharge { get; set; }
        public string currencyCode { get; set; }
        public string merchantTransactionReference { get; set; }
    }

}
