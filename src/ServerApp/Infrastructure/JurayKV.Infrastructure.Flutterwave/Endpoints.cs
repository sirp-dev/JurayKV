using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Flutterwave
{
    internal class Endpoints
    {

        public const string BaseUrl = "https://api.flutterwave.com";

         


        public const string Transaction = "/v3/payments";
        public const string VerifyTransaction = "/v3/transactions/{tx_ref}/verify";
        public const string TransactionTransfer = "/v3/charges?type=bank_transfer";
        public const string Bill = "v3/bills";




    }
}
