using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.UserManagerQueries;

public sealed class GetUserManagerListQuery : IRequest<List<UserManagerListDto>>
{
    private class GetUserManagerListQueryHandler : IRequestHandler<GetUserManagerListQuery, List<UserManagerListDto>>
    {
        private readonly IUserManagerCacheRepository _userManager;

        public GetUserManagerListQueryHandler(IUserManagerCacheRepository userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserManagerListDto>> Handle(GetUserManagerListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            List<UserManagerListDto> data = await _userManager.GetListAsync();

            return data;
        }
    }
}

