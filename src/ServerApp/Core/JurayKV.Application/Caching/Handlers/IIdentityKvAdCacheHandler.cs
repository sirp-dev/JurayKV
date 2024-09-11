using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Caching.Handlers
{
    public interface IIdentityKvAdCacheHandler
    {
        Task RemoveDetailsByIdAsync(Guid modelId);
        Task RemoveGetAsync(Guid identityKvAdId);
        Task RemoveGetByUserIdAsync(Guid userId);
        Task RemoveListAsync();
        Task RemoveListActiveTodayAsync();
        Task RemoveGetActiveByUserIdAsync(Guid userId);
    }
}
