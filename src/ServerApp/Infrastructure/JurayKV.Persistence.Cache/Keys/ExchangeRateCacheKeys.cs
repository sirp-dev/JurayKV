using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class ExchangeRateCacheKeys
    {
        public static string ListKey => "ExchangeRateList";
 
        public static string GetKey(Guid exchangeRateId)
        {
            return $"ExchangeRate-{exchangeRateId}";
        }
        public static string GetLaskKey()
        {
            return $"LastExchangeRate";
        }

        public static string GetDetailsKey(Guid exchangeRateId)
        {
            return $"ExchangeRateDetails-{exchangeRateId}";
        }
    }
}
