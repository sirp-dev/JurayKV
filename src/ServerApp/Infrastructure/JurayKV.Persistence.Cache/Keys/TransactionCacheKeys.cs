using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class TransactionCacheKeys
    {
        public static string ListKey => "TransactionList";

        public static string GetKey(Guid transactionId)
        {
            return $"Transaction-{transactionId}";
        }
        public static string ListByCountUserIdKey(Guid userId)
        {
            return $"ListByCountUserId-{userId}";
        }
        public static string GetDetailsKey(Guid transactionId)
        {
            return $"TransactionDetails-{transactionId}";
        }
    }
}
