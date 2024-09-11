using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.BucketQueries;

public sealed class GetBucketDropdownListQuery : IRequest<List<BucketDropdownListDto>>
{
    public class GetBucketDropdownListQueryHandler : IRequestHandler<GetBucketDropdownListQuery, List<BucketDropdownListDto>>
    {
        private readonly IBucketCacheRepository _bucketCacheRepository;

        public GetBucketDropdownListQueryHandler(IBucketCacheRepository bucketCacheRepository)
        {
            _bucketCacheRepository = bucketCacheRepository;
        }

        public async Task<List<BucketDropdownListDto>> Handle(GetBucketDropdownListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<BucketDropdownListDto> bucketDtos = await _bucketCacheRepository.GetDropdownListAsync();
            return bucketDtos;
        }
    }
}
