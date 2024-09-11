using JurayKV.Domain.Aggregates.EmployeeAggregate;
using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    public sealed class ExchangeRateRepository : GenericRepository<ExchangeRate>, IExchangeRateRepository
    {
        private readonly JurayDbContext _dbContext;

        public ExchangeRateRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ExchangeRate>> GetAllExchangeRates()
        {
            var list = await _dbContext.ExchangeRates.ToListAsync();
            return list;
        }

        public Task<bool> ExistsAsync(Expression<Func<ExchangeRate, bool>> condition)
        {
            IQueryable<ExchangeRate> queryable = _dbContext.Set<ExchangeRate>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            } 
            return queryable.AnyAsync();
        }

        public async Task<ExchangeRate> GetByIdAsync(Guid exchangeRateId)
        {
            exchangeRateId.ThrowIfEmpty(nameof(exchangeRateId));

            ExchangeRate exchangeRate = await _dbContext.Set<ExchangeRate>().FindAsync(exchangeRateId);
            return exchangeRate;
        }

        public async Task InsertAsync(ExchangeRate exchangeRate)
        {
            exchangeRate.ThrowIfNull(nameof(exchangeRate));

            await _dbContext.AddAsync(exchangeRate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExchangeRate exchangeRate)
        {
            exchangeRate.ThrowIfNull(nameof(exchangeRate));

            EntityEntry<ExchangeRate> trackedEntity = _dbContext.ChangeTracker.Entries<ExchangeRate>()
                .FirstOrDefault(x => x.Entity == exchangeRate);

            if (trackedEntity == null)
            {
                _dbContext.Update(exchangeRate);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ExchangeRate exchangeRate)
        {
            exchangeRate.ThrowIfNull(nameof(exchangeRate));

            _dbContext.Remove(exchangeRate);
            await _dbContext.SaveChangesAsync();
        }

       

        public async Task<ExchangeRate> GetByLatestSingleAsync()
        {
            ExchangeRate latestItem = _dbContext.ExchangeRates
    .OrderByDescending(item => item.CreatedAtUtc)
    .FirstOrDefault();
            return latestItem;
        }
    }

}
