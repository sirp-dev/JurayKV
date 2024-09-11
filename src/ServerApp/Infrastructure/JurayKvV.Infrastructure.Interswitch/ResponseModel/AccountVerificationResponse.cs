using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
   
    public class AccountVerificationResponse
    {
        public string AccountName { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseCodeGrouping { get; set; }
    }

}
