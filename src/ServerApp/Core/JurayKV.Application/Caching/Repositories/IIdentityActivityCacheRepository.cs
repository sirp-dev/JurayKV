using JurayKV.Application.Queries.IdentityActivityQueries;
using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
    [ScopedService]
    public interface IIdentityActivityCacheRepository
    {
        Task<List<IdentityActivityListDto>> GetListAsync();

        Task<IdentityActivityListDto> GetByIdAsync(Guid modelId);

        Task<IdentityActivityListDto> GetDetailsByIdAsync(Guid modelId);
    }
}
