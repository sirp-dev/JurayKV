using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
  
    public class GenerateQrResponse
    {
        public string qrCodeId { get; set; }
        public string qrCodeIdMasterPass { get; set; }
        public string rawQRData { get; set; }
        public string transactionReference { get; set; }
    }

}
