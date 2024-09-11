using JurayKV.Application;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvAdQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IUserAds
{
    [Authorize(Policy = Constants.PointPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<IdentityKvAdListDto> UserKvAds = new List<IdentityKvAdListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetActiveListTpdayIdentityKvAdListQuery command = new GetActiveListTpdayIdentityKvAdListQuery();
            UserKvAds = await _mediator.Send(command);

            return Page();
        }
    }
}
