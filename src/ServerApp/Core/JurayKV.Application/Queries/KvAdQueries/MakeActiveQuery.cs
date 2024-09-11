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
    public sealed class MakeActiveQuery : IRequest
    {
        public MakeActiveQuery(Guid id, Guid bucketId, bool active)
        {
            Id = id.ThrowIfEmpty(nameof(id));
            BucketId = bucketId.ThrowIfEmpty(nameof(bucketId));
            Active = active;
        }

        public Guid Id { get; }
        public Guid BucketId { get; }
        public bool Active { get; }
        // Handler
        private class MakeActiveQueryHandler : IRequestHandler<MakeActiveQuery>
        {
            private readonly IKvAdCacheRepository _kvAdCacheRepository;

            public MakeActiveQueryHandler(IQueryRepository repository, IKvAdCacheRepository kvAdCacheRepository)
            {
                _kvAdCacheRepository = kvAdCacheRepository;
            }

            public async Task Handle(MakeActiveQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                 await _kvAdCacheRepository.MakeActiveAsync(request.Id, request.BucketId, request.Active);
                 
            }
        }
    }

}
