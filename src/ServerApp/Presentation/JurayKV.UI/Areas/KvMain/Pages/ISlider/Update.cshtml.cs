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
    public class UpdateModel : PageModel
    {

        private readonly IMediator _mediator;
        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public SliderDetailsDto Slider { get; set; }

        [BindProperty]
        public IFormFile? imagefile { get; set; }
        [BindProperty]
        public IFormFile? imagefile2 { get; set; }

        [BindProperty]
        public bool RemoveImage { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                UpdateSliderCommand command = new UpdateSliderCommand(Slider, imagefile, imagefile2, RemoveImage);
                await _mediator.Send(command);
                TempData["success"] = "Updated Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
