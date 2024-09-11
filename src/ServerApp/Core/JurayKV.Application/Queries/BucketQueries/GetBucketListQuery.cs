using JurayKV.Application.Caching.Repositories;
using MediatR;
using System.Linq;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.BucketQueries;

public sealed class GetBucketListQuery : IRequest<List<BucketListDto>>
{
    public class GetBucketListQueryHandler : IRequestHandler<GetBucketListQuery, List<BucketListDto>>
    {
        private readonly IBucketCacheRepository _bucketCacheRepository;

        public GetBucketListQueryHandler(IBucketCacheRepository bucketCacheRepository)
        {
            _bucketCacheRepository = bucketCacheRepository;
        }

        public async Task<List<BucketListDto>> Handle(GetBucketListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<BucketListDto> bucketDtos = await _bucketCacheRepository.GetListAsync();
            return bucketDtos.OrderBy(x=>x.Name).Where(x=>x.Disable == false).ToList();
        }
    }
}
 