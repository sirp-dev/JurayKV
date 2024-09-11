namespace JurayKV.Application.Queries.DashboardQueries
{
    public class DashboardDto
    {
        public string RunningAds { get; set; }
        public int RunningAdsCount { get; set; }
        //

        public string ActiveAds { get; set; }
        public int ActiveAdsCount { get; set; }

        public string TotalUserPostToday { get; set; }
        public int TotalUserPostTodayCount { get; set; }

        public string TotalUser { get; set; }
        public int TotalUserCount { get; set; }

        public string ActiveUser { get; set; }
        public int ActiveUserCount { get; set; }


        public string TotalPointEarnToday { get; set; }
        public decimal TotalPointEarnTodayCount { get; set; }


        public string TotalPointEarnTheseWeek { get; set; }
        public decimal TotalPointEarnTheseWeekCount { get; set; }


        public string TotalPointEarnTheseMonth { get; set; }
        public decimal TotalPointEarnTheseMonthCount { get; set; }
    }
}
