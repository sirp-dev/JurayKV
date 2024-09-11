using Azure.Core;
using JurayKV.Application.Flutterwaves;
using JurayKV.Application.Interswitch;
using JurayKV.Application.Queries.SliderQueries;
using JurayKV.Domain.Aggregates.SliderAggregate;
using JurayKV.Infrastructure.Flutterwave.Models;
using JurayKV.Infrastructure.Opay.core.Repositories;
using JurayKV.Infrastructure.Opay.core.Request;
using JurayKV.Infrastructure.Opay.core.Response;
using JurayKvV.Infrastructure.Interswitch.Repositories;
using JurayKvV.Infrastructure.Interswitch.RequestModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;

namespace JurayKV.UI.Pages
{
    public class PayModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly IOpayRepository _opayRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISwitchRepository _switchRepository;
        public PayModel(ILogger<IndexModel> logger, IMediator mediator, IOpayRepository opayRepository, IHttpContextAccessor httpContextAccessor, ISwitchRepository switchRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _opayRepository = opayRepository;
            _httpContextAccessor = httpContextAccessor;
            _switchRepository = switchRepository;
        }
        public string Outcome { get; set; } 
        public async Task<IActionResult> OnGetAsync()
        {
            //var httpContext = _httpContextAccessor.HttpContext;

            //try
            //{
            //    //    //CreateTransactionTransferQuery command = new CreateTransactionTransferQuery();
            //    string reff = Guid.NewGuid().ToString();
            //    TransactionRequest model = new TransactionRequest
            //    {
            //        country = "NG",
            //        reference = reff,
            //        amount = new TransactionRequestAmount
            //        {
            //            total = 130000,
            //            currency = "NGN"
            //        },
            //        returnUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/verify?reference="+reff + "&orderNo=232432",
            //        callbackUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/verify",
            //        cancelUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/verify",
            //        displayName = "sub merchant name",
            //        expireAt = 300,
            //        sn = "PE462xxxxxxxx",
            //        userInfo = new TransactionRequestUserinfo
            //        {
            //            userEmail = "test@email.com",
            //            userId = "userid001",
            //            userMobile = "+23488889999",
            //            userName = "David"
            //        },
            //        product = new TransactionRequestProduct
            //        {
            //            description = "description",
            //            name = "name"
            //        },
            //        payMethod = ""



            //    };
            //    TransactionResponse outcome = await _opayRepository.initializeTransaction(model);
            //     if(outcome.message == "SUCCESSFUL")
            //    {
            //        return Redirect(outcome.data.cashierUrl);
            //    }
            //    //CreateInterswitchTransactionQuery command = new CreateInterswitchTransactionQuery();
            //    //Outcome = await _mediator.Send(command);


            //    return Page();

            //}
            //catch (Exception c)
            //{
            //    return Page();
            //}

            var result = await _switchRepository.ValidateBankAccount("0690181179", "044");

            //var res22 = await _switchRepository.GetBillersByCategory(result.BillerList.Category.FirstOrDefault().Id.ToString());
            return Page();
        }
    }
}
