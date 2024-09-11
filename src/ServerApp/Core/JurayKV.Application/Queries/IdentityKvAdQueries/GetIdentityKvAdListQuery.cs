using JurayKV.Application.Caching.Repositories;
using MediatR;
using System.Linq;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.IdentityKvAdQueries;

public sealed class GetIdentityKvAdListQuery : IRequest<List<IdentityKvAdListDto>>
{
    private class GetIdentityKvAdListQueryHandler : IRequestHandler<GetIdentityKvAdListQuery, List<IdentityKvAdListDto>>
    {
        private readonly IIdentityKvAdCacheRepository _departmentCacheRepository;

        public GetIdentityKvAdListQueryHandler(IIdentityKvAdCacheRepository departmentCacheRepository)
        {
            _departmentCacheRepository = departmentCacheRepository;
        }

        public async Task<List<IdentityKvAdListDto>> Handle(GetIdentityKvAdListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<IdentityKvAdListDto> departmentDtos = await _departmentCacheRepository.GetListAsync();
            return departmentDtos.OrderByDescending(x=>x.CreatedAtUtc).ToList();
        }
    }
}

