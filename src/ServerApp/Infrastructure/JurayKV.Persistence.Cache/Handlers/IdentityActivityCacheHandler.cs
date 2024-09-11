using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class IdentityActivityCacheHandler : IIdentityActivityCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public IdentityActivityCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid identityActivityId)
        {
            string detailsKey = IdentityActivityCacheKeys.GetDetailsKey(identityActivityId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveGetAsync(Guid identityActivityId)
        {
            string detailsKey = IdentityActivityCacheKeys.GetKey(identityActivityId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string identityActivityListKey = IdentityActivityCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(identityActivityListKey);
        }
    }
}
