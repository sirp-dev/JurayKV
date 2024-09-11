using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class ExchangeRateCacheHandler : IExchangeRateCacheHandler
    {
        private readonly IDistributedCache _distributedCache;

        public ExchangeRateCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid exchangeRateId)
        {
            string detailsKey = ExchangeRateCacheKeys.GetDetailsKey(exchangeRateId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string exchangeRateListKey = ExchangeRateCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(exchangeRateListKey);
        }
        public async Task RemoveLastDetailsAsync()
        {
            string exchangeRateListKey = ExchangeRateCacheKeys.GetLaskKey();
            await _distributedCache.RemoveAsync(exchangeRateListKey);
        }

        public async Task RemoveGetAsync(Guid exchangeRateId)
        {
            string detailsKey = ExchangeRateCacheKeys.GetDetailsKey(exchangeRateId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveLastListAsync()
        {
            string exchangeRateListKey = ExchangeRateCacheKeys.GetLaskKey();
            await _distributedCache.RemoveAsync(exchangeRateListKey);
        }
    }
}
