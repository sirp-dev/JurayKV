using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.BucketQueries
{
    public sealed class GetAllBucketListQuery : IRequest<List<BucketListDto>>
    {
        public class GetAllBucketListQueryHandler : IRequestHandler<GetAllBucketListQuery, List<BucketListDto>>
        {
            private readonly IBucketCacheRepository _bucketCacheRepository;

            public GetAllBucketListQueryHandler(IBucketCacheRepository bucketCacheRepository)
            {
                _bucketCacheRepository = bucketCacheRepository;
            }

            public async Task<List<BucketListDto>> Handle(GetAllBucketListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<BucketListDto> bucketDtos = await _bucketCacheRepository.GetListAsync();
                return bucketDtos.OrderBy(x => x.Name).ToList();
            }
        }
    }

}
