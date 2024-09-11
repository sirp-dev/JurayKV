using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class TransactionCacheHandler : ITransactionCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public TransactionCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid transactionId)
        {
            string detailsKey = TransactionCacheKeys.GetDetailsKey(transactionId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveGetAsync(Guid modelId)
        {
            string detailsKey = TransactionCacheKeys.GetDetailsKey(modelId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveList10ByUserAsync(Guid userId)
        {
            string detailsKey = TransactionCacheKeys.GetDetailsKey(userId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string transactionListKey = TransactionCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(transactionListKey);
        }
    }
}
