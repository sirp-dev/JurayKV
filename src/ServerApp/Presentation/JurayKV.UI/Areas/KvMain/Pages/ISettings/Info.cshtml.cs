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
    public class InfoModel : PageModel
    {

        private readonly IMediator _mediator;
        public InfoModel(IMediator mediator)
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

     }
}
