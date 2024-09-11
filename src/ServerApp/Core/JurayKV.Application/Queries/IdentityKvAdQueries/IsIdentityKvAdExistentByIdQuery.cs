using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.IdentityKvAdQueries;

public sealed class IsIdentityKvAdExistentByIdQuery : IRequest<bool>
{
    public IsIdentityKvAdExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsIdentityKvAdExistentByIdQueryHandler : IRequestHandler<IsIdentityKvAdExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsIdentityKvAdExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsIdentityKvAdExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<IdentityKvAd>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
