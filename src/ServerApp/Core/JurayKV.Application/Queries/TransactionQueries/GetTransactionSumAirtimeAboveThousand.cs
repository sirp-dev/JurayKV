using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.TransactionQueries
{
    public sealed class GetTransactionSumAirtimeAboveThousand : IRequest<bool>
    {
        public GetTransactionSumAirtimeAboveThousand(string uniqueReferrence, Guid userId)
        {
            UniqueReferrence = uniqueReferrence;
            UserId = userId;
        }

        public string UniqueReferrence { get; }
        public Guid UserId { get; }

        // Handler
        private class GetTransactionSumAirtimeAboveThousandHandler : IRequestHandler<GetTransactionSumAirtimeAboveThousand, bool>
        {
            private readonly ITransactionCacheRepository _transactionCacheRepository;

            public GetTransactionSumAirtimeAboveThousandHandler(IQueryRepository repository, ITransactionCacheRepository transactionCacheRepository)
            {
                _transactionCacheRepository = transactionCacheRepository;
            }

            public async Task<bool> Handle(GetTransactionSumAirtimeAboveThousand request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));


                bool transactionDetailsDto = await _transactionCacheRepository.CheckTransactionAboveTieOne(request.UniqueReferrence, request.UserId);

                return transactionDetailsDto;
            }
        }
    }

}
