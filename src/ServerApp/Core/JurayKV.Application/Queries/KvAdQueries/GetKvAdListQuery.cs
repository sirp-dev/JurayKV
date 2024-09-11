using JurayKV.Application.Caching.Repositories;
using MediatR;
using System.Linq;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.KvAdQueries;

public sealed class GetKvAdListQuery : IRequest<List<KvAdListDto>>
{
    private class GetKvAdListQueryHandler : IRequestHandler<GetKvAdListQuery, List<KvAdListDto>>
    {
        private readonly IKvAdCacheRepository _kvAdCacheRepository;

        public GetKvAdListQueryHandler(IKvAdCacheRepository kvAdCacheRepository)
        {
            _kvAdCacheRepository = kvAdCacheRepository;
        }

        public async Task<List<KvAdListDto>> Handle(GetKvAdListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<KvAdListDto> kvAdDtos = await _kvAdCacheRepository.GetListAsync();
            return kvAdDtos.OrderByDescending(x=>x.CreatedAtUtc).ToList();
        }
    }
}
 