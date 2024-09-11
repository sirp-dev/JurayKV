using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Services
{
    public class DataResponseDto
    {
        public Guid Id { get;set; }
        public bool BoolResponse { get;set; }
        public string Response { get;set; }
        public string DataResult { get;set;}
        public string Code { get;set; }
    }
}
