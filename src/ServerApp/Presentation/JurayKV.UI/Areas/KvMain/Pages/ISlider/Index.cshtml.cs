using JurayKV.Application;
using JurayKV.Application.Queries.SliderQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ISlider
{
    [Authorize(Policy = Constants.SliderPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<SliderDetailsDto> Slider = new List<SliderDetailsDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetSliderListQuery command = new GetSliderListQuery();
            Slider = await _mediator.Send(command);

            return Page();
        }
    }
}
