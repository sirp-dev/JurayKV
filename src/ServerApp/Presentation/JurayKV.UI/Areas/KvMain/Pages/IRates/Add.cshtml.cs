using JurayKV.Application;
using JurayKV.Application.Commands.ExchangeRateCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IRates
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
        public decimal Amount { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {

                CreateExchangeRateCommand command = new CreateExchangeRateCommand(Amount);
                Guid Result = await _mediator.Send(command);
                TempData["success"] = "Added Successfuly";

            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
