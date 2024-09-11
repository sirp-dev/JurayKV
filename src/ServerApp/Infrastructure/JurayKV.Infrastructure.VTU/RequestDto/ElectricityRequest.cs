using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.RequestDto
{
    public class ElectricityRequest
    {
        public string PhoneNumber { get; set; }
        public string MeterNumber { get; set; }
        public string ServiceId { get; set; }
        public string VariationId { get; set; }
        public string Amount { get; set; } 
    }
}
