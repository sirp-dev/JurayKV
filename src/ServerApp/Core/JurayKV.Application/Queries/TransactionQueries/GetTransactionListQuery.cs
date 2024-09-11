using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.TransactionQueries;

public sealed class GetTransactionListQuery : IRequest<List<TransactionListDto>>
{
    private class GetTransactionListQueryHandler : IRequestHandler<GetTransactionListQuery, List<TransactionListDto>>
    {
        private readonly ITransactionCacheRepository _transactionCacheRepository;

        public GetTransactionListQueryHandler(ITransactionCacheRepository transactionCacheRepository)
        {
            _transactionCacheRepository = transactionCacheRepository;
        }

        public async Task<List<TransactionListDto>> Handle(GetTransactionListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<TransactionListDto> transactionDtos = await _transactionCacheRepository.GetListAsync();
            return transactionDtos;
        }
    }
}

