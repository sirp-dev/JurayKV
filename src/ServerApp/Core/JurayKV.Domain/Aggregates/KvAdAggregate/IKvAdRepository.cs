using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.KvAdAggregate
{
    public interface IKvAdRepository
    {
        Task<KvAd> GetByIdAsync(Guid kvAdId);
        Task<KvAd> GetByActiveAsync(Guid bucketId);
        Task<List<KvAd>> GetByActiveAsync();
        Task<KvAd> MakeActiveAsync(Guid id, Guid bucketId, bool active);
        Task<List<KvAd>> AdsByCompanyIdList(Guid companyId);
        Task InsertAsync(KvAd kvAd);
        Task<List<KvAd>> AdsForAllBucketByCompanyId(DateTime date, Guid companyId);
        Task UpdateAsync(KvAd kvAd);
        Task<List<KvAd>> AdsForAllBucket(DateTime date);
        Task DeleteAsync(KvAd kvAd);
        Task<bool> ExistsAsync(Expression<Func<KvAd, bool>> condition);
        Task<List<KvAd>> AdsByBucketId(Guid bucketId);
        Task AdsClearAllActive();
        Task<List<KvAd>> AdsByCompanyId(Guid companyId);
    }
}
