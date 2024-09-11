using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.TransactionQueries;

public sealed class GetTransactionByIdQuery : IRequest<TransactionDetailsDto>
{
    public GetTransactionByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDetailsDto>
    {
        private readonly ITransactionCacheRepository _transactionCacheRepository;

        public GetTransactionByIdQueryHandler(IQueryRepository repository, ITransactionCacheRepository transactionCacheRepository)
        {
            _transactionCacheRepository = transactionCacheRepository;
        }

        public async Task<TransactionDetailsDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            
            TransactionDetailsDto transactionDetailsDto = await _transactionCacheRepository.GetByIdAsync(request.Id);

            return transactionDetailsDto;
        }
    }
}

 