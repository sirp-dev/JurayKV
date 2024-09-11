using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Domain.Aggregates.SettingAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
    [ScopedService]
    public interface ISettingCacheRepository
    {
        Task<List<SettingDetailsDto>> GetListAsync();
        Task<SettingDetailsDto> GetSettingAsync();

        Task<SettingDetailsDto> GetByIdAsync(Guid modelId);
    }
}
