using JurayKV.Application;
using JurayKV.Application.Commands.SliderCommands;
using JurayKV.Application.Queries.SliderQueries;
using JurayKV.Domain.Aggregates.SliderAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ISlider
{

    [Authorize(Policy = Constants.SliderPolicy)]
    public class AddModel : PageModel
    {

        private readonly IMediator _mediator;
        public AddModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public Slider Slider { get; set; }

        [BindProperty]
        public IFormFile? imagefile { get; set; }
        [BindProperty]
        public IFormFile? imagefile2 { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                CreateSliderCommand command = new CreateSliderCommand(Slider, imagefile, imagefile2);
                await _mediator.Send(command);
                TempData["success"] = "Added Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new data";
            }
            return RedirectToPage("./Index");
        }
    }
}
