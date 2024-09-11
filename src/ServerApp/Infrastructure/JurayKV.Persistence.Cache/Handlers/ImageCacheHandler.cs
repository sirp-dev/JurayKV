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
    public class ImageCacheHandler : IImageCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public ImageCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

       

        public async Task RemoveGetByIdAsync(Guid imageId)
        {
            string detailsKey = ImageCacheKeys.GetKey(imageId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        
        public async Task RemoveListAsync()
        {
            string imageListKey = ImageCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(imageListKey);
        }
    }

}
