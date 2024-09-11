using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.IdentityActivityAggregate
{
    public interface IIdentityActivityRepository
    {
        Task<IdentityActivity> GetByIdAsync(Guid identityActivityId);
        Task<IdentityActivity> GetByUserIdAsync(Guid userId);

        Task InsertAsync(IdentityActivity identityActivity);

        Task UpdateAsync(IdentityActivity identityActivity);

        Task DeleteAsync(IdentityActivity identityActivity);
    }
}
