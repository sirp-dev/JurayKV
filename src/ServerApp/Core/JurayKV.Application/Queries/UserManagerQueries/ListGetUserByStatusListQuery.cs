using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.UserManagerQueries
{
        public sealed class ListGetUserByStatusListQuery : IRequest<IEnumerable<UserManagerListDto>>
    {

        public ListGetUserByStatusListQuery(AccountStatus? accountStatus)
        {
            AccountStatus = accountStatus;
        }

        public AccountStatus? AccountStatus { get; }


        private class ListGetUserByStatusListQueryHandler : IRequestHandler<ListGetUserByStatusListQuery, IEnumerable<UserManagerListDto>>
        {
            private readonly IUserManagerCacheRepository _userManager;

            public ListGetUserByStatusListQueryHandler(IUserManagerCacheRepository userManager)
            {
                _userManager = userManager;
            }

            public async Task<IEnumerable<UserManagerListDto>> Handle(ListGetUserByStatusListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                var data = await _userManager.ListAsync(request.AccountStatus);

                return data;
            }
        }
    }

}