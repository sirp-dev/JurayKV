using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Handlers;

[ScopedService]
public interface IDepartmentCacheHandler
{
    Task RemoveListAsync();
}
