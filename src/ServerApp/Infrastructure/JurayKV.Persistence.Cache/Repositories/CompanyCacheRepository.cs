using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
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
    public sealed class CompanyCacheRepository : ICompanyCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;
        private readonly ICompanyRepository _companyRepository;

        public CompanyCacheRepository(IDistributedCache distributedCache, IQueryRepository repository, ICompanyRepository companyRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _companyRepository = companyRepository;
        }

        public async Task<List<CompanyListDto>> GetListAsync()
        {
            string cacheKey = CompanyCacheKeys.ListKey;
            List<CompanyListDto> list = await _distributedCache.GetAsync<List<CompanyListDto>>(cacheKey);

            if (list == null)
            {
                Expression<Func<Company, CompanyListDto>> selectExp = d => new CompanyListDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    CreatedAtUtc = d.CreatedAtUtc,
                    User = d.User,
                    UserId = d.UserId,
                    AmountPerPoint = d.AmountPerPoint,
                };

                list = await _repository.GetListAsync(selectExp);

                await _distributedCache.SetAsync(cacheKey, list);
            }

            return list;
        }

        public async Task<CompanyDetailsDto> GetByIdAsync(Guid companyId)
        {
            string cacheKey = CompanyCacheKeys.GetKey(companyId);
            CompanyDetailsDto company = await _distributedCache.GetAsync<CompanyDetailsDto>(cacheKey);

            if (company == null)
            {
                Expression<Func<Company, CompanyDetailsDto>> selectExp = d => new CompanyDetailsDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    CreatedAtUtc = d.CreatedAtUtc,
                    User = d.User,
                    UserId = d.UserId,
                    AmountPerPoint = d.AmountPerPoint,
                };

                company = await _repository.GetByIdAsync(companyId, selectExp);

                await _distributedCache.SetAsync(cacheKey, company);
            }
            return company;
        }
        public async Task<CompanyDetailsDto> GetByUserIdAsync(Guid userId)
        {

            var cmp = await _companyRepository.GetByUserIdAsync(userId);
            var company = new CompanyDetailsDto
            {
                Id = cmp.Id,
                Name = cmp.Name,
                CreatedAtUtc = cmp.CreatedAtUtc,
                User = cmp.User,
                UserId = cmp.UserId,
                AmountPerPoint = cmp.AmountPerPoint,
                Fullname = cmp.User.FirstName +" "+cmp.User.LastName,
                Email = cmp.User.Email,
                Phone = cmp.User.PhoneNumber,
            };
            
            return company;
        }
        public async Task<CompanyDetailsDto> GetDetailsByIdAsync(Guid companyId)
        {
            string cacheKey = CompanyCacheKeys.GetDetailsKey(companyId);
            CompanyDetailsDto company = await _distributedCache.GetAsync<CompanyDetailsDto>(cacheKey);

            if (company == null)
            {
                Expression<Func<Company, CompanyDetailsDto>> selectExp = d => new CompanyDetailsDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    CreatedAtUtc = d.CreatedAtUtc,
                    User = d.User,
                    UserId = d.UserId,
                    AmountPerPoint = d.AmountPerPoint,
                };

                company = await _repository.GetByIdAsync(companyId, selectExp);

                await _distributedCache.SetAsync(cacheKey, company);
            }

            return company;
        }

        public async Task<List<CompanyDropdownListDto>> GetDropdownListAsync()
        {
            string cacheKey = CompanyCacheKeys.SelectListKey;
            List<CompanyDropdownListDto> list = await _distributedCache.GetAsync<List<CompanyDropdownListDto>>(cacheKey);

            if (list == null)
            {
                Expression<Func<Company, CompanyDropdownListDto>> selectExp = d => new CompanyDropdownListDto
                {
                    Id = d.Id,
                    Name = d.Name,

                };

                list = await _repository.GetListAsync(selectExp);

                await _distributedCache.SetAsync(cacheKey, list);
            }

            return list;
        }
    }

}
