using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
    [ScopedService]
    public interface IIdentityKvAdCacheRepository
    {
        Task<List<IdentityKvAdListDto>> GetListAsync(); 
        Task<List<IdentityKvAdListDto>> GetListActiveTodayAsync();

        Task<IdentityKvAdDetailsDto> GetByIdAsync(Guid modelId);
        Task<List<IdentityKvAdListDto>> GetByUserIdAsync(Guid userId); 
        Task<List<IdentityKvAdListDto>> GetActiveByUserIdAsync(Guid userId);

        Task<IdentityKvAdDetailsDto> GetDetailsByIdAsync(Guid modelId);
        Task<List<IdentityKvAdListDto>> GetActiveByCompanyIdAsync(Guid companyId);
        Task<int> AdsCount(Guid userId);
        Task<bool> CheckIdnetityKvIdFirstTime(Guid userId);
        Task<bool> CheckVideoIdnetityKvIdFirstTime(Guid userId);
    }
}
