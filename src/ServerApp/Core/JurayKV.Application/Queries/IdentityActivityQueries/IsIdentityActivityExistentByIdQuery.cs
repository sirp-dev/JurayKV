using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.IdentityActivityQueries;

public sealed class IsIdentityActivityExistentByIdQuery : IRequest<bool>
{
    public IsIdentityActivityExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsIdentityActivityExistentByIdQueryHandler : IRequestHandler<IsIdentityActivityExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsIdentityActivityExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsIdentityActivityExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<IdentityActivity>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
