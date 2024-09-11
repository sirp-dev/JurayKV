using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Flutterwave.Dtos
{
    

    public class InitializeTransactionTransferDto
    {
        public string status { get; set; }
        public string message { get; set; }
        public Meta meta { get; set; }
    }

    public class Meta
    {
        public Authorization authorization { get; set; }
    }

    public class Authorization
    {
        public string transfer_reference { get; set; }
        public string transfer_account { get; set; }
        public string transfer_bank { get; set; }
        public string account_expiration { get; set; }
        public string transfer_note { get; set; }
        public string transfer_amount { get; set; }
        public string mode { get; set; }
    }

}
