using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.IdentityActivityQueries;

public sealed class GetIdentityActivityListQuery : IRequest<List<IdentityActivityListDto>>
{
    private class GetIdentityActivityListQueryHandler : IRequestHandler<GetIdentityActivityListQuery, List<IdentityActivityListDto>>
    {
        private readonly IIdentityActivityCacheRepository _identityActivityCacheRepository;

        public GetIdentityActivityListQueryHandler(IIdentityActivityCacheRepository identityActivityCacheRepository)
        {
            _identityActivityCacheRepository = identityActivityCacheRepository;
        }

        public async Task<List<IdentityActivityListDto>> Handle(GetIdentityActivityListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<IdentityActivityListDto> identityActivityDtos = await _identityActivityCacheRepository.GetListAsync();
            return identityActivityDtos;
        }
    }
}

 