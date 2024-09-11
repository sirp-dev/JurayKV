using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
   
    public class BankListResponse
    {
        public BankListResponseBank[] Banks { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseCodeGrouping { get; set; }
    }

    public class BankListResponseBank
    {
        public int Id { get; set; }
        public string CbnCode { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

}
