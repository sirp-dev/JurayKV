using JurayKV.Domain.Aggregates.KvAdAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.KvAdQueries;

public sealed class IsKvAdExistentByIdQuery : IRequest<bool>
{
    public IsKvAdExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsKvAdExistentByIdQueryHandler : IRequestHandler<IsKvAdExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsKvAdExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsKvAdExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<KvAd>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
