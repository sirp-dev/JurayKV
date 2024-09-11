using JurayKV.Application.Queries.ExchangeRateQueries;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
    [ScopedService]
    public interface IExchangeRateCacheRepository
    {
        Task<List<ExchangeRateListDto>> GetListAsync();

        Task<ExchangeRateDetailsDto> GetByIdAsync(Guid modelId);
        Task<ExchangeRateDetailsDto> GetByLatestAsync();

        Task<ExchangeRateDetailsDto> GetDetailsByIdAsync(Guid modelId);
    }
}
