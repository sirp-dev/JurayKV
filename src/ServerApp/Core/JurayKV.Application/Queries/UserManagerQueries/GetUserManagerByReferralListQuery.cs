using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.UserManagerQueries
{
    public sealed class GetUserManagerByReferralListQuery : IRequest<List<UserManagerListDto>>
    {
        public GetUserManagerByReferralListQuery(string phone)
        {
            Phone = phone;
        }

        public string Phone { get; }
        private class GetUserManagerByReferralListQueryHandler : IRequestHandler<GetUserManagerByReferralListQuery, List<UserManagerListDto>>
        {
            private readonly IUserManagerCacheRepository _userManager;

            public GetUserManagerByReferralListQueryHandler(IUserManagerCacheRepository userManager)
            {
                _userManager = userManager;
            }

            public async Task<List<UserManagerListDto>> Handle(GetUserManagerByReferralListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                List<UserManagerListDto> data = await _userManager.GetListReferralAsync(request.Phone);

                return data;
            }
        }
    }

}
