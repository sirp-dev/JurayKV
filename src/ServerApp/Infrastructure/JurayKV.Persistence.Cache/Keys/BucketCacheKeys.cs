using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class BucketCacheKeys
    {
        public static string ListKey => "BucketList";
        public static string SelectListKey => "BucketSelectList";


        public static string GetKey(Guid bucketId)
        {
            return $"Bucket-{bucketId}";
        }

        public static string GetDetailsKey(Guid bucketId)
        {
            return $"BucketDetails-{bucketId}";
        }
    }
}
