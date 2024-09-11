using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.AdvertRequestQueries;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class AdvertRequestCacheRepository : IAdvertRequestCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;
        private readonly IAdvertRequestRepository _advertRequestRepository;
        public AdvertRequestCacheRepository(IDistributedCache distributedCache, IQueryRepository repository, IAdvertRequestRepository advertRequestRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _advertRequestRepository = advertRequestRepository;
        }
        public async Task<List<AdvertRequestListDto>> GetListByCompanyAsync(Guid companyId)
        {
            var mainlist = await _advertRequestRepository.ListByCompanyAsync(companyId);
            var list = mainlist.Select(d => new AdvertRequestListDto
            {
                Id = d.Id,
                Amount = d.Amount,
                CompanyId = d.CompanyId,
                Company = d.Company,
                Note = d.Note,
                Status = d.Status,
                ImageUrl = d.ImageUrl,
                TransactionReference = d.TransactionReference,
                CreatedAtUtc = d.CreatedAtUtc
            }).ToList();


            return list.OrderByDescending(x => x.CreatedAtUtc).ToList();
        }
        public async Task<List<AdvertRequestListDto>> GetListAsync()
        {
            var mainlist = await _advertRequestRepository.ListByAsync();
            var list = mainlist.Select(d => new AdvertRequestListDto
            {
                Id = d.Id,
                Amount = d.Amount,
                CompanyId = d.CompanyId,
                Company = d.Company,
                Note = d.Note,
                Status = d.Status,
                ImageUrl = d.ImageUrl,
                TransactionReference = d.TransactionReference,
                CreatedAtUtc = d.CreatedAtUtc
            }).ToList();


            return list.OrderByDescending(x=>x.CreatedAtUtc).ToList();
        }

        public async Task<AdvertRequestDetailsDto> GetByIdAsync(Guid advertRequestId)
        {
             
            var d = await _advertRequestRepository.GetByIdAsync(advertRequestId);
            var advertRequest = new AdvertRequestDetailsDto
            {
                Id = d.Id,
                Amount = d.Amount,
                Company = d.Company,
                CompanyId = d.CompanyId,
                Note = d.Note,
                Status = d.Status,
                ImageUrl = d.ImageUrl,
                TransactionReference = d.TransactionReference,
                CreatedAtUtc = d.CreatedAtUtc
            };

            return advertRequest;
        }

        public async Task<AdvertRequestDetailsDto> GetDetailsByIdAsync(Guid advertRequestId)
        {
            //string cacheKey = AdvertRequestCacheKeys.GetDetailsKey(advertRequestId);
            //AdvertRequestDetailsDto advertRequest = await _distributedCache.GetAsync<AdvertRequestDetailsDto>(cacheKey);

            //if (advertRequest == null)
            //{
            Expression<Func<AdvertRequest, AdvertRequestDetailsDto>> selectExp = d => new AdvertRequestDetailsDto
            {
                Id = d.Id,
                Amount = d.Amount,
                CompanyId = d.CompanyId,
                Note = d.Note,
                Status = d.Status,
                ImageUrl = d.ImageUrl,
                TransactionReference = d.TransactionReference,
                CreatedAtUtc = d.CreatedAtUtc
            };

            var advertRequest = await _repository.GetByIdAsync(advertRequestId, selectExp);

            //    await _distributedCache.SetAsync(cacheKey, advertRequest);
            //}

            return advertRequest;
        }


        public async Task<List<AdvertRequestListDto>> GetListByCountAsync(int toplistcount, Guid userId)
        {
            //string cacheKey = AdvertRequestCacheKeys.ListByCountUserIdKey(userId);
            //List<AdvertRequestListDto> list = await _distributedCache.GetAsync<List<AdvertRequestListDto>>(cacheKey);

            //if (list == null)
            //{

            var xlist = await _advertRequestRepository.LastListByCountByCompanyId(toplistcount, userId);
            var list = xlist.Select(d => new AdvertRequestListDto
            {
                Id = d.Id,
                Amount = d.Amount,
                CompanyId = d.CompanyId,
                Note = d.Note,
                Status = d.Status,
                ImageUrl = d.ImageUrl,
                TransactionReference = d.TransactionReference,
                CreatedAtUtc = d.CreatedAtUtc
            }).ToList();

            //    await _distributedCache.SetAsync(cacheKey, list);
            //}

            return list.OrderByDescending(x => x.CreatedAtUtc).ToList();
        }

        public Task<List<AdvertRequestListDto>> GetListByCountAsync(int latestcount)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AdvertRequestCount(Guid userId)
        {
            return await _advertRequestRepository.AdvertRequestCount(userId);
        }
    }

}
