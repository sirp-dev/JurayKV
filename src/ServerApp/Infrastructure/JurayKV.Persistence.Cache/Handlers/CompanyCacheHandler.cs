using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class CompanyCacheHandler : ICompanyCacheHandler
    {

        private readonly IDistributedCache _distributedCache;

        public CompanyCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task RemoveDetailsByIdAsync(Guid companyId)
        {
            string detailsKey = CompanyCacheKeys.GetDetailsKey(companyId);
            await _distributedCache.RemoveAsync(detailsKey);
        }
        public async Task RemoveDropdownListAsync()
        {
            string companyListKey = CompanyCacheKeys.SelectListKey;
            await _distributedCache.RemoveAsync(companyListKey);
        }

        public async Task RemoveGetAsync(Guid companyId)
        {
            string detailsKey = CompanyCacheKeys.GetKey(companyId);
            await _distributedCache.RemoveAsync(detailsKey);
        }

        public async Task RemoveListAsync()
        {
            string companyListKey = CompanyCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(companyListKey);
        }
    }
}
