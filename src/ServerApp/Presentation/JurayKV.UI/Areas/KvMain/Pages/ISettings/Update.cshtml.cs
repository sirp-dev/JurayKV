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
    public class UpdateModel : PageModel
    {

        private readonly IMediator _mediator;
        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public SettingDetailsDto Setting { get; set; }
         
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetSettingByIdQuery command = new GetSettingByIdQuery(id);
                Setting = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                UpdateSettingCommand command = new UpdateSettingCommand(Setting);
                await _mediator.Send(command);
                TempData["success"] = "Updated Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new setting";
            }
            return RedirectToPage("./Index");
        }
    }
}
