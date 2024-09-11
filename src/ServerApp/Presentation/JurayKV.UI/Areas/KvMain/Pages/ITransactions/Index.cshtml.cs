using JurayKV.Application;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ITransactions
{
    [Authorize(Policy = Constants.Transaction)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<TransactionListDto> Transactions = new List<TransactionListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetTransactionListQuery command = new GetTransactionListQuery();
            Transactions = await _mediator.Send(command);

            return Page();
        }
    }
}
