using JurayKV.Application.Queries.AdvertRequestQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
    [ScopedService]
    public interface IAdvertRequestCacheRepository
    {
        Task<List<AdvertRequestListDto>> GetListAsync();
        Task<List<AdvertRequestListDto>> GetListByCountAsync(int latestcount);
        Task<List<AdvertRequestListDto>> GetListByCompanyAsync(Guid companyId);
        Task<AdvertRequestDetailsDto> GetByIdAsync(Guid modelId);

        Task<AdvertRequestDetailsDto> GetDetailsByIdAsync(Guid modelId);

        Task<List<AdvertRequestListDto>> GetListByCountAsync(int toplistcount, Guid companyId);

        Task<int> AdvertRequestCount(Guid companyId);

    }
}
