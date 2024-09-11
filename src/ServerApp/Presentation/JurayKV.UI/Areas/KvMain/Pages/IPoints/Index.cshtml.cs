using JurayKV.Application;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IPoints
{
    [Authorize(Policy = Constants.PointPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<KvPointListDto> KvPoints = new List<KvPointListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetKvPointListQuery command = new GetKvPointListQuery();
            KvPoints = await _mediator.Send(command);

            return Page();
        }
    }
}
