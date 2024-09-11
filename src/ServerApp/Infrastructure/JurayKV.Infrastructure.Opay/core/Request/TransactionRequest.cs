using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Opay.core.Request
{
    
    public class TransactionRequest
    {
        public string country { get; set; }
        public string reference { get; set; }
        public TransactionRequestAmount amount { get; set; }
        public string returnUrl { get; set; }
        public string callbackUrl { get; set; }
        public string cancelUrl { get; set; }
        public string displayName { get; set; }
        public int expireAt { get; set; }
        public string sn { get; set; }
        public TransactionRequestUserinfo userInfo { get; set; }
        public TransactionRequestProduct product { get; set; }
        public string payMethod { get; set; }
    }

    public class TransactionRequestAmount
    {
        public int total { get; set; }
        public string currency { get; set; }
    }

    public class TransactionRequestUserinfo
    {
        public string userEmail { get; set; }
        public string userId { get; set; }
        public string userMobile { get; set; }
        public string userName { get; set; }
    }

    public class TransactionRequestProduct
    {
        public string description { get; set; }
        public string name { get; set; }
    }




}
