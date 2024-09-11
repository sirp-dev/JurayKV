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
    public class UserManagerCacheHandler : IUserManagerCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public UserManagerCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid transactionId)
        {
            string detailsKey = UserManagerCacheKeys.GetDetailsKey(transactionId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string transactionListKey = UserManagerCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(transactionListKey);
        }
    }
}
