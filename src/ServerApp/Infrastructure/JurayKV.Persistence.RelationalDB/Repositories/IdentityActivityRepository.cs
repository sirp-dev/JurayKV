using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class IdentityActivityRepository : GenericRepository<IdentityActivity>, IIdentityActivityRepository
    {
        private readonly JurayDbContext _dbContext;

        public IdentityActivityRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsAsync(Expression<Func<IdentityActivity, bool>> condition)
        {
            IQueryable<IdentityActivity> queryable = _dbContext.Set<IdentityActivity>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }

        public async Task<IdentityActivity> GetByIdAsync(Guid identityActivityId)
        {
            identityActivityId.ThrowIfEmpty(nameof(identityActivityId));

            IdentityActivity identityActivity = await _dbContext.Set<IdentityActivity>().FindAsync(identityActivityId);
            return identityActivity;
        }

        public async Task InsertAsync(IdentityActivity identityActivity)
        {
            identityActivity.ThrowIfNull(nameof(identityActivity));

            await _dbContext.AddAsync(identityActivity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(IdentityActivity identityActivity)
        {
            identityActivity.ThrowIfNull(nameof(identityActivity));

            EntityEntry<IdentityActivity> trackedEntity = _dbContext.ChangeTracker.Entries<IdentityActivity>()
                .FirstOrDefault(x => x.Entity == identityActivity);

            if (trackedEntity == null)
            {
                _dbContext.Update(identityActivity);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IdentityActivity identityActivity)
        {
            identityActivity.ThrowIfNull(nameof(identityActivity));

            _dbContext.Remove(identityActivity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IdentityActivity> GetByUserIdAsync(Guid userId)
        {
            userId.ThrowIfEmpty(nameof(userId));

            IdentityActivity identityActivity = await _dbContext.Set<IdentityActivity>().FirstOrDefaultAsync(x=>x.UserId == userId);
            return identityActivity;
        }
    }

}
