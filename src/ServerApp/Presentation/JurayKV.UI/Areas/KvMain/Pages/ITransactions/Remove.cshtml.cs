using JurayKV.Application;
using JurayKV.Application.Commands.TransactionCommands;
using JurayKV.Application.Queries.TransactionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ITransactions
{
    [Authorize(Policy = Constants.Transaction)]
    public class RemoveModel : PageModel
    {

        private readonly IMediator _mediator;
        public RemoveModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public TransactionDetailsDto UpdateTransaction { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetTransactionByIdQuery command = new GetTransactionByIdQuery(id);
                UpdateTransaction = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch Transaction";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                DeleteTransactionCommand command = new DeleteTransactionCommand(UpdateTransaction.Id);
                await _mediator.Send(command);
                TempData["success"] = "Deleted Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. removing new Transaction";
            }
            return RedirectToPage("./Index");
        }
    }
}
