using JurayKV.Domain.Aggregates.DashboardAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class DashboardRepository : IDashboardRepository
    {
        private readonly JurayDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _usermanager;

        public DashboardRepository(JurayDbContext dbContext, UserManager<ApplicationUser> usermanager)
        {
            _dbContext = dbContext;
            _usermanager = usermanager;
        }

        public async Task<int> ActiveAdsCount()
        {
            throw new NotImplementedException();
        }

        public Task<int> RunningAdsCount()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalActiveUsersCount()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalPointsEarnTheseMonthCount()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalPointsEarnTheseWeekCount()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalPointsEarnTodayCount()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalUsersCount()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalUsersPostTodayCount()
        {
            throw new NotImplementedException();
        }
    }
}
