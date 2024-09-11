using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Caching.Handlers
{
    public interface IKvPointCacheHandler
    {
        Task RemoveDetailsByIdAsync(Guid modelId);
        Task RemoveListBy10ByUserAsync(Guid userId);
        Task RemoveListByUserAsync(Guid userId);
        Task RemoveGetAsync(Guid modelId);

        Task RemoveListAsync(); 
    }
}
