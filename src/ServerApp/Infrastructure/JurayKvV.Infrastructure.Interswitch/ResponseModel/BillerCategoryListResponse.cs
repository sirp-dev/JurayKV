using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{
    
    public class BillerCategoryListResponse
    {
        public BillerCategoryListResponseBillercategory[] BillerCategories { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseCodeGrouping { get; set; }
    }

    public class BillerCategoryListResponseBillercategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public object[] Billers { get; set; }
    }

}
