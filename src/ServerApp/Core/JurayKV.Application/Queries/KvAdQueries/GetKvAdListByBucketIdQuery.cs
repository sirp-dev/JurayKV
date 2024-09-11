using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.KvAdQueries;

public sealed class GetKvAdListByBucketIdQuery : IRequest<List<KvAdListDto>>
{
    public GetKvAdListByBucketIdQuery(Guid bucketId)
    {
        BucketId = bucketId.ThrowIfEmpty(nameof(bucketId));
    }

    public Guid BucketId { get; }
    private class GetKvAdListByBucketIdQueryHandler : IRequestHandler<GetKvAdListByBucketIdQuery, List<KvAdListDto>>
    {
        private readonly IKvAdCacheRepository _kvAdCacheRepository;

        public GetKvAdListByBucketIdQueryHandler(IKvAdCacheRepository kvAdCacheRepository)
        {
            _kvAdCacheRepository = kvAdCacheRepository;
        }

        public async Task<List<KvAdListDto>> Handle(GetKvAdListByBucketIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<KvAdListDto> kvAdDtos = await _kvAdCacheRepository.GetListByBucketIdAsync(request.BucketId);
            return kvAdDtos;
        }
    }
}
 