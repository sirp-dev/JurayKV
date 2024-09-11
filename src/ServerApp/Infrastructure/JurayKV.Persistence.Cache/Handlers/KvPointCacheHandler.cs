using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class KvPointCacheHandler : IKvPointCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public KvPointCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid kvPointId)
        {
            string detailsKey = KvPointCacheKeys.GetDetailsKey(kvPointId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveGetAsync(Guid modelId)
        {
            string detailsKey = KvPointCacheKeys.GetKey(modelId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string kvPointListKey = KvPointCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(kvPointListKey);
        }

        public async Task RemoveListBy10ByUserAsync(Guid userId)
        {
            string detailsKey = KvPointCacheKeys.ListBy10UserIdKey(userId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListByUserAsync(Guid userId)
        {
            string kvPointListKey = KvPointCacheKeys.ListUserIdKey(userId);
            await _distributedCache.RemoveAsync(kvPointListKey);
        }
    }
}
