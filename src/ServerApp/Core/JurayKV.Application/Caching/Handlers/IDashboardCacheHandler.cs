namespace JurayKV.Application.Caching.Handlers
{
    public interface IDashboardCacheHandler
    {
        Task RemoveRunningAdsCount();
        Task RemoveActiveAdsCount();
        Task RemoveTotalUsersPostTodayCount();
        Task RemoveTotalUsersCount();
        Task RemoveTotalActiveUsersCount();
        Task RemoveTotalPointsEarnTodayCount();
        Task RemoveTotalPointsEarnTheseWeekCount();
        Task RemoveTotalPointsEarnTheseMonthCount();
    }

}
