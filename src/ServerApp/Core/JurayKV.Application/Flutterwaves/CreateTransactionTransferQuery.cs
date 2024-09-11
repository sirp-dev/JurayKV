using JurayKV.Infrastructure.Flutterwave.Dtos;
using JurayKV.Infrastructure.Flutterwave.Models;
using JurayKV.Infrastructure.Flutterwave.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Flutterwaves
{
        public sealed class CreateTransactionTransferQuery : IRequest<InitializeTransactionTransferDto>
    {
        public class CreateTransactionTransferQueryHandler : IRequestHandler<CreateTransactionTransferQuery, InitializeTransactionTransferDto>
        {
            private readonly IFlutterTransactionService _repositoryService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CreateTransactionTransferQueryHandler(IFlutterTransactionService repositoryService, IHttpContextAccessor httpContextAccessor)
            {
                _repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<InitializeTransactionTransferDto> Handle(CreateTransactionTransferQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                var httpContext = _httpContextAccessor.HttpContext;

                TransactionTransferInitialize model = new TransactionTransferInitialize();
                model.currency = "NGN";
                model.tx_ref = "354rty";
                model.amount = "300"; 
                model.client_ip = "154.123.220.1";
                model.device_fingerprint = "wewee";
                model.email = "onwukaemeka41@gmail.com";
                model.phone_number = "1234567890"; 
                model.narration = "Koboview";
                model.is_permanent = false;

                InitializeTransactionTransferDto repositories = await _repositoryService.InitializeTransactionTransfer(model);

                return repositories;
            }

        }
    }

}
