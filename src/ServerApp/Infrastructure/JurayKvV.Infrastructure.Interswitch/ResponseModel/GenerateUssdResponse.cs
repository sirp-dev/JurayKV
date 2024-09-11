using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
    
    public class GenerateUssdResponse
    {
        public string responseDescription { get; set; }
        public string logId { get; set; }
        public bool isRedirect { get; set; }
        public object[] additionalInfo { get; set; }
    }

}
