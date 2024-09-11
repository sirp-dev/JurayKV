using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Application.Queries.KvPointQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
   [ScopedService]
    public interface IKvPointCacheRepository
    {
        Task<List<KvPointListDto>> GetListAsync();
        Task<List<KvPointListDto>> GetListByWeekAsync(DateTime? date);
        Task<List<KvPointListDto>> GetListByCountAsync(int toplistcount, Guid userId);
        Task<List<KvPointListDto>> GetListByUserIdAsync(Guid userId);

        Task<KvPointDetailsDto> GetByIdAsync(Guid modelId);

        Task<KvPointDetailsDto> GetDetailsByIdAsync(Guid modelId);

        Task<bool> CheckFirstPoint(Guid userId);

    }
}
