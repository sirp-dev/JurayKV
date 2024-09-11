using JurayKV.Application;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IUserAds
{
     [Authorize(Policy = Constants.PointPolicy)]
    public class AllModel : PageModel
    {

        private readonly IMediator _mediator;
        public AllModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<IdentityKvAdListDto> UserKvAds = new List<IdentityKvAdListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetIdentityKvAdListQuery command = new GetIdentityKvAdListQuery();
            UserKvAds = await _mediator.Send(command);

            return Page();
        }
    }
}
