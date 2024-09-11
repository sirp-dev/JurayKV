using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class SettingCacheKeys
    {
        public static string ListKey => "SettingList";
        public static string DefaultKey => "SettingDefault";

        public static string GetKey(Guid sliderId)
        {
            return $"Setting-{sliderId}";
        }

    }
}
