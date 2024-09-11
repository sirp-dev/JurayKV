using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.KvPointQueries;

public sealed class GetKvPointListQuery : IRequest<List<KvPointListDto>>
{
    private class GetKvPointListQueryHandler : IRequestHandler<GetKvPointListQuery, List<KvPointListDto>>
    {
        private readonly IKvPointCacheRepository _kvPointCacheRepository;

        public GetKvPointListQueryHandler(IKvPointCacheRepository kvPointCacheRepository)
        {
            _kvPointCacheRepository = kvPointCacheRepository;
        }

        public async Task<List<KvPointListDto>> Handle(GetKvPointListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<KvPointListDto> kvPointDtos = await _kvPointCacheRepository.GetListAsync();
            return kvPointDtos;
        }
    }
}

 