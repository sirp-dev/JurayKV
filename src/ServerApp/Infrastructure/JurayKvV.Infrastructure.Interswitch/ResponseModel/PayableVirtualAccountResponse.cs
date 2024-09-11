using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
    

    public class PayableVirtualAccountResponse
    {
        public int id { get; set; }
        public string merchantCode { get; set; }
        public string payableCode { get; set; }
        public bool enabled { get; set; }
        public long dateCreated { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public string bankName { get; set; }
        public string bankCode { get; set; }
    }

}
