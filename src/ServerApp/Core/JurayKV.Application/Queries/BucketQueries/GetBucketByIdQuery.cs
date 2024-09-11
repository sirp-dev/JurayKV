using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.BucketAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.BucketQueries;

public sealed class GetBucketByIdQuery : IRequest<BucketDetailsDto>
{
    public GetBucketByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    public class GetBucketByIdQueryHandler : IRequestHandler<GetBucketByIdQuery, BucketDetailsDto>
    {
        private readonly IQueryRepository _repository;
        private readonly IBucketCacheRepository _bucketCacheRepository;

        public GetBucketByIdQueryHandler(IQueryRepository repository, IBucketCacheRepository bucketCacheRepository)
        {
            _repository = repository;
            _bucketCacheRepository = bucketCacheRepository;
        }

        public async Task<BucketDetailsDto> Handle(GetBucketByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
 
             
            BucketDetailsDto bucketDetailsDto = await _bucketCacheRepository.GetByIdAsync(request.Id);

            return bucketDetailsDto;
        }
    }
}
