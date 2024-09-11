using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.KvPointQueries;

public sealed class GetKvPointByIdQuery : IRequest<KvPointDetailsDto>
{
    public GetKvPointByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetKvPointByIdQueryHandler : IRequestHandler<GetKvPointByIdQuery, KvPointDetailsDto>
    {
        private readonly IKvPointCacheRepository _kvPointCacheRepository;

        public GetKvPointByIdQueryHandler(IQueryRepository repository, IKvPointCacheRepository kvPointCacheRepository)
        {
            _kvPointCacheRepository = kvPointCacheRepository;
        }

        public async Task<KvPointDetailsDto> Handle(GetKvPointByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

           
            KvPointDetailsDto kvPointDetailsDto = await _kvPointCacheRepository.GetByIdAsync(request.Id);

            return kvPointDetailsDto;
        }
    }
}

 