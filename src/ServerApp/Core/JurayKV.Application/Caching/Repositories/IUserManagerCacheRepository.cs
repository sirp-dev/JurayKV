using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.UserAccountQueries.DashboardQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Caching.Repositories
{
    [ScopedService]
    public interface IUserManagerCacheRepository
    {
        Task<IEnumerable<UserManagerListDto>> ListAsync(AccountStatus? status);
        Task<List<UserManagerListDto>> GetListAsync();
        Task<UserManagerDetailsDto> GetReferralInfoByPhoneAsync(string phone);
        Task<List<UserManagerListDto>> GetListByStatusAsync(AccountStatus status);
        Task<UserManagerDetailsDto> GetByIdAsync(Guid modelId);
        Task<UserDashboardDto> GetUserDashboardDto(Guid userId, CancellationToken cancellationToken);
        Task<List<UserManagerListDto>> GetListReferralAsync(string myphone);
        Task<int> GetListReferralCountAsync(string myphone);
        Task<UserListPagedDto> GetListByStatusAndDateAsync(AccountStatus status, DateTime? startdate, DateTime? enddate, int pageSize, int pageNumber, string? searchstring, int sortOrder);
        Task<UserListPagedDto> ListGetListByStatusAndDateAsync(AccountStatus status, DateTime? startdate, DateTime? enddate, int pageSize, int pageNumber, string? searchstring, int sortOrder);


        }
}
