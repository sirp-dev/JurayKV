using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.AdvertRequestQueries;

public sealed class GetAdvertRequestByIdQuery : IRequest<AdvertRequestDetailsDto>
{
    public GetAdvertRequestByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetAdvertRequestByIdQueryHandler : IRequestHandler<GetAdvertRequestByIdQuery, AdvertRequestDetailsDto>
    {
        private readonly IAdvertRequestCacheRepository _advertRequestCacheRepository;

        public GetAdvertRequestByIdQueryHandler(IQueryRepository repository, IAdvertRequestCacheRepository advertRequestCacheRepository)
        {
            _advertRequestCacheRepository = advertRequestCacheRepository;
        }

        public async Task<AdvertRequestDetailsDto> Handle(GetAdvertRequestByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            
            AdvertRequestDetailsDto advertRequestDetailsDto = await _advertRequestCacheRepository.GetByIdAsync(request.Id);

            return advertRequestDetailsDto;
        }
    }
}

 