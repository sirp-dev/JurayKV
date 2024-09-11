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
   
public sealed class GetActiveAdsQuery : IRequest<KvAdDetailsDto>
    {
        public GetActiveAdsQuery(Guid id)
        {
            Id = id.ThrowIfEmpty(nameof(id));
        }

        public Guid Id { get; }

        private class GetActiveAdsQueryHandler : IRequestHandler<GetActiveAdsQuery, KvAdDetailsDto>
        {
            private readonly IKvAdCacheRepository _kvAdCacheRepository;

            public GetActiveAdsQueryHandler(IKvAdCacheRepository kvAdCacheRepository)
            {
                _kvAdCacheRepository = kvAdCacheRepository;
            }

            public async Task<KvAdDetailsDto> Handle(GetActiveAdsQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                 KvAdDetailsDto kvAdDtos = await _kvAdCacheRepository.GetByActiveAsync(request.Id);
                return kvAdDtos;
            }
        }
    }
}
