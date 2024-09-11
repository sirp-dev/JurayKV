using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class BucketCacheHandler : IBucketCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public BucketCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid bucketId)
        {
            string detailsKey = BucketCacheKeys.GetDetailsKey(bucketId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveDropdownListAsync()
        {
            string bucketListKey = BucketCacheKeys.SelectListKey;
            await _distributedCache.RemoveAsync(bucketListKey);
        }

        public async Task RemoveGetByIdAsync(Guid bucketId)
        {
            string detailsKey = BucketCacheKeys.GetKey(bucketId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string bucketListKey = BucketCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(bucketListKey);
        }
    }
}
