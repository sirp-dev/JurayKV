using JurayKV.Domain.Aggregates.KvAdAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.KvPointAggregate
{
    public interface IKvPointRepository
    {
        Task<KvPoint> GetByIdAsync(Guid kvPointId);
        Task<KvPoint> GetByIdentityIdByUserAsync(Guid identityKvAd, Guid userId);

        Task<Guid> InsertAsync(KvPoint kvPoint);

        Task UpdateAsync(KvPoint kvPoint);

        Task DeleteAsync(KvPoint kvPoint);

        Task<List<KvPoint>> LastTenByUserId (int toplistcount, Guid userId);
        Task<List<KvPoint>> LastByUserId (Guid userId);


        Task<bool> CheckFirstPoint (Guid userId);
    }
}
