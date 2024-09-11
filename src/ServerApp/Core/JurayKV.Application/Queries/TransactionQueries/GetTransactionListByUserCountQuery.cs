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
    public sealed class GetTransactionListByUserCountQuery : IRequest<List<TransactionListDto>>
    {
        public GetTransactionListByUserCountQuery(Guid userId, int count)
        {
            UserId = userId;
            Count = count;
        }

        public Guid UserId { get; }
        public int Count { get; }
        private class GetTransactionListByUserCountQueryHandler : IRequestHandler<GetTransactionListByUserCountQuery, List<TransactionListDto>>
        {
            private readonly ITransactionCacheRepository _transactionCacheRepository;

            public GetTransactionListByUserCountQueryHandler(ITransactionCacheRepository transactionCacheRepository)
            {
                _transactionCacheRepository = transactionCacheRepository;
            }

            public async Task<List<TransactionListDto>> Handle(GetTransactionListByUserCountQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<TransactionListDto> transactionDtos = await _transactionCacheRepository.GetListByUserIdAsync(request.UserId, request.Count);
                return transactionDtos;
            }
        }
    }


}
