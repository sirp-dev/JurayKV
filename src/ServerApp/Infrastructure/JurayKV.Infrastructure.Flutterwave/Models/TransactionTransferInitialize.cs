 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Flutterwave.Models
{
    public class TransactionTransferInitialize
    {
        public string tx_ref { get; set; }
        public string amount { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string currency { get; set; }
        public string client_ip { get; set; }
        public string device_fingerprint { get; set; }
        public string narration { get; set; }
        public bool is_permanent { get; set; }
    }
}
