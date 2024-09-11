using JurayKV.Application;
using JurayKV.Application.Commands.ImageCommands;
using JurayKV.Application.Queries.ImageQueries;
using JurayKV.Domain.Aggregates.ImageAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ImagePage
{
    [Authorize(Policy = Constants.AdminPolicy)]
    public class UpdateModel : PageModel
    {

        private readonly IMediator _mediator;
        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public ImageDto Image { get; set; }

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
                GetImageByIdQuery command = new GetImageByIdQuery(id);
                Image = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch image";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                UpdateImageCommand command = new UpdateImageCommand(Image, imagefile);
                await _mediator.Send(command);
                TempData["success"] = "Updated Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new image";
            }
            return RedirectToPage("./Index");
        }
    }
}
