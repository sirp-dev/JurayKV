using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class IdentityKvAdCacheKeys
    {
        public static string ListKey => "IdentityKvAdList";
        public static string ListActiveKey => "IdentityKvAdActiveList";

        public static string GetKey(Guid identityKvAdId)
        {
            return $"IdentityKvAd-{identityKvAdId}";
        }

        public static string GetDetailsKey(Guid identityKvAdId)
        {
            return $"IdentityKvAdDetails-{identityKvAdId}";
        }

        public static string GetActiveByUserIdKey(Guid userId)
        {
            return $"GetActiveByUserId-{userId}";
        }


        public static string GetByUserIdKey(Guid userId)
        {
            return $"GetByUserId-{userId}";
        }
    }
}
