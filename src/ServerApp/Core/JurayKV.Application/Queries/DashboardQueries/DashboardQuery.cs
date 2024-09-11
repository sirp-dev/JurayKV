using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.DepartmentQueries;
using JurayKV.Domain.Aggregates.DepartmentAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.DashboardQueries
{



    public sealed class DashboardQuery : IRequest<DashboardDto>
    {
        public class DashboardQueryHandler : IRequestHandler<DashboardQuery, DashboardDto>
        {
            private readonly IDashboardCacheRepository _dashboardCacheRepository;

            public DashboardQueryHandler(IDashboardCacheRepository dashboardCacheRepository)
            {
                _dashboardCacheRepository = dashboardCacheRepository;
            }

            public async Task<DashboardDto> Handle(DashboardQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                DashboardDto data = new DashboardDto();

                data.TotalUserPostTodayCount = await _dashboardCacheRepository.TotalUsersPostTodayCount();
                data.TotalUserPostToday = "Total User Post Today";

                data.TotalUserCount = await _dashboardCacheRepository.TotalUsersCount();
                data.TotalUser = "Total Users";

                 
                data.ActiveAdsCount = await _dashboardCacheRepository.ActiveAdsCount();
                data.ActiveAds = "Running Ads Today";

                data.ActiveUserCount = await _dashboardCacheRepository.TotalActiveUsersCount();
                data.ActiveUser = "Active User";

                data.TotalPointEarnTodayCount = await _dashboardCacheRepository.TotalPointsEarnTodayCount();
                data.TotalPointEarnToday = "Total Point Earn Today";

                data.TotalPointEarnTheseWeekCount = await _dashboardCacheRepository.TotalPointsEarnTheseWeekCount();
                data.TotalPointEarnTheseWeek = "Total Point Earn These Week";

                data.TotalPointEarnTheseMonthCount = await _dashboardCacheRepository.TotalPointsEarnTheseMonthCount();
                data.TotalPointEarnTheseMonth = "Total Point Earn These Month";
                return data;
            }
        }
    }
}
