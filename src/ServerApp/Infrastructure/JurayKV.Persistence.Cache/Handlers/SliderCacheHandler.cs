using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class SliderCacheHandler : ISliderCacheHandler
    {
        private readonly IDistributedCache _distributedCache;

        public SliderCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task RemoveGetByIdAsync(Guid sliderId)
        {
            string detailsKey = SliderCacheKeys.GetKey(sliderId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string sliderListKey = SliderCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(sliderListKey);
        }
    }
}
