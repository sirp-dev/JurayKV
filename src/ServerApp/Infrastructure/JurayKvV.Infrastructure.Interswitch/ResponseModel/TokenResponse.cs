using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
   
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public TokenResponseMetadata metadata { get; set; }
        public string merchant_code { get; set; }
        public string requestor_id { get; set; }
        public string terminalId { get; set; }
        public string env { get; set; }
        public string payable_id { get; set; }
        public object client_description { get; set; }
        public string institution_id { get; set; }
        public string core_id { get; set; }
        public string[] api_resources { get; set; }
        public string client_name { get; set; }
        public object client_logo { get; set; }
        public string payment_code { get; set; }
        public string client_code { get; set; }
        public string jti { get; set; }
    }

    public class TokenResponseMetadata
    {
        public string institutionCode { get; set; }
    }

}
