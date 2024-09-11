using System.Threading.Tasks;
using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;

namespace JurayKV.Persistence.Cache.Handlers;

internal sealed class DepartmentCacheHandler : IDepartmentCacheHandler
{
    private readonly IDistributedCache _distributedCache;

    public DepartmentCacheHandler(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task RemoveListAsync()
    {
        string departmentListKey = DepartmentCacheKeys.ListKey;
        await _distributedCache.RemoveAsync(departmentListKey);
    }
}
