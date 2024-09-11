using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Domain.Aggregates.BucketAggregate;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{

    [ScopedService]
    public interface IBucketCacheRepository
    {
        Task<List<BucketListDto>> GetListAsync();
        Task<List<BucketListDto>> GetListAndAdsAsync();
        Task<List<BucketDropdownListDto>> GetDropdownListAsync();

        Task<BucketDetailsDto> GetByIdAsync(Guid modelId);

        Task<BucketDetailsDto> GetDetailsByIdAsync(Guid modelId);
    }
}
