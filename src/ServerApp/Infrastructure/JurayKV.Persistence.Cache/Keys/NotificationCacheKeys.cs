using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
   
    public static class NotificationCacheKeys
    {
        public static string ListKey => "NotificationList";
        public static string GetKey(Guid modelId)
        {
            return $"Notification-{modelId}";
        }

        public static string GetDetailsKey(Guid modelId)
        {
            return $"NotificationDetails-{modelId}";
        }
    }
}
