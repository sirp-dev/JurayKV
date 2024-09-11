using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
        internal static class AdvertRequestCacheKeys
    {
        public static string ListKey => "AdvertRequestList";

        public static string GetKey(Guid advertRequestId)
        {
            return $"AdvertRequest-{advertRequestId}";
        }
        public static string ListByCountUserIdKey(Guid userId)
        {
            return $"ListByCountUserId-{userId}";
        }
        public static string GetDetailsKey(Guid advertRequestId)
        {
            return $"AdvertRequestDetails-{advertRequestId}";
        }
    }

}
