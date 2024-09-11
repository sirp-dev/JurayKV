using JurayKV.Application;
using JurayKV.Application.Commands.SettingCommands;
using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Domain.Aggregates.SettingAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ISetting
{

    [Authorize(Policy = Constants.AdminPolicy)]
    public class AddModel : PageModel
    {

        private readonly IMediator _mediator;
        public AddModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public Setting Setting { get; set; }
         
        public async Task<IActionResult> OnGetAsync()
        {

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                CreateSettingCommand command = new CreateSettingCommand(Setting);
                await _mediator.Send(command);
                TempData["success"] = "Added Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new Setting";
            }
            return RedirectToPage("./Index");
        }
    }
}
