using JurayKV.Application;
using JurayKV.Application.Queries.ImageQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ImagePage
{
    [Authorize(Policy = Constants.AdminPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<ImageDto> Image = new List<ImageDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetImageListQuery command = new GetImageListQuery();
            Image = await _mediator.Send(command);

            return Page();
        }
    }
}
