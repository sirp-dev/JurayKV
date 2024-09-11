using JurayKV.Domain.ValueObjects;
using JurayKV.Infrastructure.Opay.core.Repositories;
using JurayKV.Infrastructure.Opay.core.Request;
using JurayKV.Infrastructure.Opay.core.Response;
using JurayKvV.Infrastructure.Interswitch.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Pages
{
    public class Pay2Model : PageModel
    {
        private readonly ILogger<Pay2Model> _logger;
        private readonly IMediator _mediator;
        private readonly IOpayRepository _opayRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISwitchRepository _switchRepository;
        public Pay2Model(ILogger<Pay2Model> logger, IMediator mediator, IOpayRepository opayRepository, IHttpContextAccessor httpContextAccessor, ISwitchRepository switchRepository)
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
            var httpContext = _httpContextAccessor.HttpContext;

            try
            {
                //    //CreateTransactionTransferQuery command = new CreateTransactionTransferQuery();
                string reff = Guid.NewGuid().ToString();
                //TransactionRequest model = new TransactionRequest
                //{
                //    country = "NG",
                //    reference = reff,
                //    amount = new TransactionRequestAmount
                //    {
                //        total = 130000,
                //        currency = "NGN"
                //    },
                //    returnUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/verify?reference=" + reff + "&orderNo=232432",
                //    callbackUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/verify",
                //    cancelUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/verify",
                //    displayName = "sub merchant name",
                //    expireAt = 300,
                //    sn = "PE462xxxxxxxx",
                //    userInfo = new TransactionRequestUserinfo
                //    {
                //        userEmail = "test@email.com",
                //        userId = "userid001",
                //        userMobile = "+23488889999",
                //        userName = "David"
                //    },
                //    product = new TransactionRequestProduct
                //    {
                //        description = "description",
                //        name = "name"
                //    },
                //    payMethod = ""



                //};
                // BankListResponse outcome = await _opayRepository.getBankList();
                //if (outcome.message == "SUCCESSFUL")
                //{
                //    return Redirect(outcome.data.cashierUrl);
                //}
                //CreateInterswitchTransactionQuery command = new CreateInterswitchTransactionQuery();
                //Outcome = await _mediator.Send(command);

                //VerifyAccountRequest model = new VerifyAccountRequest();
                //     model.bankAccountNo = "0690181179";
                //model.countryCode = "NG";
                //model.bankCode = "044";
                //VerifyAccountResponse outcome = await _opayRepository.verifyBankAccount(model);


                //TransferToBankRequest bmodel = new TransferToBankRequest
                //{
                //    amount = "200",
                //    country = "NG",
                //    reference = Guid.NewGuid().ToString(),
                //    currency = "NGN",
                //    reason = "tr tr",
                //    receiver = new TransferToBankReceiver
                //    {
                //        name = outcome.data.accountName.ToString(),
                //        bankAccountNumber = outcome.data.accountNo.ToString(),
                //        bankCode = "044",
                //    }
                //};

                //TransferToBankResponse xoutcome = await _opayRepository.transferToBank(bmodel);
                //var oiu = await _switchRepository.GetBillersCategories();
                return Page();

            }
            catch (Exception c)
            {
                return Page();
            }
        }
    }
}
