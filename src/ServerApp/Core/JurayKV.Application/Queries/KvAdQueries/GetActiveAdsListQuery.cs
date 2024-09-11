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
    
    public sealed class GetActiveAdsListQuery : IRequest<List<KvAdDetailsDto>>
    {
      

        private class GetActiveAdsListQueryHandler : IRequestHandler<GetActiveAdsListQuery, List<KvAdDetailsDto>>
        {
            private readonly IKvAdCacheRepository _kvAdCacheRepository;

            public GetActiveAdsListQueryHandler(IKvAdCacheRepository kvAdCacheRepository)
            {
                _kvAdCacheRepository = kvAdCacheRepository;
            }

            public async Task<List<KvAdDetailsDto>> Handle(GetActiveAdsListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                var kvAdDtos = await _kvAdCacheRepository.GetByActiveAsync();
                return kvAdDtos;
            }
        }
    }

}
