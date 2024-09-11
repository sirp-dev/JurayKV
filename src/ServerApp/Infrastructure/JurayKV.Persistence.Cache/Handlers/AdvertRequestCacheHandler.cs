using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class AdvertRequestCacheHandler : IAdvertRequestCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public AdvertRequestCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid advertRequestId)
        {
            string detailsKey = AdvertRequestCacheKeys.GetDetailsKey(advertRequestId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveGetAsync(Guid modelId)
        {
            string detailsKey = AdvertRequestCacheKeys.GetDetailsKey(modelId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveList10ByCompanyAsync(Guid companyId)
        {
            string detailsKey = AdvertRequestCacheKeys.GetDetailsKey(companyId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        

        public async Task RemoveListAsync()
        {
            string advertRequestListKey = AdvertRequestCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(advertRequestListKey);
        }
    }

}
