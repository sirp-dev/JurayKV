using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.RequestModel
{
     
    public class PayableVirtualAccountRequest
    {
        public string accountName { get; set; }
        public string merchantCode { get; set; }
    }

}
