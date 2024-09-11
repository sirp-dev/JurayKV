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
    public class UpdateModel : PageModel
    {

        private readonly IMediator _mediator;
        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public TransactionDetailsDto Command { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetTransactionByIdQuery command = new GetTransactionByIdQuery(id);
                Command = await _mediator.Send(command);

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
                UpdateTransactionCommand command = new UpdateTransactionCommand(Command.Id,
Command.WalletId, Command.UserId,
                    Command.UniqueReference, Command.OptionalNote, Command.Amount, Command.TransactionType, Command.Status, Command.TransactionReference, Command.Description,
                    Command.TrackCode);
                await _mediator.Send(command);
                TempData["success"] = "Updated Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new Transaction";
            }
            return RedirectToPage("./Index");
        }
    }
}
