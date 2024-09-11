using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class AdvertRequestRepository : GenericRepository<AdvertRequest>, IAdvertRequestRepository
    {
        private readonly JurayDbContext _dbContext;

        public AdvertRequestRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsAsync(Expression<Func<AdvertRequest, bool>> condition)
        {
            IQueryable<AdvertRequest> queryable = _dbContext.Set<AdvertRequest>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }
        public async Task<AdvertRequest> GetByTransactionReferenceAsync(string transactionReference)
        {
             
            AdvertRequest advertRequest = await _dbContext.AdvertRequests.FirstOrDefaultAsync(x=>x.TransactionReference == transactionReference);
            return advertRequest;
        }

        public async Task<AdvertRequest> GetByIdAsync(Guid advertRequestId)
        {
            advertRequestId.ThrowIfEmpty(nameof(advertRequestId));

            AdvertRequest advertRequest = await _dbContext.Set<AdvertRequest>().FindAsync(advertRequestId);
            return advertRequest;
        }

        public async Task InsertAsync(AdvertRequest advertRequest)
        {
            advertRequest.ThrowIfNull(nameof(advertRequest));

            await _dbContext.AddAsync(advertRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(AdvertRequest advertRequest)
        {
            advertRequest.ThrowIfNull(nameof(advertRequest));

            EntityEntry<AdvertRequest> trackedEntity = _dbContext.ChangeTracker.Entries<AdvertRequest>()
                .FirstOrDefault(x => x.Entity == advertRequest);

            if (trackedEntity == null)
            {
                _dbContext.Update(advertRequest);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(AdvertRequest advertRequest)
        {
            advertRequest.ThrowIfNull(nameof(advertRequest));

            _dbContext.Remove(advertRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<AdvertRequest>> LastListByCountByCompanyId(int toplistcount, Guid companyId)
        {
            var list = await _dbContext.AdvertRequests
                .Where(x => x.CompanyId == companyId).OrderByDescending(x => x.CreatedAtUtc)
                .Take(toplistcount).ToListAsync();
            return list;
        }

        public async Task<int> AdvertRequestCount(Guid companyId)
        {
            return await _dbContext.AdvertRequests.Where(x => x.CompanyId == companyId).CountAsync();
        }

        public async Task<List<AdvertRequest>> ListByCompanyAsync(Guid companyId)
        {
            var list = await _dbContext.AdvertRequests
                .Include(x => x.Company)
                .Where(x => x.CompanyId == companyId).OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync();
            return list;
        }

        public async Task<List<AdvertRequest>> ListByAsync()
        {
            var list = await _dbContext.AdvertRequests.Include(x => x.Company)

                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync();
            return list;
        }
    }

}
