using JurayKV.Application;
using JurayKV.Application.Commands.KvAdCommands;
using JurayKV.Application.Queries.KvAdQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IAds
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
        public KvAdDetailsDto UpdateKvAd { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetKvAdByIdQuery command = new GetKvAdByIdQuery(id);
                UpdateKvAd = await _mediator.Send(command);

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
