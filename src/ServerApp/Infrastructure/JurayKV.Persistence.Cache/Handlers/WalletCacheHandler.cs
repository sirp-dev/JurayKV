using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class WalletCacheHandler : IWalletCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public WalletCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid walletId)
        {
            string detailsKey = WalletCacheKeys.GetDetailsKey(walletId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveDetailsByUserIdAsync(Guid userId)
        {
            string detailsKey = WalletCacheKeys.GetUserKey(userId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveGetAsync(Guid modelId)
        {
            string detailsKey = WalletCacheKeys.GetDetailsKey(modelId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string walletListKey = WalletCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(walletListKey);
        }
    }
}
