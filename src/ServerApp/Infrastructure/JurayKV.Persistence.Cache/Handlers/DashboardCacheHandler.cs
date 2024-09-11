using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class DashboardCacheHandler : IDashboardCacheHandler
    {
        private readonly IDistributedCache _distributedCache;

        public DashboardCacheHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task RemoveListAsync()
        {
            string companyListKey = CompanyCacheKeys.ListKey;
            await _distributedCache.RemoveAsync(companyListKey);
        }

        public async Task RemoveActiveAdsCount()
        {
            string dataKey = DashboardCacheKeys.ActiveAdsCountKey;
            await _distributedCache.RemoveAsync(dataKey);
        }

        public async Task RemoveRunningAdsCount()
        {
            string dataKey = DashboardCacheKeys.ActiveAdsCountKey; 
            await _distributedCache.RemoveAsync(dataKey);
        }

        public async Task RemoveTotalActiveUsersCount()
        {
            string dataKey = DashboardCacheKeys.ActiveAdsCountKey; 
            await _distributedCache.RemoveAsync(dataKey);
        }

        public async Task RemoveTotalPointsEarnTheseMonthCount()
        {
            string dataKey = DashboardCacheKeys.ActiveAdsCountKey; 
            await _distributedCache.RemoveAsync(dataKey);
        }

        public async Task RemoveTotalPointsEarnTheseWeekCount()
        {
            string dataKey = DashboardCacheKeys.ActiveAdsCountKey;
            await _distributedCache.RemoveAsync(dataKey);
        }

        public async Task RemoveTotalPointsEarnTodayCount()
        {
            string dataKey = DashboardCacheKeys.ActiveAdsCountKey; 
            await _distributedCache.RemoveAsync(dataKey);
        }

        public async Task RemoveTotalUsersCount()
        {
            string dataKey = DashboardCacheKeys.ActiveAdsCountKey;
            await _distributedCache.RemoveAsync(dataKey);
        }

        public async Task RemoveTotalUsersPostTodayCount()
        {
            string dataKey = DashboardCacheKeys.ActiveAdsCountKey;
            await _distributedCache.RemoveAsync(dataKey);
        }
    }
}
