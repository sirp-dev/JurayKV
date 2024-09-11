using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Request
{
     

    public class VerifyAccountRequest
    {
        public string bankCode { get; set; }
        public string bankAccountNo { get; set; }
        public string countryCode { get; set; }
    }

}
