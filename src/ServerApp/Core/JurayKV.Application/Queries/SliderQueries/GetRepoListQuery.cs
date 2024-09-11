using JurayKV.Infrastructure.Flutterwave.Dtos;
using JurayKV.Infrastructure.Flutterwave.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.SliderQueries
{
    public sealed class GetRepoListQuery : IRequest<FlutterResponseDto>
    {
        public class GetRepoListQueryHandler : IRequestHandler<GetRepoListQuery, FlutterResponseDto>
        {
            private readonly IFlutterTransactionService _repositoryService;

            public GetRepoListQueryHandler(IFlutterTransactionService repositoryService)
            {
                _repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
            }

            public async Task<FlutterResponseDto> Handle(GetRepoListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                PaymentRequestDto model = new PaymentRequestDto();
                model.Currency = "NGN";
                model.TxRef = "354rty";
                model.Amount = "300";
                model.PaymentOptions = "card";
                model.ConsumerId = Guid.NewGuid();
                model.ConsumerMac = "wewee";
                model.Email = "onwukaemeka41@gmail.com";
                model.PhoneNumber = "1234567890";
                model.Name = "Peter E";
                model.Title = "Koboview";
                model.Description = "kv";
                model.Logo = "";
              model.RedirectUrl = "https://localhost:7090/pay";

                FlutterResponseDto repositories = await _repositoryService.InitializeTransaction(model);
                
                return repositories;
            }

         }
    }
}
