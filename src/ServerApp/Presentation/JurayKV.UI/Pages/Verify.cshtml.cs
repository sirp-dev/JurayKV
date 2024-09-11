using JurayKV.Application.Flutterwaves;
using JurayKV.Application.Queries.SliderQueries;
using JurayKV.Infrastructure.Opay.core.Repositories;
using JurayKV.Infrastructure.Opay.core.Request;
using JurayKV.Infrastructure.Opay.core.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Pages
{
    public class VerifyModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly IOpayRepository _opayRepository;

        public VerifyModel(ILogger<IndexModel> logger, IMediator mediator, IOpayRepository opayRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _opayRepository = opayRepository;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var tranxRef = HttpContext.Request.Query["tx_ref"].ToString();
         
            var transaction_id = HttpContext.Request.Query["transaction_id"].ToString();
            var status = HttpContext.Request.Query["status"].ToString();



            var merchantcode = HttpContext.Request.Query["merchantcode"].ToString();
            var transactionreference = HttpContext.Request.Query["transactionreference"].ToString();
            var amount = HttpContext.Request.Query["amount"].ToString();



            var orderNo = HttpContext.Request.Query["orderNo"].ToString();
            var country = HttpContext.Request.Query["NG"].ToString();
            //TransactionStatusRequest model = new TransactionStatusRequest
            //{
            //    country = "NG",
            //    reference = reference,
            //};
            //TransactionStatusResponse outcome = await _opayRepository.transactionStatus(model);

            // Perform any necessary processing based on the payment status

            return Page();
        }
    }
   
}
