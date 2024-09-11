using JurayKV.Domain.Aggregates.DepartmentAggregate;
using JurayKV.Domain.Aggregates.GenericRepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.BucketAggregate
{
    public interface IBucketRepository : IGenericRepository<Bucket>
    {
        Task<Bucket> GetByIdAsync(Guid bucketId);

        Task InsertAsync(Bucket bucket);

        Task UpdateAsync(Bucket bucket);

        Task DeleteAsync(Bucket bucket);

        Task<List<Bucket>> GetBucketAndAdsAsync();
    }
}
