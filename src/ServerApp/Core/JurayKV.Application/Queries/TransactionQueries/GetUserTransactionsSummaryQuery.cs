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

    public sealed class GetUserTransactionsSummaryQuery : IRequest<ListTransactionDto>
    {

        private class GetUserTransactionsSummaryQueryHandler : IRequestHandler<GetUserTransactionsSummaryQuery, ListTransactionDto>
        {
            private readonly ITransactionCacheRepository _transaction;

            public GetUserTransactionsSummaryQueryHandler(ITransactionCacheRepository transaction)
            {
                _transaction = transaction;
            }

            public async Task<ListTransactionDto> Handle(GetUserTransactionsSummaryQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                var data = await _transaction.GetUserTransactionsSummary();

                return data;
            }
        }
    }

}
