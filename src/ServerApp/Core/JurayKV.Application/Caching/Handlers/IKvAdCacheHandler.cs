using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Caching.Handlers
{
    public interface IKvAdCacheHandler
    {
        Task RemoveDetailsByIdAsync(Guid modelId);
        Task RemoveByBucketIdAsync(Guid bucketId);
        Task RemoveGetAsync(Guid modelId);

        Task RemoveListAsync(); 
    }
}
