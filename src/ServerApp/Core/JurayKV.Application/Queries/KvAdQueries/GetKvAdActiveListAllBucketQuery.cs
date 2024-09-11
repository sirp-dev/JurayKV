using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.KvAdQueries
{
    public sealed class GetKvAdActiveListAllBucketQuery : IRequest<List<KvAdListDto>>
    {
        public GetKvAdActiveListAllBucketQuery(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; }
        private class GetKvAdActiveListAllBucketQueryHandler : IRequestHandler<GetKvAdActiveListAllBucketQuery, List<KvAdListDto>>
        {
            private readonly IKvAdCacheRepository _kvAdCacheRepository;

            public GetKvAdActiveListAllBucketQueryHandler(IKvAdCacheRepository kvAdCacheRepository)
            {
                _kvAdCacheRepository = kvAdCacheRepository;
            }

            public async Task<List<KvAdListDto>> Handle(GetKvAdActiveListAllBucketQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<KvAdListDto> kvAdDtos = await _kvAdCacheRepository.GetActiveListAllBucketAsync(request.Date);
                return kvAdDtos;
            }
        }
    }

}
