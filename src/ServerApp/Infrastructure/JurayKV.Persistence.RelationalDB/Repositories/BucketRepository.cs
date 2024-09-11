using JurayKV.Domain.Aggregates.BucketAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class BucketRepository : GenericRepository<Bucket>, IBucketRepository
    {
        private readonly JurayDbContext _dbContext;

        public BucketRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsAsync(Expression<Func<Bucket, bool>> condition)
        {
            IQueryable<Bucket> queryable = _dbContext.Set<Bucket>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }

        public async Task<Bucket> GetByIdAsync(Guid bucketId)
        {
             
            if (bucketId == Guid.Empty)
            {
                throw new ArgumentException("bucketId cannot be empty.", nameof(bucketId));
            }
            //Bucket bucket = await _dbContext.Set<Bucket>().FindAsync(bucketId);
            Bucket bucket = await _dbContext.Buckets.FirstOrDefaultAsync(x=>x.Id == bucketId);
            return bucket;
        }

        public async Task InsertAsync(Bucket bucket)
        {
            if (bucket == null)
            {
                throw new ArgumentException("bucketId cannot be empty.", nameof(bucket));
            }

            await _dbContext.AddAsync(bucket);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bucket bucket)
        {
            if (bucket == null)
            {
                throw new ArgumentException("bucketId cannot be empty.", nameof(bucket));
            }

            EntityEntry<Bucket> trackedEntity = _dbContext.ChangeTracker.Entries<Bucket>()
                .FirstOrDefault(x => x.Entity == bucket);

            if (trackedEntity == null)
            {
                _dbContext.Update(bucket);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Bucket bucket)
        {
            if (bucket == null)
            {
                throw new ArgumentException("bucketId cannot be empty.", nameof(bucket));
            }

            _dbContext.Remove(bucket);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Bucket>> GetBucketAndAdsAsync()
        {
            return null;
        }
    }

}
