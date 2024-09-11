using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
     internal static class UserManagerCacheKeys
    {
        public static string ListKey => "UserManagerList";

       

        public static string GetDetailsKey(Guid userManagerId)
        {
            return $"UserManagerDetails-{userManagerId}";
        }
    }
}
