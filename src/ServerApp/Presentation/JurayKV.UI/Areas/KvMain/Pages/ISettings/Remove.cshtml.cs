using JurayKV.Application;
using JurayKV.Application.Commands.SettingCommands;
using JurayKV.Application.Queries.SettingQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ISetting
{
    [Authorize(Policy = Constants.AdminPolicy)]
    public class RemoveModel : PageModel
    {

        private readonly IMediator _mediator;
        public RemoveModel(IMediator mediator)
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
                TempData["error"] = "unable to fetch setting";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                DeleteSettingCommand command = new DeleteSettingCommand(Setting.Id);
                await _mediator.Send(command);
                TempData["success"] = "Deleted Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. removing new setting";
            }
            return RedirectToPage("./Index");
        }
    }
}
