using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class KvAdCacheKeys
    {
        public static string ListKey => "KvAdList";

        public static string GetKey(Guid kvAdId)
        {
            return $"KvAd-{kvAdId}";
        }

        public static string GetDetailsKey(Guid kvAdId)
        {
            return $"KvAdDetails-{kvAdId}";
        }

        public static string ListByBucketIdKey(Guid bucketId)
        {
            return $"ListByBucketId-{bucketId}";
        }
    }
}
