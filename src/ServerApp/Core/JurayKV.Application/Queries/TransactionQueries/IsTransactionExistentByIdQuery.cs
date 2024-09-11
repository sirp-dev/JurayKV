using JurayKV.Domain.Aggregates.TransactionAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.TransactionQueries;

public sealed class IsTransactionExistentByIdQuery : IRequest<bool>
{
    public IsTransactionExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsTransactionExistentByIdQueryHandler : IRequestHandler<IsTransactionExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsTransactionExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsTransactionExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<Transaction>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
