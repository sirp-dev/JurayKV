using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class CompanyCacheKeys
    {
        public static string ListKey => "CompanyList";
        public static string SelectListKey => "CompanySelectList";

        public static string GetKey(Guid companyId)
        {
            return $"Company-{companyId}";
        }
        public static string GetUserKey(Guid userId)
        {
            return $"CompanyDetails-{userId}";
        }

        public static string GetDetailsKey(Guid companyId)
        {
            return $"CompanyDetails-{companyId}";
        }
    }
}
