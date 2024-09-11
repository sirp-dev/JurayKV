using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class DashboardCacheKeys
    {
         
        public static string RunningAdsCountKey => "RunningAdsCount";
        public static string ActiveAdsCountKey => "ActiveAdsCount";
        public static string TotalUsersPostTodayCountKey => "TotalUsersPostTodayCount";
        public static string TotalUsersCountKey => "TotalUsersCount";
        public static string TotalActiveUsersCountKey => "TotalActiveUsersCount";
        public static string TotalPointsEarnTodayCountKey => "TotalPointsEarnTodayCount";
        public static string TotalPointsEarnTheseWeekCountKey => "TotalPointsEarnTheseWeekCount";
        public static string TotalPointsEarnTheseMonthCountKey => "TotalPointsEarnTheseMonthCount";

    }
}
