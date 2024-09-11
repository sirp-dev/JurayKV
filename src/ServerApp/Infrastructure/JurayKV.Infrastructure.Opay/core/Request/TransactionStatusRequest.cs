using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Request
{
    public class TransactionStatusRequest
    {
        public string country { get; set; }
        public string reference { get; set; }
    }
}
