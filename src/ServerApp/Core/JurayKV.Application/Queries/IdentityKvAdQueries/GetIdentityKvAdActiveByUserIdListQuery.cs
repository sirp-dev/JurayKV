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
     public sealed class GetIdentityKvAdActiveByUserIdListQuery : IRequest<List<IdentityKvAdListDto>>
    {
        public GetIdentityKvAdActiveByUserIdListQuery(Guid userId)
        {
            UserId = userId.ThrowIfEmpty(nameof(userId));
        }

        public Guid UserId { get; }

        public class GetIdentityKvAdActiveByUserIdListQueryHandler : IRequestHandler<GetIdentityKvAdActiveByUserIdListQuery, List<IdentityKvAdListDto>>
        {
            private readonly IIdentityKvAdCacheRepository _departmentCacheRepository;

            public GetIdentityKvAdActiveByUserIdListQueryHandler(IIdentityKvAdCacheRepository departmentCacheRepository)
            {
                _departmentCacheRepository = departmentCacheRepository;
            }

            public async Task<List<IdentityKvAdListDto>> Handle(GetIdentityKvAdActiveByUserIdListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<IdentityKvAdListDto> departmentDtos = await _departmentCacheRepository.GetActiveByUserIdAsync(request.UserId);
                return departmentDtos;
            }
        }
    }

}
