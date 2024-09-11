using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.ExchangeRateQueries;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using JurayKV.Domain.Aggregates.GenericRepositoryInterface;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.Caching;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class ExchangeRateCacheRepository : IExchangeRateCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IExchangeRateRepository _exchangeRepository;
        private readonly IQueryRepository _repository;
        public ExchangeRateCacheRepository(IDistributedCache distributedCache, IExchangeRateRepository exchangeRepository, IQueryRepository repository)
        {
            _distributedCache = distributedCache;
            _exchangeRepository = exchangeRepository;
            _repository = repository;
        }

        public async Task<List<ExchangeRateListDto>> GetListAsync()
        {
            string cacheKey = ExchangeRateCacheKeys.ListKey;
            List<ExchangeRateListDto> list = await _distributedCache.GetAsync<List<ExchangeRateListDto>>(cacheKey);

            if (list == null)
            {

                var xlist = await _exchangeRepository.GetAllExchangeRates();
                list = xlist.Select(d => new ExchangeRateListDto
                {
                    Id = d.Id,
                    Amount = d.Amount,
                    CreatedAtUtc = d.CreatedAtUtc
                }).ToList();
                await _distributedCache.SetAsync(cacheKey, list.OrderBy(x => x.CreatedAtUtc));
            }

            return list;
        }

        public async Task<ExchangeRateDetailsDto> GetByIdAsync(Guid exchangeRateId)
        {
            string cacheKey = ExchangeRateCacheKeys.GetKey(exchangeRateId);
            ExchangeRateDetailsDto exchangeRate = await _distributedCache.GetAsync<ExchangeRateDetailsDto>(cacheKey);


            if (exchangeRate == null)
            {
                Expression<Func<ExchangeRate, ExchangeRateDetailsDto>> selectExp = d => new ExchangeRateDetailsDto
                {
                    Id = d.Id,
                    Amount = d.Amount,
                    CreatedAtUtc = d.CreatedAtUtc
                };

                exchangeRate = await _repository.GetByIdAsync(exchangeRateId, selectExp);

                await _distributedCache.SetAsync(cacheKey, exchangeRate);
            }

            return exchangeRate;
        }

        public async Task<ExchangeRateDetailsDto> GetDetailsByIdAsync(Guid exchangeRateId)
        {
            string cacheKey = ExchangeRateCacheKeys.GetDetailsKey(exchangeRateId);
            ExchangeRateDetailsDto exchangeRate = await _distributedCache.GetAsync<ExchangeRateDetailsDto>(cacheKey);

            if (exchangeRate == null)
            {
                Expression<Func<ExchangeRate, ExchangeRateDetailsDto>> selectExp = d => new ExchangeRateDetailsDto
                {
                    Id = d.Id,
                    Amount = d.Amount,
                    CreatedAtUtc = d.CreatedAtUtc
                };

                exchangeRate = await _repository.GetByIdAsync(exchangeRateId, selectExp);

                await _distributedCache.SetAsync(cacheKey, exchangeRate);
            }

            return exchangeRate;
        }

        public async Task<ExchangeRateDetailsDto> GetByLatestAsync()
        {
            string cacheKey = ExchangeRateCacheKeys.GetLaskKey();
            ExchangeRateDetailsDto exchangeRate = await _distributedCache.GetAsync<ExchangeRateDetailsDto>(cacheKey);

            if (exchangeRate == null)
            {
                var mainexchangeRate = await _exchangeRepository.GetByLatestSingleAsync();
                if (mainexchangeRate != null) { 
                exchangeRate = new ExchangeRateDetailsDto
                {
                    Id = mainexchangeRate.Id,
                    Amount = mainexchangeRate.Amount,
                    CreatedAtUtc = mainexchangeRate.CreatedAtUtc
                };
                await _distributedCache.SetAsync(cacheKey, exchangeRate);
                }
            }

            return exchangeRate;
        }
    }

}
