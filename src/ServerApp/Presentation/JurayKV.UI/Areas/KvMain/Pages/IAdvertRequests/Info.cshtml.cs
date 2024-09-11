using JurayKV.Application;
using JurayKV.Application.Commands.AdvertRequestCommands;
using JurayKV.Application.Queries.AdvertRequestQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IAdvertRequests
{
    [Authorize(Policy = Constants.AdvertPolicy)]
    public class InfoModel : PageModel
    {

        private readonly IMediator _mediator;
        public InfoModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public AdvertRequestDetailsDto UpdateAdvertRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetAdvertRequestByIdQuery command = new GetAdvertRequestByIdQuery(id);
                UpdateAdvertRequest = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch AdvertRequest";
                return RedirectToPage("/Index");
            }
        }

     }
}
