using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class NotificationCacheHandler : INotificationCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public NotificationCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid modelId)
        {
            string detailsKey = NotificationCacheKeys.GetDetailsKey(modelId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveGetAsync(Guid modelId)
        {
            string detailsKey = NotificationCacheKeys.GetKey(modelId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string departmentListKey = NotificationCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(departmentListKey);
        }
    }
}
