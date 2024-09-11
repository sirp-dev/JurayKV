using JurayKV.Application;
using JurayKV.Application.Queries.AdvertRequestQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IAdvertRequests
{
    [Authorize(Policy = Constants.AdvertPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<AdvertRequestListDto> AdvertRequests = new List<AdvertRequestListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetAdvertRequestListQuery command = new GetAdvertRequestListQuery();
            AdvertRequests = await _mediator.Send(command);

            return Page();
        }
    }
}
