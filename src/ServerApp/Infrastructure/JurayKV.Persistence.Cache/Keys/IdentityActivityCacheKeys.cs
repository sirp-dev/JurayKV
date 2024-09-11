using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class IdentityActivityCacheKeys
    {
        public static string ListKey => "IdentityActivityList";

        public static string GetKey(Guid identityActivityId)
        {
            return $"IdentityActivity-{identityActivityId}";
        }

        public static string GetDetailsKey(Guid identityActivityId)
        {
            return $"IdentityActivityDetails-{identityActivityId}";
        }
    }
}
