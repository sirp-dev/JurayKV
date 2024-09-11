using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
    

    public class UssdIssuersResponse
    {
        public USSDBanks[] USSDBanks { get; set; }
    }

    public class USSDBanks
    {
        public string name { get; set; }
        public string code { get; set; }
        public string cbnCode { get; set; }
    }

}
