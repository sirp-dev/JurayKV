using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.AdvertRequestAggregate
{
    public interface IAdvertRequestRepository
    {
        Task<AdvertRequest> GetByIdAsync(Guid advertRequestId);

        Task InsertAsync(AdvertRequest advertRequest);

        Task UpdateAsync(AdvertRequest advertRequest);

        Task DeleteAsync(AdvertRequest advertRequest);
        Task<AdvertRequest> GetByTransactionReferenceAsync(string transactionReference);
        Task<List<AdvertRequest>> LastListByCountByCompanyId(int toplistcount, Guid companyId);
        Task<List<AdvertRequest>> ListByCompanyAsync(Guid companyId);
        Task<List<AdvertRequest>> ListByAsync();
        Task<int> AdvertRequestCount(Guid companyId);

    }
}
