using JurayKV.Domain.Aggregates.KvAdAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.ValueObjects;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class KvAdRepository : GenericRepository<KvAd>, IKvAdRepository
    {
        private readonly JurayDbContext _dbContext;

        public KvAdRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsAsync(Expression<Func<KvAd, bool>> condition)
        {
            IQueryable<KvAd> queryable = _dbContext.Set<KvAd>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }

        public async Task<KvAd> GetByIdAsync(Guid kvAdId)
        {
            kvAdId.ThrowIfEmpty(nameof(kvAdId));

            KvAd kvAd = await _dbContext.Set<KvAd>().FindAsync(kvAdId);
            return kvAd;
        }

        public async Task InsertAsync(KvAd kvAd)
        {
            kvAd.ThrowIfNull(nameof(kvAd));
           
            await _dbContext.AddAsync(kvAd);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(KvAd kvAd)
        {
            kvAd.ThrowIfNull(nameof(kvAd));

            EntityEntry<KvAd> trackedEntity = _dbContext.ChangeTracker.Entries<KvAd>()
                .FirstOrDefault(x => x.Entity == kvAd);

            if (trackedEntity == null)
            {
                _dbContext.Update(kvAd);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(KvAd kvAd)
        {
            kvAd.ThrowIfNull(nameof(kvAd));

            _dbContext.Remove(kvAd);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<KvAd>> AdsByCompanyIdList(Guid companyId)
        {
            var data = await _dbContext.kvAds
                .Include(x => x.Bucket)
                .Include(x => x.Company)
                .Include(x => x.ImageFile)
                .Where(x => x.CompanyId == companyId && x.Status == Domain.Primitives.Enum.DataStatus.Active)
                .Where(x => x.ImageFile != null)
                .ToListAsync();

            return data;
        }
        public async Task<List<KvAd>> AdsByBucketId(Guid bucketId)
        {
            var data = await _dbContext.kvAds
                .Include(x=>x.Bucket)
                .Include(x=>x.Company)
                .Include(x=>x.ImageFile)
                .Where(x=>x.BucketId == bucketId && x.Status == Domain.Primitives.Enum.DataStatus.Active)
                .Where(x=>x.ImageFile != null)
                .ToListAsync();

            return data;
        }
        public async Task<List<KvAd>> AdsByCompanyId(Guid companyId)
        {
            DateTime mdate = DateForSix.GetTheDateBySix(DateTime.UtcNow.AddHours(1));

            //
            var data = await _dbContext.kvAds
                .Include(x => x.Bucket)
                .Include(x => x.Company)
                .Include(x => x.IdentityKvAds)
                .Include(x => x.ImageFile)
                .Where(x => x.CompanyId == companyId)
                .Where(x => x.ImageFile != null)
                .Where(x=>x.CreatedAtUtc.Date < mdate.Date)
                .ToListAsync();

            return data;
        }
        public async Task<List<KvAd>> AdsForAllBucketByCompanyId(DateTime date, Guid companyId)
        {

            var data = await _dbContext.kvAds
                .Include(x => x.Bucket)
                .Include(x => x.Company)
                .Include(x=>x.IdentityKvAds)
                .Include(x => x.ImageFile)
                .Where(x=>x.CompanyId == companyId)
                .Where(item => item.CreatedAtUtc.Date == date.Date)
                //.Where(x => x.Status == Domain.Primitives.Enum.DataStatus.Active && date.ToString("ddMMyyyy") == x.DateId)
                .Where(x => x.ImageFile != null)
                .ToListAsync();

            return data;
        }
        public async Task<List<KvAd>> AdsForAllBucket(DateTime date)
        {
            DateTime mdate = DateForSix.GetTheDateBySix(DateTime.UtcNow.AddHours(1));
            var data = await _dbContext.kvAds
                .Include(x => x.Bucket)
                .Include(x => x.Company)
                .Include(x => x.ImageFile)
                .Where(x => x.Status == Domain.Primitives.Enum.DataStatus.Active && date.ToString("ddMMyyyy") == x.DateId)
                .Where(x => x.ImageFile != null)
                .ToListAsync();

            return data;
        }
        public async Task<KvAd> GetByActiveAsync(Guid bucketId)
        {
            return await _dbContext.kvAds.Include(x=>x.Bucket).Include(x => x.ImageFile).Include(x=>x.Company).FirstOrDefaultAsync(x=>x.Active == true && x.BucketId == bucketId && x.ImageFile != null);
        }

        public async Task<KvAd> MakeActiveAsync(Guid id, Guid bucketId, bool active)
        {
            var getallactive = _dbContext.kvAds.Where(x => x.BucketId == bucketId && x.Active == true).AsQueryable();
            foreach(var kvAd in getallactive)
            {
                kvAd.Active = false;
                _dbContext.Attach(kvAd).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync();
            var item = await _dbContext.kvAds.FirstOrDefaultAsync(x=>x.Id == id && x.BucketId == bucketId);
            item.Active = active;
            _dbContext.Attach(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task AdsClearAllActive()
        {
            var item = _dbContext.kvAds.AsNoTracking().Where(x => x.Active == true).AsQueryable();
            foreach(var x in item)
            {
                x.Active = false;
                _dbContext.Attach(x).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<List<KvAd>> GetByActiveAsync()
        //{
        //    return await _dbContext.kvAds.Include(x => x.Bucket).Include(x => x.ImageFile).Include(x => x.Company).Where(x => x.Active == true ).ToListAsync();
        //}

        public async Task<List<KvAd>> GetByActiveAsync()
        {
            DateTime createdAtUtc = DateTime.UtcNow.AddHours(1);
            DateTime sixAMDateTime = new DateTime(createdAtUtc.Year, createdAtUtc.Month, createdAtUtc.Day, 6, 0, 0);
            DateTime sixAMDateTimec = sixAMDateTime.AddDays(1);

            return await _dbContext.kvAds
                .Include(x => x.Bucket)
                .Include(x => x.ImageFile)
                .Include(x => x.Company)
                .Where(x => x.Active == true && sixAMDateTime <= DateTime.UtcNow.AddHours(1) && DateTime.UtcNow.AddHours(1) <= sixAMDateTimec)
                .ToListAsync();
        }
    }

}
