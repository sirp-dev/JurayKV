using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.KvAdQueries;

public sealed class GetKvAdByIdQuery : IRequest<KvAdDetailsDto>
{
    public GetKvAdByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetKvAdByIdQueryHandler : IRequestHandler<GetKvAdByIdQuery, KvAdDetailsDto>
    {
        private readonly IKvAdCacheRepository _kvAdCacheRepository;

        public GetKvAdByIdQueryHandler(IQueryRepository repository, IKvAdCacheRepository kvAdCacheRepository)
        {
            _kvAdCacheRepository = kvAdCacheRepository;
        }

        public async Task<KvAdDetailsDto> Handle(GetKvAdByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

           

            KvAdDetailsDto kvAdDetailsDto = await _kvAdCacheRepository.GetByIdAsync(request.Id);

            return kvAdDetailsDto;
        }
    }
}

 