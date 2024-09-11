using JurayKV.Application;
using JurayKV.Application.Queries.ExchangeRateQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IRates
{

    [Authorize(Policy = Constants.Transaction)]
    public class InfoModel : PageModel
    {

        private readonly IMediator _mediator;
        public InfoModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public ExchangeRateDetailsDto UpdateExchangeRate { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetExchangeRateByIdQuery command = new GetExchangeRateByIdQuery(id);
                UpdateExchangeRate = await _mediator.Send(command);

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
