using JurayKV.Infrastructure.Flutterwave.Dtos;
using JurayKV.Infrastructure.Flutterwave.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Flutterwaves
{
    public sealed class VerifyTransactionQuery : IRequest<FlutterTransactionVerifyDto>
    {
        public VerifyTransactionQuery(string tx_ref)
        {
            Tx_ref = tx_ref;
        }

        public string Tx_ref { get; }
        public class VerifyTransactionQueryHandler : IRequestHandler<VerifyTransactionQuery, FlutterTransactionVerifyDto>
        {
            private readonly IFlutterTransactionService _repositoryService;

            public VerifyTransactionQueryHandler(IFlutterTransactionService repositoryService)
            {
                _repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
            }

            public async Task<FlutterTransactionVerifyDto> Handle(VerifyTransactionQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));


                FlutterTransactionVerifyDto repositories = await _repositoryService.VerifyTransaction(request.Tx_ref);

                return repositories;
            }

        }
    }

}
