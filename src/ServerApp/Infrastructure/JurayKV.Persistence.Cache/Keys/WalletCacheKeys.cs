using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class WalletCacheKeys
    {
        public static string ListKey => "WalletList";

        public static string GetKey(Guid walletId)
        {
            return $"Wallet-{walletId}";
        }
        public static string GetUserKey(Guid userId)
        {
            return $"WalletUser-{userId}";
        }

        public static string GetDetailsKey(Guid walletId)
        {
            return $"WalletDetails-{walletId}";
        }
    }
}
