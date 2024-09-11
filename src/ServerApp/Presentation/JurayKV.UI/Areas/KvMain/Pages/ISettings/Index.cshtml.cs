using JurayKV.Application;
using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ISetting
{
    [Authorize(Policy = Constants.AdminPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<SettingDetailsDto> Settingx = new List<SettingDetailsDto>();
        public SettingDetailsDto Setting { get; set; }
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetSettingListQuery command = new GetSettingListQuery();
            Settingx= await _mediator.Send(command);


            GetSettingDefaultQuery settingcommand = new GetSettingDefaultQuery();
            Setting = await _mediator.Send(settingcommand);

            return Page();
        }
    }
}
