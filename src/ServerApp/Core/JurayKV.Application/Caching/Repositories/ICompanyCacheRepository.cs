using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
    [ScopedService]
    public interface ICompanyCacheRepository
    {
        Task<List<CompanyListDto>> GetListAsync();
        Task<List<CompanyDropdownListDto>> GetDropdownListAsync();

        Task<CompanyDetailsDto> GetByIdAsync(Guid modelId);
        Task<CompanyDetailsDto> GetByUserIdAsync(Guid userId);

        Task<CompanyDetailsDto> GetDetailsByIdAsync(Guid modelId);
    }
}
