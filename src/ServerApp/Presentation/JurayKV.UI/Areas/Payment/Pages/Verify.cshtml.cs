using JurayKV.Application.Commands.TransactionCommands;
using JurayKV.Application.Flutterwaves;
using JurayKV.Application.Queries.TransactionQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.Payment.Pages
{
    public class VerifyModel : PageModel
    {
        private readonly ILogger<VerifyModel> _logger;
        private readonly IMediator _mediator;

        public VerifyModel(ILogger<VerifyModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var tranxRef = HttpContext.Request.Query["tx_ref"].ToString();
            var transaction_id = HttpContext.Request.Query["transaction_id"].ToString();
            var status = HttpContext.Request.Query["status"].ToString();
            var areapath = HttpContext.Request.Query["areapath"].ToString();
            VerifyTransactionQuery command = new VerifyTransactionQuery(transaction_id);
            //var response = await _mediator.Send(command);
            //if (response.status == "success")
            //{
            //    GetTransactionByIdQuery getcommand = new GetTransactionByIdQuery(Guid.Parse(tranxRef));
            //    var Transactionresponse = await _mediator.Send(getcommand);


            //    ValidateAndUpdateTransactionCommand validateCommand = new ValidateAndUpdateTransactionCommand(Transactionresponse.Id, transaction_id, EntityStatus.Successful);
            //    TransactionResponseDto outcome = await _mediator.Send(validateCommand);
            //    if (outcome.Success)
            //    {
            //        TempData["success"] = "Successful";
            //        return RedirectToPage("/Account/Index", new { area = "client" });
            //        //return RedirectToPage(outcome.Path, new { area = areapath });
            //    }
            //    else
            //    {
            //        TempData["error"] = "Unable to Complete Process";
            //        return RedirectToPage(outcome.Path, new { area = "client" });
            //    }

            //}
            TempData["error"] = "Unable to Process Completely";
            return RedirectToPage("/Account/Index", new {area= areapath });
        }
    }
}
