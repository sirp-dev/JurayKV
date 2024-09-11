using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Caching.Handlers
{
    public interface IAdvertRequestCacheHandler
    {
        Task RemoveGetAsync(Guid modelId);
        Task RemoveDetailsByIdAsync(Guid modelId);
        Task RemoveList10ByCompanyAsync(Guid companyId);

        Task RemoveListAsync();
    }
}
