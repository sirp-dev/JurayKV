using JurayKV.Application;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IAds
{
    [Authorize(Policy = Constants.AdvertPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<KvAdListDto> KvAds = new List<KvAdListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetKvAdListQuery command = new GetKvAdListQuery();
            KvAds = await _mediator.Send(command);

            return Page();
        }
    }
}
