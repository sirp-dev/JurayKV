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
    public class SettingCacheHandler : ISettingCacheHandler
    {
        private readonly IDistributedCache _distributedCache;

        public SettingCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task RemoveGetByIdAsync(Guid settingId)
        {
            string detailsKey = SettingCacheKeys.GetKey(settingId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string settingListKey = SettingCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(settingListKey);
        }


        public async Task RemoveSettingAsync()
        {
            string settingListKey = SettingCacheKeys.DefaultKey;
            await _distributedCache.RemoveAsync(settingListKey);
        }
    }

}
