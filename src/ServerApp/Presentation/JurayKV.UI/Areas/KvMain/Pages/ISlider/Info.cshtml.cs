using JurayKV.Application;
using JurayKV.Application.Commands.SliderCommands;
using JurayKV.Application.Queries.SliderQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ISlider
{
    [Authorize(Policy = Constants.SliderPolicy)]
    public class InfoModel : PageModel
    {

        private readonly IMediator _mediator;
        public InfoModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public SliderDetailsDto Slider { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetSliderByIdQuery command = new GetSliderByIdQuery(id);
                Slider = await _mediator.Send(command);

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
