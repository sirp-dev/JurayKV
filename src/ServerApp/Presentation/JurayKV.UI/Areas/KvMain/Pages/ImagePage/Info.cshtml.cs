using JurayKV.Application;
using JurayKV.Application.Commands.ImageCommands;
using JurayKV.Application.Queries.ImageQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ImagePage
{
    [Authorize(Policy = Constants.AdminPolicy)]
    public class InfoModel : PageModel
    {

        private readonly IMediator _mediator;
        public InfoModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public ImageDto Image { get; set; }

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
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("/Index");
            }
        }

     }
}
