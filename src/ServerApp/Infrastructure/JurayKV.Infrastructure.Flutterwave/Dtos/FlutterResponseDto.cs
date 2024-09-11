using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace JurayKV.Infrastructure.Flutterwave.Dtos
{
    public class FlutterResponseDto
    {
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }

    }

    public class Data
    {
        public string link { get; set; }
    }
}
