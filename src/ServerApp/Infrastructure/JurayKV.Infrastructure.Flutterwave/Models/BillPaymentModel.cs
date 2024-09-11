using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Flutterwave.Models
{
     
        

        public class BillPaymentModel
    {
            public string country { get; set; }
            public string customer { get; set; }
            public string amount { get; set; }
            public string recurrence { get; set; }
            public string type { get; set; }
            public string reference { get; set; }
            public string biller_name { get; set; }
         }
     

}
