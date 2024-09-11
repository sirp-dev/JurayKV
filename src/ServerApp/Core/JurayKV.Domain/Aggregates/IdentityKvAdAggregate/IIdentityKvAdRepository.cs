using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.IdentityKvAdAggregate
{
    public interface IIdentityKvAdRepository
    {
        Task<IdentityKvAd> GetByIdAsync(Guid identityKvAdId);

        Task<Guid> InsertAsync(IdentityKvAd identityKvAd);

        Task UpdateAsync(IdentityKvAd identityKvAd);

        Task DeleteAsync(IdentityKvAd identityKvAd);
        Task DeleteUserAsync(Guid userId);

        Task<List<IdentityKvAd>> GetListByUserId(Guid userId);
        Task<IQueryable<IdentityKvAd>> GetActiveListByUserId(Guid userId);
        Task<IQueryable<IdentityKvAd>> ListActiveToday();
        Task<IQueryable<IdentityKvAd>> GetActiveListByCompanyId(Guid companyId);
        Task<bool> CheckExist(Guid userId, Guid kvAdId);
        Task<bool> CheckUserAdvertCountToday(Guid userId);

        Task<List<IdentityKvAd>> ListNonActive();
        Task<int> AdsCount(Guid userId);

        Task<bool> CheckIdnetityKvIdFirstTime(Guid userId);
        Task<bool> CheckVideoIdnetityKvIdFirstTime(Guid userId);
    }
}
