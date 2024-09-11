using JurayKV.Application;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IPermissions
{
    [Authorize(Policy = Constants.SuperAdminPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<PermissionListDto> Roles = new List<PermissionListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            ListPermissionQuery command = new ListPermissionQuery();
            Roles = await _mediator.Send(command);

            return Page();
        }
    }
}
