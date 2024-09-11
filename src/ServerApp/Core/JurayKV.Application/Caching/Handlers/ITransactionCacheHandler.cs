using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Caching.Handlers
{
    public interface ITransactionCacheHandler
    {
        Task RemoveGetAsync(Guid modelId);
        Task RemoveDetailsByIdAsync(Guid modelId); 
        Task RemoveList10ByUserAsync(Guid userId);

        Task RemoveListAsync();
    }
}
