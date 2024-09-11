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
    public class AddModel : PageModel
    {

        private readonly IMediator _mediator;
        public AddModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public ImageFile Image { get; set; }

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
                CreateImageCommand command = new CreateImageCommand(Image, imagefile);
                await _mediator.Send(command);
                TempData["success"] = "Added Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
