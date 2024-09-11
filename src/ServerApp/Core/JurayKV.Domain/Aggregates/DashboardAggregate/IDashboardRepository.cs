using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.DashboardAggregate
{
    public interface IDashboardRepository
    {
        Task<int> RunningAdsCount();
        Task<int> ActiveAdsCount();
        Task<int> TotalUsersPostTodayCount();
        Task<int> TotalUsersCount();
        Task<int> TotalActiveUsersCount();
        Task<int> TotalPointsEarnTodayCount();
        Task<int> TotalPointsEarnTheseWeekCount();
        Task<int> TotalPointsEarnTheseMonthCount();
         
         
    }
}
