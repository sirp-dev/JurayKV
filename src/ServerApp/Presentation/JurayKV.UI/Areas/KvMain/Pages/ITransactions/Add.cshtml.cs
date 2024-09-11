using JurayKV.Application;
using JurayKV.Application.Commands.TransactionCommands;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.KvMain.Pages.ITransactions
{

    [Authorize(Policy = Constants.Transaction)]
    public class AddModel : PageModel
    {

        private readonly IMediator _mediator;
        public AddModel(IMediator mediator)
        {
            _mediator = mediator;
        }

         

        [BindProperty]
        public CommandDto Command { get; set; }
        public class CommandDto
        {
            public Guid WalletId { get; set; }
           
            public Guid UserId { get; set; }
            public string UniqueReference { get; set; }
            public string OptionalNote { get; set; }
            public decimal Amount { get; set; }

            public TransactionTypeEnum TransactionType { get; set; }
            public EntityStatus Status { get; set; }
            public string TransactionReference { get; set; }
            public string Description { get; set; }
            public string TrackCode { get; set; }
        }
        public async Task<IActionResult> OnGetAsync(Guid walletId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // Get the user's ID claim

            if (userIdClaim != null)
            {
                string userId = userIdClaim.Value;
                Command.UserId = Guid.Parse(userId);
                Command.WalletId = walletId;
                Command.TrackCode = Guid.NewGuid().ToString();
                Command.TransactionReference = Guid.NewGuid().ToString();
            }
                return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            try
            {
                CreateTransactionCommand command = new CreateTransactionCommand(Command.WalletId, Command.UserId,
                    Command.UniqueReference, Command.OptionalNote, Command.Amount, Command.TransactionType, Command.Status, Command.TransactionReference, Command.Description,
                    Command.TrackCode);
                Guid Result = await _mediator.Send(command);
                TempData["success"] = "Added Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new Transaction";
            }
            return RedirectToPage("./Index");
        }
    }
}
