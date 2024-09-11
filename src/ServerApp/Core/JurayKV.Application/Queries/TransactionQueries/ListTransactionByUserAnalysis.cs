using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.UserManagerQueries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.TransactionQueries
{
    public sealed class ListTransactionByUserAnalysis : IRequest<List<ListTransactionDto>>
    {

        public ListTransactionByUserAnalysis(int pageSize, int pageNumber, string? searchString, int sortOrder)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            SearchString = searchString;
            SortOrder = sortOrder;
        }


        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchString { get; set; }
        public int SortOrder { get; set; }
        private class ListTransactionByUserAnalysisHandler : IRequestHandler<ListTransactionByUserAnalysis, List<ListTransactionDto>>
        {
            private readonly ITransactionCacheRepository _transaction;

            public ListTransactionByUserAnalysisHandler(ITransactionCacheRepository transaction)
            {
                _transaction = transaction;
            }

            public async Task<List<ListTransactionDto>> Handle(ListTransactionByUserAnalysis request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                var data = await _transaction.ListUserTransactions(request.PageSize, request.PageNumber, request.SearchString, request.SortOrder);

                return data;
            }
        }
    }
}
