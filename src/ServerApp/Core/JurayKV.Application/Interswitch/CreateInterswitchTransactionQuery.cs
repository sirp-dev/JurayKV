using JurayKV.Infrastructure.Flutterwave.Dtos;
using JurayKV.Infrastructure.Flutterwave.Repositories;
using JurayKvV.Infrastructure.Interswitch.Repositories;
using JurayKvV.Infrastructure.Interswitch.RequestModel;
using JurayKvV.Infrastructure.Interswitch.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Interswitch
{
    public sealed class CreateInterswitchTransactionQuery : IRequest<string>
    {
        public class CreateInterswitchTransactionQueryHandler : IRequestHandler<CreateInterswitchTransactionQuery, string>
        {
            private readonly ISwitchRepository _repositoryService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CreateInterswitchTransactionQueryHandler(ISwitchRepository repositoryService, IHttpContextAccessor httpContextAccessor)
            {
                _repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<string> Handle(CreateInterswitchTransactionQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                var httpContext = _httpContextAccessor.HttpContext;

                PaymentRequest model = new PaymentRequest();
                model.merchant_code = "VNA";
                model.currency = "566";
                model.txn_ref = Guid.NewGuid().ToString();
                model.amount = 100;
                model.cust_id = Guid.NewGuid().ToString(); 
                model.cust_email = "onwukaemeka41@gmail.com";
                model.pay_item_id = "103";
                model.cust_name = "Peter E";
                model.pay_item_name = "Koboview";
                model.site_redirect_url = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/verify";

                //string repositories = await _repositoryService.Payment(model);

                return "";
            }

        }
    }

}
