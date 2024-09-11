using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.Caching;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class KvPointCacheRepository : IKvPointCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;
        private readonly IKvPointRepository _kvPointRepository;
        public KvPointCacheRepository(IDistributedCache distributedCache, IQueryRepository repository, IKvPointRepository kvPointRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _kvPointRepository = kvPointRepository;
        }

        public async Task<List<KvPointListDto>> GetListAsync()
        {
            //string cacheKey = KvPointCacheKeys.ListKey;
            //List<KvPointListDto> list = await _distributedCache.GetAsync<List<KvPointListDto>>(cacheKey);

            //if (list == null)
            //{
                Expression<Func<KvPoint, KvPointListDto>> selectExp = d => new KvPointListDto
                {
                    Id = d.Id,
                    CreatedAtUtc = d.CreatedAtUtc,
                    IdentityKvAdId = d.IdentityKvAdId,
                    Status = d.Status,
                    Point = d.Point,
                    LastModifiedAtUtc = d.LastModifiedAtUtc,
                    PointHash = d.PointHash,
                    Fullname = d.User.UserName,

                    UserId = d.UserId,
                };

               var list = await _repository.GetListAsync(selectExp);

            //    await _distributedCache.SetAsync(cacheKey, list);
            //}

            return list;
        }

        public async Task<KvPointDetailsDto> GetByIdAsync(Guid kvPointId)
        {
            //string cacheKey = KvPointCacheKeys.GetKey(kvPointId);
            //KvPointDetailsDto kvPoint = await _distributedCache.GetAsync<KvPointDetailsDto>(cacheKey);
            //if (kvPoint == null)
            //{
                Expression<Func<KvPoint, KvPointDetailsDto>> selectExp = d => new KvPointDetailsDto
                {
                    Id = d.Id,
                    CreatedAtUtc = d.CreatedAtUtc,
                    IdentityKvAdId = d.IdentityKvAdId,
                    Status = d.Status,
                    Point = d.Point,
                    LastModifiedAtUtc = d.LastModifiedAtUtc,
                    PointHash = d.PointHash,
                    Fullname = d.User.UserName,

                    UserId = d.UserId,
                };

               var kvPoint = await _repository.GetByIdAsync(kvPointId, selectExp);

            //    await _distributedCache.SetAsync(cacheKey, kvPoint);
            //}
            return kvPoint;
        }

        public async Task<KvPointDetailsDto> GetDetailsByIdAsync(Guid kvPointId)
        {
            //string cacheKey = KvPointCacheKeys.GetDetailsKey(kvPointId);
            //KvPointDetailsDto kvPoint = await _distributedCache.GetAsync<KvPointDetailsDto>(cacheKey);

            //if (kvPoint == null)
            //{
                Expression<Func<KvPoint, KvPointDetailsDto>> selectExp = d => new KvPointDetailsDto
                {
                    Id = d.Id,
                    CreatedAtUtc = d.CreatedAtUtc,
                    IdentityKvAdId = d.IdentityKvAdId,
                    Status = d.Status,
                    Point = d.Point,
                    LastModifiedAtUtc = d.LastModifiedAtUtc,
                    PointHash = d.PointHash,
                    Fullname = d.User.UserName,

                    UserId = d.UserId,
                };

               var kvPoint = await _repository.GetByIdAsync(kvPointId, selectExp);

            //    await _distributedCache.SetAsync(cacheKey, kvPoint);
            //}

            return kvPoint;
        }

        public Task<List<KvPointListDto>> GetListByWeekAsync(DateTime? date)
        {
            throw new NotImplementedException();
        }

        public async Task<List<KvPointListDto>> GetListByCountAsync(int toplistcount, Guid userId)
        {
            //string cacheKey = KvPointCacheKeys.ListBy10UserIdKey(userId);
            //List<KvPointListDto> list = await _distributedCache.GetAsync<List<KvPointListDto>>(cacheKey);

            //if (list == null)
            //{

                var xlist = await _kvPointRepository.LastTenByUserId(toplistcount, userId);
               var list = xlist.Select(d => new KvPointListDto
                {
                    Id = d.Id,
                    CreatedAtUtc = d.CreatedAtUtc,
                    IdentityKvAdId = d.IdentityKvAdId,
                    Status = d.Status,
                    Point = d.Point,
                    LastModifiedAtUtc = d.LastModifiedAtUtc,
                    PointHash = d.PointHash,
                    Fullname = d.User.UserName,
                    UserId = d.UserId,
                }).ToList();

            //    await _distributedCache.SetAsync(cacheKey, list);
            //}

            return list;
        }

        public async Task<List<KvPointListDto>> GetListByUserIdAsync(Guid userId)
        {
            //string cacheKey = KvPointCacheKeys.ListUserIdKey(userId);
            //List<KvPointListDto> list = await _distributedCache.GetAsync<List<KvPointListDto>>(cacheKey);

            //if (list == null)
            //{

            //    var xlist = await _kvPointRepository.LastByUserId(userId);
            //    list = xlist.Select(d => new KvPointListDto
            //    {
            //        Id = d.Id,
            //        CreatedAtUtc = d.CreatedAtUtc,
            //        IdentityKvAdId = d.IdentityKvAdId,
            //        Status = d.Status,
            //        Point = d.Point,
            //        LastModifiedAtUtc = d.LastModifiedAtUtc,
            //        PointHash = d.PointHash,
            //        Fullname = d.User.UserName,
            //        UserId = d.UserId,
            //    }).ToList();

            //    await _distributedCache.SetAsync(cacheKey, list);
            //}
            var xlist = await _kvPointRepository.LastByUserId(userId);
            var list = xlist.Select(d => new KvPointListDto
            {
                Id = d.Id,
                CreatedAtUtc = d.CreatedAtUtc,
                IdentityKvAdId = d.IdentityKvAdId,
                Status = d.Status,
                Point = d.Point,
                LastModifiedAtUtc = d.LastModifiedAtUtc,
                PointHash = d.PointHash,
                Fullname = d.User.UserName,
                UserId = d.UserId,
            }).ToList();
            return list;
        }

        public async Task<bool> CheckFirstPoint(Guid userId)
        {
            return await _kvPointRepository.CheckFirstPoint(userId);
        }
    }

}
