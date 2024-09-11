using JurayKV.Application.Queries.CompanyQueries;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{

    [ScopedService]
    public interface IDashboardCacheRepository
    {

        //Task<int> RunningAdsCount();
        Task<int> ActiveAdsCount();
        Task<int> TotalUsersPostTodayCount();
        Task<int> TotalUsersCount();
        Task<int> TotalActiveUsersCount();
        Task<decimal> TotalPointsEarnTodayCount();
        Task<decimal> TotalPointsEarnTheseWeekCount();
        Task<decimal> TotalPointsEarnTheseMonthCount();

    }
}
