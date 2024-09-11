using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class KvAdCacheHandler : IKvAdCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public KvAdCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveByBucketIdAsync(Guid bucketId)
        {
            string detailsKey = KvAdCacheKeys.ListByBucketIdKey(bucketId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveDetailsByIdAsync(Guid kvAdId)
        {
            string detailsKey = KvAdCacheKeys.GetDetailsKey(kvAdId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveGetAsync(Guid modelId)
        {
            string detailsKey = KvAdCacheKeys.GetKey(modelId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string kvAdListKey = KvAdCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(kvAdListKey);
        }
    }
}
