using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.IdentityActivityQueries;
using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.Caching;

namespace JurayKV.Persistence.Cache.Repositories
{
        public sealed class IdentityActivityCacheRepository : IIdentityActivityCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;

        public IdentityActivityCacheRepository(IDistributedCache distributedCache, IQueryRepository repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public async Task<List<IdentityActivityListDto>> GetListAsync()
        {
            string cacheKey = IdentityActivityCacheKeys.ListKey;
            List<IdentityActivityListDto> list = await _distributedCache.GetAsync<List<IdentityActivityListDto>>(cacheKey);

            if (list == null)
            {
                Expression<Func<IdentityActivity, IdentityActivityListDto>> selectExp = d => new IdentityActivityListDto
                {
                    Id = d.Id,
                    Activity = d.Activity,
                    UserId = d.UserId,
                    CreatedAtUtc = d.CreatedAtUtc
                };

                list = await _repository.GetListAsync(selectExp);

                await _distributedCache.SetAsync(cacheKey, list);
            }

            return list;
        }

        public async Task<IdentityActivityListDto> GetByIdAsync(Guid bucketId)
        {
            string cacheKey = IdentityActivityCacheKeys.GetKey(bucketId);
            IdentityActivityListDto bucket = await _distributedCache.GetAsync<IdentityActivityListDto>(cacheKey);

            if (bucket == null)
            {
                bucket = await _repository.GetByIdAsync<IdentityActivityListDto>(bucketId);

                await _distributedCache.SetAsync(cacheKey, bucket);
            }

            return bucket;
        }

        public async Task<IdentityActivityListDto> GetDetailsByIdAsync(Guid bucketId)
        {
            string cacheKey = IdentityActivityCacheKeys.GetDetailsKey(bucketId);
            IdentityActivityListDto bucket = await _distributedCache.GetAsync<IdentityActivityListDto>(cacheKey);

            if (bucket == null)
            {
                Expression<Func<IdentityActivity, IdentityActivityListDto>> selectExp = d => new IdentityActivityListDto
                {
                    Id = d.Id,
                    Activity = d.Activity,
                    UserId = d.UserId,
                    CreatedAtUtc = d.CreatedAtUtc
                };

                bucket = await _repository.GetByIdAsync(bucketId, selectExp);

                await _distributedCache.SetAsync(cacheKey, bucket);
            }

            return bucket;
        }
    }

}
