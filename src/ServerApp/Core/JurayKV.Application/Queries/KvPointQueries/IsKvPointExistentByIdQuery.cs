using JurayKV.Domain.Aggregates.KvPointAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.KvPointQueries;

public sealed class IsKvPointExistentByIdQuery : IRequest<bool>
{
    public IsKvPointExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsKvPointExistentByIdQueryHandler : IRequestHandler<IsKvPointExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsKvPointExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsKvPointExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<KvPoint>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
