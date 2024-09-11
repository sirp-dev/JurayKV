using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class IdentityKvAdCacheHandler : IIdentityKvAdCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public IdentityKvAdCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid identityKvAdId)
        {
            string detailsKey = IdentityKvAdCacheKeys.GetDetailsKey(identityKvAdId);
            await _distributedCache.RemoveAsync(detailsKey);
             
        }
        public async Task RemoveGetAsync(Guid identityKvAdId)
        { 

            string keydetailsKey = IdentityKvAdCacheKeys.GetKey(identityKvAdId);
            await _distributedCache.RemoveAsync(keydetailsKey);
        }
        public async Task RemoveListAsync()
        {
            string identityKvAdListKey = IdentityKvAdCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(identityKvAdListKey);
        }

        public async Task RemoveGetByUserIdAsync(Guid userId)
        {
            string detailsKey = IdentityKvAdCacheKeys.GetByUserIdKey(userId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveGetActiveByUserIdAsync(Guid userId)
        {
            string detailsKey = IdentityKvAdCacheKeys.GetActiveByUserIdKey(userId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListActiveTodayAsync()
        {
            string identityKvAdListKey = IdentityKvAdCacheKeys.ListActiveKey;
            await _distributedCache.RemoveAsync(identityKvAdListKey);
        }
    }
}
