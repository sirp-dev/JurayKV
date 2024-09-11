using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.TransactionQueries
{
    public sealed class GetTransactionListByUserQuery : IRequest<List<TransactionListDto>>
    {
        public GetTransactionListByUserQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
        private class GetTransactionListByUserQueryHandler : IRequestHandler<GetTransactionListByUserQuery, List<TransactionListDto>>
        {
            private readonly ITransactionCacheRepository _transactionCacheRepository;

            public GetTransactionListByUserQueryHandler(ITransactionCacheRepository transactionCacheRepository)
            {
                _transactionCacheRepository = transactionCacheRepository;
            }

            public async Task<List<TransactionListDto>> Handle(GetTransactionListByUserQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<TransactionListDto> transactionDtos = await _transactionCacheRepository.GetListByUserIdAsync(request.UserId);
                return transactionDtos;
            }
        }
    }

}
