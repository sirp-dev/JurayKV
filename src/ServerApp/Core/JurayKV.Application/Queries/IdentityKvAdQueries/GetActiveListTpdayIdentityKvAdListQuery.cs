using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.IdentityKvAdQueries
{
    
    public sealed class GetActiveListTpdayIdentityKvAdListQuery : IRequest<List<IdentityKvAdListDto>>
    {
        private class GetActiveListTpdayIdentityKvAdListQueryHandler : IRequestHandler<GetActiveListTpdayIdentityKvAdListQuery, List<IdentityKvAdListDto>>
        {
            private readonly IIdentityKvAdCacheRepository _departmentCacheRepository;

            public GetActiveListTpdayIdentityKvAdListQueryHandler(IIdentityKvAdCacheRepository departmentCacheRepository)
            {
                _departmentCacheRepository = departmentCacheRepository;
            }

            public async Task<List<IdentityKvAdListDto>> Handle(GetActiveListTpdayIdentityKvAdListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<IdentityKvAdListDto> departmentDtos = await _departmentCacheRepository.GetListActiveTodayAsync();
                return departmentDtos;
            }
        }
    }
}
