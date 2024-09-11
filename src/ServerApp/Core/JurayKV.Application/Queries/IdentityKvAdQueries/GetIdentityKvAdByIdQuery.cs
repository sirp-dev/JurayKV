using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.IdentityKvAdQueries;

public sealed class GetIdentityKvAdByIdQuery : IRequest<IdentityKvAdDetailsDto>
{
    public GetIdentityKvAdByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetIdentityKvAdByIdQueryHandler : IRequestHandler<GetIdentityKvAdByIdQuery, IdentityKvAdDetailsDto>
    {
        private readonly IIdentityKvAdCacheRepository _departmentCacheRepository;

        public GetIdentityKvAdByIdQueryHandler(IIdentityKvAdCacheRepository departmentCacheRepository)
        {
            _departmentCacheRepository = departmentCacheRepository;
        }

        public async Task<IdentityKvAdDetailsDto> Handle(GetIdentityKvAdByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            IdentityKvAdDetailsDto departmentDetailsDto = await _departmentCacheRepository.GetByIdAsync(request.Id);

            return departmentDetailsDto;
        }
    }
}

