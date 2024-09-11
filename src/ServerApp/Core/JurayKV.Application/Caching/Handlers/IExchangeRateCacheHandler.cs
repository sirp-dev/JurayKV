using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Caching.Handlers
{
    public interface IExchangeRateCacheHandler
    {
        Task RemoveDetailsByIdAsync(Guid exchangeRateId);
        Task RemoveGetAsync(Guid exchangeRateId);

        Task RemoveListAsync();
        Task RemoveLastListAsync();
    }
}
