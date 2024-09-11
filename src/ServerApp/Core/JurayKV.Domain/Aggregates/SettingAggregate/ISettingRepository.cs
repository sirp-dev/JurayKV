using JurayKV.Domain.Aggregates.GenericRepositoryInterface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.SettingAggregate
{
    public interface ISettingRepository : IGenericRepository<Setting>
    {
        Task<Setting> GetByIdAsync(Guid settingId);
        Task<Setting> GetSettingAsync();

        Task InsertAsync(Setting setting);

        Task UpdateAsync(Setting setting);

        Task DeleteAsync(Setting setting); 
    }
}
