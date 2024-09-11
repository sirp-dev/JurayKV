using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.AdvertRequestQueries;

public sealed class GetAdvertRequestListQuery : IRequest<List<AdvertRequestListDto>>
{
    private class GetAdvertRequestListQueryHandler : IRequestHandler<GetAdvertRequestListQuery, List<AdvertRequestListDto>>
    {
        private readonly IAdvertRequestCacheRepository _advertRequestCacheRepository;

        public GetAdvertRequestListQueryHandler(IAdvertRequestCacheRepository advertRequestCacheRepository)
        {
            _advertRequestCacheRepository = advertRequestCacheRepository;
        }

        public async Task<List<AdvertRequestListDto>> Handle(GetAdvertRequestListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<AdvertRequestListDto> advertRequestDtos = await _advertRequestCacheRepository.GetListAsync();
            return advertRequestDtos;
        }
    }
}

