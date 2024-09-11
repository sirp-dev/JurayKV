using JurayKV.Infrastructure.Flutterwave.Dtos;
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
    public sealed class CreateTransactionQuery : IRequest<FlutterResponseDto>
    {
        public class CreateTransactionQueryHandler : IRequestHandler<CreateTransactionQuery, FlutterResponseDto>
        {
            private readonly IFlutterTransactionService _repositoryService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CreateTransactionQueryHandler(IFlutterTransactionService repositoryService, IHttpContextAccessor httpContextAccessor)
            {
                _repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<FlutterResponseDto> Handle(CreateTransactionQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                var httpContext = _httpContextAccessor.HttpContext;

                PaymentRequestDto model = new PaymentRequestDto();
                model.Currency = "NGN";
                model.TxRef = "354rxxddtyss";
                model.Amount = "100";
                model.PaymentOptions = "card";
                model.ConsumerId = Guid.NewGuid();
                model.ConsumerMac = "wewee";
                model.Email = "onwukaemeka41@gmail.com";
                model.PhoneNumber = "1234567890";
                model.Name = "Peter E";    
                model.Title = "Koboview";
                model.Description = "kv";
                model.Logo = "";
                model.RedirectUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/verify";
 
                FlutterResponseDto repositories = await _repositoryService.InitializeTransaction(model);

                return repositories;
            }

        }
    }

}
