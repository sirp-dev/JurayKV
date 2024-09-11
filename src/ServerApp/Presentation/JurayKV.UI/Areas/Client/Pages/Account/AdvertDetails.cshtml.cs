using JurayKV.Application;
using JurayKV.Application.Queries.AdvertRequestQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.Client.Pages.Account
{
     [Authorize(Policy = Constants.CompanyPolicy)]
    public class AdvertDetailsModel : PageModel
    {

        private readonly IMediator _mediator;
        public AdvertDetailsModel(IMediator mediator)
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
