using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.KvAdQueries
{
    public sealed class ClearActiveAdsQuery : IRequest
    {
       
        // Handler
        private class ClearActiveAdsQueryHandler : IRequestHandler<ClearActiveAdsQuery>
        {
            private readonly IKvAdCacheRepository _kvAdCacheRepository;

            public ClearActiveAdsQueryHandler(IQueryRepository repository, IKvAdCacheRepository kvAdCacheRepository)
            {
                _kvAdCacheRepository = kvAdCacheRepository;
            }

            public async Task Handle(ClearActiveAdsQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                  await _kvAdCacheRepository.ClearAllActive();
                 
            }
        }
    }

}
