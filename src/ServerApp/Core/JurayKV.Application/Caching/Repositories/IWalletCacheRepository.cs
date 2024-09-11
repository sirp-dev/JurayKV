using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.Aggregates.WalletAggregate;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
    [ScopedService]
    public interface IWalletCacheRepository
    {
        Task<List<WalletDetailsDto>> GetListAsync();

        Task<WalletDetailsDto> GetByIdAsync(Guid modelId);
        Task<WalletDetailsDto> GetByUserIdAsync(Guid userId);

        Task<WalletDetailsDto> GetDetailsByIdAsync(Guid modelId);
    }
}
