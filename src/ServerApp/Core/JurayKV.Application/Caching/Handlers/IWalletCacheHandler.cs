using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Caching.Handlers
{
    public interface IWalletCacheHandler
    {
        Task RemoveGetAsync(Guid modelId);
        Task RemoveDetailsByIdAsync(Guid modelId);
        Task RemoveDetailsByUserIdAsync(Guid userId);

        Task RemoveListAsync();
    }
}
