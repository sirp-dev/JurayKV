using JurayKV.Domain.Aggregates.SettingAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class SettingRepository : GenericRepository<Setting>, ISettingRepository
    {
        private readonly JurayDbContext _dbContext;

        public SettingRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsAsync(Expression<Func<Setting, bool>> condition)
        {
            IQueryable<Setting> queryable = _dbContext.Set<Setting>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }

        public async Task<Setting> GetByIdAsync(Guid settingId)
        {

            if (settingId == Guid.Empty)
            {
                throw new ArgumentException("settingId cannot be empty.", nameof(settingId));
            }
            //Setting setting = await _dbContext.Set<Setting>().FindAsync(settingId);
            Setting setting = await _dbContext.Settings.FirstOrDefaultAsync(x => x.Id == settingId);
            return setting;
        }

        public async Task InsertAsync(Setting setting)
        {
            if (setting == null)
            {
                throw new ArgumentException("settingId cannot be empty.", nameof(setting));
            }

            await _dbContext.AddAsync(setting);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Setting setting)
        {
            if (setting == null)
            {
                throw new ArgumentException("settingId cannot be empty.", nameof(setting));
            }

            EntityEntry<Setting> trackedEntity = _dbContext.ChangeTracker.Entries<Setting>()
                .FirstOrDefault(x => x.Entity == setting);

            if (trackedEntity == null)
            {
                _dbContext.Update(setting);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Setting setting)
        {
            if (setting == null)
            {
                throw new ArgumentException("settingId cannot be empty.", nameof(setting));
            }

            _dbContext.Remove(setting);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Setting> GetSettingAsync()
        {
            
            Setting setting = await _dbContext.Settings.FirstOrDefaultAsync();
            return setting;
        }
    }

}
