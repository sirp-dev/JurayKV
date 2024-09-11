using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Flutterwave.Dtos
{
      public class  BillPaymentDto
    {
        public string status { get; set; }
        public string message { get; set; }
        public BillData data { get; set; }
    }

    public class BillData
    {
        public string phone_number { get; set; }
        public int amount { get; set; }
        public string network { get; set; }
        public string flw_ref { get; set; }
        public string tx_ref { get; set; }
        public object reference { get; set; }
    }
}
