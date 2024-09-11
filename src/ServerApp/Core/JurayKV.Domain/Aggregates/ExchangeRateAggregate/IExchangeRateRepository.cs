using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.ExchangeRateAggregate
{
    public interface IExchangeRateRepository
    {
        Task<List<ExchangeRate>> GetAllExchangeRates();
        Task<ExchangeRate> GetByIdAsync(Guid exchangeRateId);
        Task<ExchangeRate> GetByLatestSingleAsync();

        Task InsertAsync(ExchangeRate exchangeRate);

        Task UpdateAsync(ExchangeRate exchangeRate);

        Task DeleteAsync(ExchangeRate exchangeRate);
    }
}
