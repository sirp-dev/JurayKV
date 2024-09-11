using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class KvPointCacheKeys
    {
        public static string ListKey => "KvPointList"; 

        public static string ListBy10UserIdKey(Guid userId)
        {
            return $"ListBy10UserIdKey-{userId}";
        }
        public static string GetKey(Guid kvPointId)
        {
            return $"KvPoint-{kvPointId}";
        }

        public static string GetDetailsKey(Guid kvPointId)
        {
            return $"KvPointDetails-{kvPointId}";
        }

        public static string ListUserIdKey(Guid userId)
        {
            return $"ListByUserIdKey-{userId}";
        }

    }
}
