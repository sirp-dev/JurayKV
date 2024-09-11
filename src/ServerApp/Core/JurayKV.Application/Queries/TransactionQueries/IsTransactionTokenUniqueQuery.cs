using JurayKV.Domain.Aggregates.TransactionAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.TransactionQueries;

public sealed class IsTransactionTokenUniqueQuery : IRequest<bool>
{
    public IsTransactionTokenUniqueQuery(Guid transactionId, string token)
    {
        Id = transactionId.ThrowIfEmpty(nameof(transactionId));
        Token = token.ThrowIfNullOrEmpty(nameof(token));
    }

    public Guid Id { get; }

    public string Token { get; }

    private class IsTransactionNameUniqueQueryHandler : IRequestHandler<IsTransactionTokenUniqueQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsTransactionNameUniqueQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsTransactionTokenUniqueQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExistent = await _repository.ExistsAsync<Transaction>(d => d.Id != request.Id && d.TrackCode == request.Token, cancellationToken);
            return !isExistent;
        }
    }
}
