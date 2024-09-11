using JurayKV.Application.Commands.TransactionCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static JurayKV.Domain.Primitives.Enum;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using JurayKV.Application;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using Microsoft.AspNetCore.Identity;
using JurayKV.Application.Caching.Repositories;

namespace JurayKV.UI.Areas.KvMain.Pages.IUsers
{
    [Authorize(Policy = Constants.ValidatorPolicy)]
    public class ValidateTransactionModel : PageModel
    {

        private readonly IMediator _mediator; 
        private readonly IUserManagerCacheRepository _userManager;

        public ValidateTransactionModel(IMediator mediator, IUserManagerCacheRepository userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [BindProperty]
        public CommandDto Command { get; set; } = new CommandDto();
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

        public WalletDetailsDto walet {  get; set; }

        public string Name { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid userId)
        {
            GetWalletUserByIdQuery commandwallet = new GetWalletUserByIdQuery(userId);
             walet = await _mediator.Send(commandwallet);
            if (walet == null)
            {
                return RedirectToPage("./Index");
            }

              
                Command.UserId = walet.UserId;
                Command.WalletId = walet.Id;
                Command.TrackCode = Guid.NewGuid().ToString();
                Command.TransactionReference = Guid.NewGuid().ToString();
            var user = await _userManager.GetByIdAsync(walet.UserId);
            Name = user.Fullname;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                CreateReconsileTransactionCommand command = new CreateReconsileTransactionCommand(Command.WalletId, Command.UserId,
                    Command.UniqueReference, Command.OptionalNote, Command.Amount, Command.TransactionType, Command.Status, Command.TransactionReference, Command.Description,
                    Command.TrackCode);
                bool Result = await _mediator.Send(command);
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
