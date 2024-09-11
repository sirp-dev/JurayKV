using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.RequestDto
{
    public class AirtimeRequest
    {
        public string PhoneNumber { get; set; }
        public string Network { get; set; }
        public string Amount { get; set; }
    }
}
